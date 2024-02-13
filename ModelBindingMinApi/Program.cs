using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.ConfigureRouteHandlerJsonOptions(o => {
	o.SerializerOptions.AllowTrailingCommas = true;
	o.SerializerOptions.PropertyNamingPolicy =
   JsonNamingPolicy.CamelCase;
	o.SerializerOptions.PropertyNameCaseInsensitive = true;
});*/


var app = builder.Build();

app.MapGet("/", (LinkGenerator generator, HttpContext context) => GetHomePage(generator, context) + @"
/products/3?id=1
/products?id=3
/product/p221

");


// /products/3?id=1
// Received 3
app.MapGet("/products/{id?}", (int? id) => $"Received {id}").WithName("RouteParam");  //route param

// /products?id=3
// Received 3
app.MapGet("/products", (int id) => $"Received {id}").WithName("QuString");  // query string


app.MapGet("/product/{id}/paged", (
	[FromRoute] int id,  //enforces to bind to route value
	[FromQuery] int page,  //enforces to bind to query string
	[FromHeader(Name = "PageSize")] int pageSize  // binds the argument to the header
	) => $"Recieved id: {id}, page: {page}, pageSize: {pageSize}");


app.MapGet("/product/{id}", (ProductId id) => $"Product id: {id}").WithName("ProductId");   // ProductId automatically binds to route  values as it implements TryParse.

app.MapPost("/product", (Product product) => $"Received {product}");  // deserializes json to complex type(product)

//app.MapGet("/products/search", (int[] id) => $"Received {id.Length} ids");   // same with below
app.MapGet("/products/search", ([FromQuery(Name = "id")] int[] ids) => $"Received {ids.Length} ids . ids: {string.Join(", ", ids)}"); // same with above

app.MapPost("/products", (Product[] products) => 
$"Received {products.Length} items: {string.Join(", ", products.Select(x => x))}");

// /stock/3 works
// /stock works as well
app.MapGet("/stock/{id?}", (int? id) => $"Received {id}");  // id is null when not provided
app.MapGet("/stock2", (int? id) => $"Recieved: {id}");

app.Run();

static string GetHomePage(LinkGenerator links, HttpContext context)
{

	var routeParam = links.GetUriByName(context, "RouteParam", new {Id = 2});

	return $@"Try navigating to one of the following paths:
    {routeParam}";
}

record Product(int Id, string Name, int Stock);   // does not impl TryParse so it is complex type

readonly record struct ProductId(int Id)
	: IParsable<ProductId>
{
	static ProductId IParsable<ProductId>.Parse(string s, IFormatProvider? provider)
	{
		if (TryParse(s, provider, out var result))
		{
			return result;
		}

		throw new InvalidOperationException($"Cannot convert '{s}' to type {nameof(ProductId)}");
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ProductId result)
	{
		if (s is not null
			&& s.StartsWith('p')
			&& int.TryParse(s.AsSpan().Slice(1), out var id))
		{
			result = new ProductId(id);
			return true;
		}

		result = default;
		return false;
	}
}