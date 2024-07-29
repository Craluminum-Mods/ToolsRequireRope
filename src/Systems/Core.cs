using System.Linq;
using Vintagestory.API.Common;

[assembly: ModInfo(name: "Tools Require Rope", modID: "toolsrequirerope", Side = "Server")]

namespace ToolsRequireRope;

public class Core : ModSystem
{
    public static ConfigToolsRequireRope ConfigToolsRequireRope { get; private set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override double ExecuteOrder() => 1.01;

    public override void AssetsLoaded(ICoreAPI api)
    {
        ConfigToolsRequireRope = ModConfig.ReadConfig<ConfigToolsRequireRope>(api, "ToolsRequireRope.json");
        if (ConfigToolsRequireRope == null) return;

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            AssetLocation target = AssetLocation.Create(ConfigToolsRequireRope.DefaultBinding);

            if (ConfigToolsRequireRope.Bindings.TryGetValue(recipe.Output.ResolvedItemstack.Collectible.Code.ToString(), out string binding))
            {
                if (binding == null) continue;
                target = AssetLocation.Create(binding);
            }

            if (recipe.IngredientPattern.Replace("_", "").Length >= 4 || recipe.Ingredients.Values.Any(ingred => ingred.Code == target))
            {
                continue;
            }

            CraftingRecipeIngredient ingred = api.World.CreateIngredient(target);
            char availableLetter = recipe.FindAvailableLetter();

            switch (recipe.Ingredients.Count)
            {
                case 2:
                    {
                        char a = recipe.IngredientPattern[0];
                        char b = recipe.IngredientPattern[1];
                        recipe.IngredientPattern = $"{a}{b}{availableLetter}";
                        recipe.Width = 1;
                        recipe.Height = 3;
                        recipe.Ingredients.Add(availableLetter.ToString(), ingred);
                        recipe.ResolveIngredients(api.World);
                        break;
                    }
                case 3:
                    {
                        char a = recipe.IngredientPattern[0];
                        char b = recipe.IngredientPattern[1];
                        char c = recipe.IngredientPattern[2];
                        recipe.IngredientPattern = $"{a}{b},{c}{availableLetter}";
                        recipe.Width = 2;
                        recipe.Height = 2;
                        recipe.Ingredients.Add(availableLetter.ToString(), ingred);
                        recipe.ResolveIngredients(api.World);
                        break;
                    }
            }
        }

        api.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}