var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();




app.MapGet("/fruits", () => Fruit.All);  // A lambda expression

var getFruitId = (string id) => Fruit.All[id];
app.MapGet("/fruit/{id}", getFruitId);            // A Func<T, TResult> variable

app.MapPost("/fruit/{id}", Handlers.AddFruit);    // A static method

Handlers handler = new();
app.MapPut("/fruit/{id}", handler.ReplaceFruit);  // A method on an instance variable

app.MapDelete("/fruit/{id}", DeleteFruit);    // A local function

app.Run();


void DeleteFruit(string id)
{
	Fruit.All.Remove(id);
}


record Fruit(string Name, int Stock)
{
	public static readonly Dictionary<string, Fruit> All = new();
};

class Handlers
{
	public static void AddFruit(string id, Fruit fruit)  // converts response to json
	{
		Fruit.All.Add(id, fruit);
	}

	public void ReplaceFruit(string id, Fruit fruit)
	{
		Fruit.All[id] = fruit;
	}
}
