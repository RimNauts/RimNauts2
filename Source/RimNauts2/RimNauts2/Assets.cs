using UnityEngine;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class Assets {
        private static AssetBundle assets;
        public static Shader neo_shader;

        public static void init() {
            get_assets();
            neo_shader = get_asset("neo.shader", ShaderDatabase.WorldOverlayCutout);
        }

        private static void get_assets() {
            string platform_str;
            switch (Application.platform) {
                case RuntimePlatform.OSXPlayer:
                    platform_str = "mac";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    platform_str = "windows";
                    break;
                case RuntimePlatform.LinuxPlayer:
                    platform_str = "linux";
                    break;
                default:
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.assets_loaded",
                        prefix: Style.tab,
                        args: new NamedArgument[] { "no supported" }
                    );
                    return;
            }
            foreach (var assets in RimNauts2_ModContent.instance.Content.assetBundles.loadedAssetBundles) {
                if (assets.name.Contains(platform_str)) {
                    Assets.assets = assets;
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.assets_loaded",
                        prefix: Style.tab,
                        args: new NamedArgument[] { platform_str }
                    );
                    return;
                }
            }
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.assets_loaded",
                prefix: Style.tab,
                args: new NamedArgument[] { "no supported" }
            );
        }

        public static T get_asset<T>(string name, T fallback) where T : Object {
            if (assets == null) return fallback;
            return assets.LoadAsset<T>(name);
        }
    }
}
