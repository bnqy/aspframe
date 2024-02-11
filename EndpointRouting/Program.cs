var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();


var app = builder.Build();



app.MapGet("/test", () => "Hello World!")
	.WithName("hello");  // gives a name "hello"

app.MapGet("/redirect-hello", () => Results.RedirectToRoute("hello"));  // generates a url and redirects to "hello" endp

app.MapHealthChecks("/healthcheck");
app.MapRazorPages();  // adds all razors as an endpoint
app.MapGet("/products/{name}", (string name) => $"Products name is {name}")
	.WithName("products");  // gives a name to endpoint by adding metadata to it

app.MapGet("/links", (LinkGenerator generator) =>
{
	string link = generator.GetPathByName("products", new { name = "big-widget" });
	string link2 = generator.GetUriByName("products", new { Name = "super-fancy-widget" }, "https", new HostString("localhost"));
	return $"View the product at {link} and \t {link2}";
});



app.Run();
