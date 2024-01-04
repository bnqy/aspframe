using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Concurrent;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/fruit", () => _fruit);

app.MapGet("/fruit/{id}", (string id) =>
_fruit.TryGetValue(id, out var fruit) 
? TypedResults.Ok(fruit)
: Results.NotFound());

//var getFruit = (string id) => Fruit.All[id];
//app.MapGet("/fruit/{id}", getFruit);

app.MapPost("/fruit/{id}", (string id, Fruit fruit) => 
_fruit.TryAdd(id, fruit)
? TypedResults.Created($"/fruit/{id}", fruit)
: Results.BadRequest(new { id = "A fruit with this id already exists" }));
//app.MapPost("/fruit/{id}", Handlers.AddFruit);


app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
{
	_fruit[id] = fruit;
	return Results.NoContent();
});
//Handlers handlers = new();
//app.MapPut("/fruit/{id}", handlers.ReplaceFruit);


app.MapDelete("/fruit/{id}", (string id) =>
{
	_fruit.TryRemove(id, out _);
	return Results.NoContent();
});
//app.MapDelete("/fruit/{id}", DeleteFruit);

app.MapGet("/teapot", (HttpResponse hp) =>
{
	hp.StatusCode = 418;
	hp.ContentType = MediaTypeNames.Text.Plain;
	return hp.WriteAsync("I am a teapot!");
});

app.Run();

/*void DeleteFruit(string id)
{
	Fruit.All.Remove(id);
}
*/

record Fruit(string Name, int Stock)
{
	public static readonly Dictionary<string, Fruit> All = new();
};

/*class Handlers
{
	public void ReplaceFruit(string id, Fruit fruit)
	{
		Fruit.All[id] = fruit;
	}

	public static void AddFruit(string id, Fruit fruit)
	{
		Fruit.All.Add(id, fruit);
	}
}*/
