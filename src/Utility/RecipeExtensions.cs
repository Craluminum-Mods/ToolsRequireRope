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

    public static CraftingRecipeIngredient CreateIngredient(this IWorldAccessor world, AssetLocation path)
    {
        ItemStack stack = new ItemStack(world.GetItem(path));
        stack.ResolveBlockOrItem(world);

        return new()
        {
            Type = stack.Class,
            ResolvedItemstack = stack,
            Code = stack.Collectible.Code,
        };
    }

    public static bool IsTool(this GridRecipe recipe)
    {
        return recipe.Output.ResolvedItemstack.Collectible.Tool != null;
    }
}