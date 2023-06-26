using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

[assembly: ModInfo("Tools Require Rope")]

namespace ToolsRequireRope;

class Core : ModSystem
{
    const string target = "rope";

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);

        var ingred = api.World.CreateIngredient(target);

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            char availableLetter = recipe.FindAvailableLetter();

            if (!recipe.IsTool() || recipe.Ingredients.Values.Any(ingred => ingred.HasItem(target))) continue;

            switch (recipe.Ingredients.Count)
            {
                case 2:
                    {
                        var a = recipe.IngredientPattern[0];
                        var b = recipe.IngredientPattern[1];
                        recipe.IngredientPattern = $"{a}{b}{availableLetter}";
                        recipe.Width = 1;
                        recipe.Height = 3;
                        recipe.AddIngredientAndResolve(api.World, availableLetter, ingred);
                        break;
                    }
                case 3:
                    {
                        var a = recipe.IngredientPattern[0];
                        var b = recipe.IngredientPattern[1];
                        var c = recipe.IngredientPattern[2];
                        recipe.IngredientPattern = $"{a}{b},{c}{availableLetter}";
                        recipe.Width = 2;
                        recipe.Height = 2;
                        recipe.AddIngredientAndResolve(api.World, availableLetter, ingred);
                        break;
                    }
            }
        }

        api.World.Logger.Event("started 'Tools Require Rope' mod");
    }
}