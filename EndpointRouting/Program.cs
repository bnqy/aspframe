using EndpointRouting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ProductService>();  //registers ProductService as a service


var app = builder.Build();

app.MapGet("/", (LinkGenerator links, HttpContext context) => GetHomePage(links, context));     // injecting DI

app.MapGet("/test", () => "Hello World!")
	.WithName("hello");  // gives a name "hello"

app.MapGet("/redirect-hello", () => Results.RedirectToRoute("hello")) // generates a url and redirects to "hello" endpoint
	.WithName("redirect");

app.MapHealthChecks("/healthcheck")
	.WithName("healthcheck"); // gives a name "healthcheck"

app.MapRazorPages();  // adds all razors as an endpoint

app.MapGet("/products/{name}", (string name) => $"Products name is {name}")
	.WithName("products");  // gives a name to endpoint by adding metadata to it

app.MapGet("/links", (LinkGenerator generator) =>
{
	string link = generator.GetPathByName("products", new { name = "big-widget" });    //generates:  /products/big-widget
	string link2 = generator.GetUriByName("products", 
		new { Name = "super-fancy-widget" }, "https", new HostString("localhost"));  /* https://localhost/products/super-fancy-widget */

	return $"View the product at {link} and \n\t{link2}";
}).WithName("Links");   // gives a name to endpoint

app.MapGet("/{name}", (ProductService service, string name) =>
{
	var product = service.GetProduct(name);
	return product is null
		? Results.NotFound()
		: Results.Ok(product);
}).WithName("product"); // gives a name to endpoint

app.Run();


static string GetHomePage(LinkGenerator links, HttpContext context)
{

	var healthcheck = links.GetPathByName("healthcheck");
	var helloWorld = links.GetPathByName("hello");
	var redirect = links.GetPathByName("redirect");
	var bigWidget = links.GetPathByName("product", new { Name = "big-widget" });
	var fancyWidget = links.GetUriByName(context, "product", new { Name = "super-fancy-widget" });
	var link = links.GetUriByName(context, "Links");

	return $@"Try navigating to one of the following paths:
    {healthcheck} (standard health check)
    {helloWorld} (Hello world! response)
    {redirect} (Redirects to the {helloWorld} endpoint)
    {bigWidget} or {fancyWidget} (returns the Product details)/not-a-product (returns a 404)
    {link} links";
}