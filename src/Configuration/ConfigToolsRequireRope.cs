using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace ToolsRequireRope;

public class ConfigToolsRequireRope : IModConfig
{
    public readonly string Comment = "You can make specific tools require different bindings in this file. Examples: \"rope\", \"flaxfibers\", \"flaxtwine\", \"primitivesurvival:cordage\"";
    public readonly string DefaultBinding = "rope";
    public Dictionary<string, string> Bindings { get; set; } = new();

    public ConfigToolsRequireRope(ICoreAPI api, ConfigToolsRequireRope previousConfig = null)
    {
        if (previousConfig != null)
        {
            foreach ((string key, string value) in previousConfig.Bindings)
            {
                if (!Bindings.ContainsKey(key))
                {
                    Bindings.Add(key, value);
                }
            }
        }

        if (api != null)
        {
            FillDefault(api);
        }
    }

    private void FillDefault(ICoreAPI api)
    {
        foreach (GridRecipe key in api.World.GridRecipes.Where(x => x.IsTool()).ToList())
        {
            if (!Bindings.ContainsKey(key.Output.ResolvedItemstack.Collectible.Code.ToString()))
            {
                Bindings.Add(key.Output.ResolvedItemstack.Collectible.Code.ToString(), DefaultBinding);
            }
        }
    }
}