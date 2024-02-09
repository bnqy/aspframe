using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var fruits = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/fruits", () => fruits);

app.Run();

record Fruit(string Name, int stock);