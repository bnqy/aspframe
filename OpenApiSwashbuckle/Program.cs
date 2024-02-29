using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();   // endpoint discovery feature
builder.Services.AddSwaggerGen(x =>
x.SwaggerDoc("v1", new OpenApiInfo  // change openapi doc
{
	Title = "Test test",
	Description = "Test description",
	Version = "1.0",
}));  // add Swashbuckle to create OpenApi document

var app = builder.Build();

var _fruits = new ConcurrentDictionary<string, Fruit>();

app.UseSwagger();   // adds middleware to expose openapi document
app.UseSwaggerUI();  // ui

app.MapGet("/", () => "Hello World!");

app.MapGet("/fruit/{id}", (string id) =>
_fruits.TryGetValue(id, out var fruit) 
? TypedResults.Ok(fruit)
: Results.Problem(statusCode: 404));

app.MapPost("/fruit/{id}", (string id, Fruit fruit) => 
_fruits.TryAdd(id, fruit)
? TypedResults.Created($"fruit/{id}", fruit)
: Results.ValidationProblem(new Dictionary<string, string[]>
{
	{ "id", new[] { "A fruit with this id already exists" } }
})
);

app.Run();


record Fruit(string Name, int Stock);