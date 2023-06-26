using Vintagestory.API.Common;

namespace ToolsRequireRope;

public static class RecipeExtensions
{
    public static char FindAvailableLetter(this GridRecipe recipe)
    {
        char availableLetter = 'A';
        for (; availableLetter <= 'Z'; availableLetter++)
        {
            if (!recipe.IngredientPattern.Contains(availableLetter.ToString())) break;
        }

        return availableLetter;
    }

    public static void AddIngredientAndResolve(this GridRecipe recipe, IWorldAccessor world, char availableLetter, CraftingRecipeIngredient ingred)
    {
        recipe.Ingredients.Add(availableLetter.ToString(), ingred);
        recipe.ResolveIngredients(world);
    }

    public static CraftingRecipeIngredient CreateIngredient(this IWorldAccessor world, string path)
    {
        var stack = new ItemStack(world.GetItem(new AssetLocation(path)));
        stack.ResolveBlockOrItem(world);

        return new()
        {
            Type = stack.Class,
            ResolvedItemstack = stack,
            Code = stack.Collectible.Code,
        };
    }

    public static bool IsTool(this GridRecipe recipe) => recipe.Output.ResolvedItemstack.Collectible.Tool != null;
    public static bool HasItem(this CraftingRecipeIngredient ingredient, string target) => ingredient?.Code == new AssetLocation(target);
}