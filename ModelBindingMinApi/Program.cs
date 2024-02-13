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

// LinkGenerator is registered in Di so can be used in the param
app.MapGet("/links", ([FromServices] LinkGenerator links) => $"The Links API can be found at {links.GetPathByName("LinksApi")}")
	.WithName("LinksApi");

app.MapGet("/well-known", (HttpContext httpContext) => httpContext.Response.WriteAsync("Hello World!"));

app.MapPost("/sizes", (SizeDetails sd) => $"Recieved {sd}");

app.MapGet("/category/{id}",
 ([AsParameters] SearchModel model) => $"Received {model}");   // asparam simplifies it like a DTO class

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

record SizeDetails(double Height, double Width)
{
	public static async ValueTask<SizeDetails?> BindAsync(HttpContext context)
	{
		using var sr = new StreamReader(context.Request.Body);

		var line1 = await sr.ReadLineAsync(context.RequestAborted);
		if (line1 is null)
		{
			return null;
		}
		var line2 = await sr.ReadLineAsync(context.RequestAborted);
		if (line2 is null)
		{
			return null;
		}

		return double.TryParse(line1, out var height)
			&& double.TryParse(line2, out var width)
			? new SizeDetails(height, width)
			: null;
	}
}

record struct SearchModel(
 int id,
 int page,
 [FromHeader(Name = "sort")] bool? sortAsc,
 [FromQuery(Name = "q")] string search);