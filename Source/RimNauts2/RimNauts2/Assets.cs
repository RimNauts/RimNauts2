using UnityEngine;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class Assets {
        public static Shader neo_shader;

        static Assets() {
            AssetBundle assets = RimNauts2_ModContent.instance.Content.assetBundles.loadedAssetBundles[0];
            neo_shader = assets.LoadAsset<Shader>("neo.shader");
        }
    }
}
