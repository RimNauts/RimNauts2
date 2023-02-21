using UnityEngine;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class Assets {
        public static Shader neo_shader;

        static Assets() {
            foreach (var assets in RimNauts2_ModContent.instance.Content.assetBundles.loadedAssetBundles) {
                neo_shader = assets.LoadAsset<Shader>("neo.shader");
                if (neo_shader.name != "Custom/neo") break;
            }
            if (neo_shader.name != "Custom/neo") neo_shader = ShaderDatabase.WorldOverlayCutout;
        }
    }
}
