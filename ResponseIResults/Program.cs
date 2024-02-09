using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Concurrent;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var fruits = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/fruits", () => fruits);

app.MapGet("/fruit/{id}", (string id) => fruits.TryGetValue(id, out var fruit) 
? TypedResults.Ok(fruit)   // returns 2000 ok res with json
// : Results.NotFound()
: Results.Problem(statusCode: 404));   // if id does not exist returns 404 not found res

app.MapPost("/fruit/{id}", (string id, Fruit fruit) => fruits.TryAdd(id, fruit)
? TypedResults.Created($"/fruit/{id}", fruit)                    // returns 201 created response wirh json
//: Results.BadRequest(new { id = "this id is already exists"})
: Results.ValidationProblem(new Dictionary<string, string[]>()
{
	{"id", new[] {"A fruit with this id already exists"}}
}));  // if id exists returns 400 Bad res

app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
{
	fruits[id] = fruit;
	return Results.NoContent();   // returns 204 response no content
});

app.MapDelete("/fruit/{id}", (string id) =>
{
	fruits.TryRemove(id, out _);
	return Results.NoContent();
});


app.MapGet("/httpresponse", (HttpResponse resp) =>
{
	resp.StatusCode = 418;
	resp.ContentType = MediaTypeNames.Text.Plain;
	return resp.WriteAsync("It is a resp with Httpresponse!");
});


app.Run();

record Fruit(string Name, int stock);