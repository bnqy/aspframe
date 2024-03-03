// 12

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EFCore_Recipe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title="Recipe App", Version="v1"}));

builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

// connstr taken from config, from ConnectionStrings section
var connectionStringSqlite = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionStringSqlServer = builder.Configuration.GetConnectionString("SqlServer");


// adds context class to DI
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionStringSqlite));
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionStringSqlServer!));

var app = builder.Build();

app.UseSwagger(); app.UseSwaggerUI();


RouteGroupBuilder routes = app.MapGroup("")
	.WithTags("Recipes")
	.WithOpenApi();

routes.MapGet("/", async (RecipeService service) =>
{
	return await service.GetRecipes();
});

routes.MapPost("/", async (CreateRecipeCommand cmd, RecipeService service) =>
{
	var id = await service.CreateRecipe(cmd);
	return Results.CreatedAtRoute("view-recipe", new { id });
});

app.Run();

public class AppDbContext
	: DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)  // opt obj contains infos like connection strings
		: base(options)
	{
	}

	public DbSet<Recipe> Recipes { get; set; }   // will be used to query db IQueryable
}

public class Recipe
{
	public int RecipeId { get; set; }  // interprets as a PK bc Id
	public required string Name { get; set; }  // to prevent warnings about uninitialized non-nullable values.
	public TimeSpan TimeToCook { get; set; }
	public bool IsDeleted { get; set; }
	public required string Method { get; set; }
	public bool IsVegetarian { get; set; }
	public bool IsVegan { get; set; }
	public required ICollection<Ingredient> Ingredients { get; set; }  // many-one
}

public class Ingredient
{
	public int IngredientId { get; set; }  // as a PK
	public int RecipeId { get; set; }  // as a FK
	public required string Name { get; set; }
	public decimal Quantity { get; set; }
	public int JustMigrate { get; set; }
	public required string Unit { get; set; }
}


public class EditRecipeBase
{
	[Required, StringLength(100)]
	public required string Name { get; set; }
	[Range(0, 23), DisplayName("Time to cook (hrs)")]
	public int TimeToCookHrs { get; set; }
	[Range(0, 59), DisplayName("Time to cook (mins)")]
	public int TimeToCookMins { get; set; }
	[Required]
	public required string Method { get; set; }
	[DisplayName("Vegetarian?")]
	public bool IsVegetarian { get; set; }
	[DisplayName("Vegan?")]
	public bool IsVegan { get; set; }
}

public class CreateRecipeCommand : EditRecipeBase
{
	public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

	public Recipe ToRecipe()
	{
		return new Recipe
		{
			Name = Name,
			TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
			Method = Method,
			IsVegetarian = IsVegetarian,
			IsVegan = IsVegan,
			Ingredients = Ingredients.Select(x => x.ToIngredient()).ToList()
		};
	}
}
public class CreateIngredientCommand
{
	[Required, StringLength(100)]
	public required string Name { get; set; }
	[Range(0, int.MaxValue)]
	public decimal Quantity { get; set; }
	[Required, StringLength(20)]
	public required string Unit { get; set; }

	public Ingredient ToIngredient()
	{
		return new Ingredient
		{
			Name = Name,
			Quantity = Quantity,
			Unit = Unit,
		};
	}
}

public class RecipeDetailViewModel
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string Method { get; set; }

	public required IEnumerable<Item> Ingredients { get; set; }

	public class Item
	{
		public required string Name { get; set; }
		public required string Quantity { get; set; }
	}
}

public class RecipeSummaryViewModel
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string TimeToCook { get; set; }
	public int NumberOfIngredients { get; set; }

	public static RecipeSummaryViewModel FromRecipe(Recipe recipe)
	{
		return new RecipeSummaryViewModel
		{
			Id = recipe.RecipeId,
			Name = recipe.Name,
			TimeToCook = $"{recipe.TimeToCook.Hours}hrs {recipe.TimeToCook.Minutes}mins",
		};
	}
}

public class UpdateRecipeCommand : EditRecipeBase
{
	public int Id { get; set; }

	public void UpdateRecipe(Recipe recipe)
	{
		recipe.Name = Name;
		recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
		recipe.Method = Method;
		recipe.IsVegetarian = IsVegetarian;
		recipe.IsVegan = IsVegan;
	}
}
