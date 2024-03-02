// 12

using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public class AppDbContext
	: DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)  // opt obj contains infos like connection strings
		: base(options)
	{
	}

	public DbSet<Recipe> Recipes { get; set; }   // will be used to query db
}

public class Recipe
{
	public int RecipeId { get; set; }  // interprets as a PK bc Id
	public required string Name { get; set; }  // to prevent warnings about uninitialized non-nullable values.
	public TimeSpan TimeToCook { get; set; }
	public bool IsDeleted { get; set; }
	public required string Method { get; set; }
	public required ICollection<Ingredient> Ingredients { get; set; }  // many-one
}

public class Ingredient
{
	public int IngredientId { get; set; }  // as a PK
	public int RecipeId { get; set; }  // as a FK
	public required string Name { get; set; }
	public decimal Quantity { get; set; }
	public required string Unit { get; set; }
}