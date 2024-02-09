using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Concurrent;
using System.Net.Mime;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler();
}
app.UseStatusCodePages();

var fruits = new ConcurrentDictionary<string, Fruit>();


app.MapGet("/fruits", () => fruits);

/*app.MapGet("/fruit/{id}", (string id) =>
{
	if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
	{
		return Results.ValidationProblem(new Dictionary<string, string[]>
		{
			{"id", new[] {"Invalid format. Id must start with 'f'"}}
		});
	}

	return fruits.TryGetValue(id, out var fruit)
	? TypedResults.Ok(fruit)   // returns 2000 ok res with json
							   // : Results.NotFound()	   
	: Results.Problem(statusCode: 404);   // if id does not exist returns 404 not found res
});*/

app.MapGet("/fruit/{id}", (string id) =>
fruits.TryGetValue(id, out var fruit)
? TypedResults.Ok(fruit)   // returns 2000 ok res with json
						   // : Results.NotFound()	   
: Results.Problem(statusCode: 404)) // if id does not exist returns 404 not found res
	.AddEndpointFilter(ValidHepler.ValidateId) // add filter
	.AddEndpointFilter(async (context, next) =>
	{
		app.Logger.LogInformation("Executing filter...");
		object? result = await next(context);
		app.Logger.LogInformation($"Handler result: {result}");
		return result;
	});



app.MapPost("/fruit/{id}", (string id, Fruit fruit) => fruits.TryAdd(id, fruit)
? TypedResults.Created($"/fruit/{id}", fruit)                    // returns 201 created response wirh json
																 //: Results.BadRequest(new { id = "this id is already exists"})
: Results.ValidationProblem(new Dictionary<string, string[]>()
{
	{"id", new[] {"A fruit with this id already exists"}}   // if id exists returns 400 Bad res
})) 
	.AddEndpointFilterFactory(ValidHepler.ValidateIdFactory);


app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
{
	fruits[id] = fruit;
	return Results.NoContent();   // returns 204 response no content
});


app.MapDelete("/fruit/{id}", (string id) =>
{
	fruits.TryRemove(id, out _);
	return Results.NoContent();
})
	.AddEndpointFilter<IdValidFilter>();




app.MapGet("/httpresponse", (HttpResponse resp) =>
{
	resp.StatusCode = 418;
	resp.ContentType = MediaTypeNames.Text.Plain;
	return resp.WriteAsync("It is a resp with Httpresponse!");
});




app.MapGet("/", void () => throw new Exception());

app.MapGet("/sc", () => Results.NotFound());


app.Run();








record Fruit(string Name, int stock);

class ValidHepler
{
	public static async ValueTask<object?> ValidateId(
		EndpointFilterInvocationContext context,   // exposes endpoint arguments and HttpContext
		EndpointFilterDelegate next)  // next filter or endpoint
	{
		var id = context.GetArgument<string>(0);  //gets first argument in the req

		if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
		{
			return Results.ValidationProblem(new Dictionary<string, string[]>
			{
				{"id", new[] {"Invalid format. Id must start with 'f'"}}
			});
		}

		return await next(context);   // calls next filters
	}


	public static EndpointFilterDelegate ValidateIdFactory(
		EndpointFilterFactoryContext context,  // provide details about endpoint handler
		EndpointFilterDelegate next)
	{
		ParameterInfo[] parameters = context.MethodInfo.GetParameters();

		int? idPos = null;

		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].Name == "id" && parameters[i].ParameterType == typeof(string))
			{
				idPos = i;
				break;
			}

			if (!idPos.HasValue)
			{
				return next;
			}
		}

		return async (invocationContext) =>
		{
			var id = invocationContext.GetArgument<string>(idPos.Value);

			if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
			{
				return Results.ValidationProblem(new Dictionary<string, string[]>()
				{
					{ "id", new[] { "Id must start with 'f'" } }
				});
			}

			return await next(invocationContext);
		};
	}

}

class IdValidFilter : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var id = context.GetArgument<string>(0);

		if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
		{
			return Results.ValidationProblem(new Dictionary<string, string[]>()
			{
				{"id", new[] {"Invalid format. Id must start with 'f'"}}
			});
		}

		return await next(context);
	}
}