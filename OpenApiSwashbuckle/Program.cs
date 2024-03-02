using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();   // endpoint discovery feature
builder.Services.AddSwaggerGen(x =>
x.SwaggerDoc("v1", new OpenApiInfo  // changes openapi doc
{
	Title = "Test test",
	Description = "Test description",
	Version = "1.0",
}));  // add Swashbuckle to create OpenApi document

var app = builder.Build();

var _fruits = new ConcurrentDictionary<string, Fruit>();

app.UseSwagger();   // adds middleware to expose openapi document  /swagger/v1/swagger.json
app.UseSwaggerUI();  // ui  /swagger

app.MapGet("/", () => "Hello World!");

app.MapGet("/fruit/{id}", (string id) =>
_fruits.TryGetValue(id, out var fruit) 
? TypedResults.Ok(fruit)
: Results.Problem(statusCode: 404))
	.WithName("GetFruit")
	.WithSummary("Gets a fruit")   // adds summary
	.WithDescription("gets fruit by id, if"+  // adds Desc
	"fruit does not exists returns 404")
	.WithTags("fruit")   // groups endpoint in UI. each endpoint can have multi tags
	.Produces<Fruit>()   // endpoint returns Fruit obj, and when not provided it assumed 200
	.ProducesProblem(404)  // when id is not fount it returns 404
	.WithOpenApi(  // it must be added to work summary and desc
	o =>
	{
		o.Parameters[0].Description = "Gets a id of fruit";  // describes parrameter
       //o.Summary = "Gets id";  // can be used instead WithSummary()
		return o;
	}
	);  
 
app.MapPost("/fruit/{id}", (string id, Fruit fruit) => 
_fruits.TryAdd(id, fruit)
? TypedResults.Created($"fruit/{id}", fruit)
: Results.ValidationProblem(new Dictionary<string, string[]>
{
	{ "id", new[] { "A fruit with this id already exists" } }
}))
	.WithName("PostFruit")
	.WithTags("fruit")
	.WithDescription("Adds fruit by id and returns 201(created), if exists returns 400")
	.Produces<Fruit>(201)   // returns 201 instead 200
	.ProducesValidationProblem() // returns 400 with valid error
	.WithOpenApi(o =>
	{
		o.Parameters[0].Description = "Id of a fruit";
		return o;
	});  

app.Run();


record Fruit(string Name, int Stock);