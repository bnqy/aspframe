var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(o =>
{
	o.LowercaseUrls = true;   // URLs converted to lower case
	o.LowercaseQueryStrings = false; // query strings are not converted to lowercase
	o.AppendTrailingSlash = true;   // adds slash / at the end of URL
});

var app = builder.Build();

app.MapGet("/oKyEs", () => Results.Ok()).WithName("ok");
app.MapGet("/{name}", (string name) => name).WithName("name");


app.MapGet("/", (LinkGenerator link, HttpContext c) => new[]
{
	link.GetPathByName("ok"), // returns -> /okyes/
	link.GetUriByName(c, "name", new{ Name = "Dua-Lipa", Q = "Test"}) // returns -> /dua-lipa/?Q=Test (https://localhost:7204/dua-lipa/?Q=Test)
});

app.Run();
