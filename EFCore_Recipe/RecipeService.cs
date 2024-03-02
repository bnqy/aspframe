using Microsoft.EntityFrameworkCore;

namespace EFCore_Recipe;

public class RecipeService
{
	readonly AppDbContext appDbContext;

	public async Task<int> CreateRecipe(CreateRecipeCommand cmd)
	{
		var recipe = new Recipe
		{
			Name = cmd.Name,
			TimeToCook = new TimeSpan(cmd.TimeToCookHrs, cmd.TimeToCookMins, 0),
			Method = cmd.Method,
			IsVegan = cmd.IsVegan,
			IsVegetarian = cmd.IsVegetarian,
			Ingredients = cmd.Ingredients.Select(i =>
			new Ingredient
			{
				Name = i.Name,
				Quantity = i.Quantity,
				Unit = i.Unit,
			})
			.ToList(),
		};

		appDbContext.Add(recipe);
		appDbContext.SaveChangesAsync();

		return recipe.RecipeId;
	}

	public async Task<ICollection<RecipeSummaryViewModel>> GetRecipes()
	{
		return await appDbContext.Recipes
			.Where(x => !x.IsDeleted)
			.Select(y => new RecipeSummaryViewModel
			{
				Id = y.RecipeId,
				Name = y.Name,
				TimeToCook = $"{y.TimeToCook.TotalMinutes}mins"
			})
			.ToListAsync();
	}

	public async Task<RecipeDetailViewModel?> GetRecipeDetail(int id)
	{
		return await appDbContext.Recipes
			.Where(x => x.RecipeId == id)
			.Where(x => !x.IsDeleted)
			.Select(x => new RecipeDetailViewModel
			{
				Id = x.RecipeId,
				Name = x.Name,
				Method = x.Method,
				Ingredients = x.Ingredients
				.Select(item => new RecipeDetailViewModel.Item
				{
					Name = item.Name,
					Quantity = $"{item.Quantity} {item.Unit}"
				})
			})
			.SingleOrDefaultAsync();
	}
}
