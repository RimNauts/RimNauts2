using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class Assets {
        private static AssetBundle assets;
        public static Shader neo_shader;
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void init() {
            get_assets();
            neo_shader = get_asset("neo.shader", ShaderDatabase.WorldOverlayCutout);
            foreach (var (type, object_holders) in Defs.Loader.object_holders) {
                foreach (var object_holder in object_holders) {
                    if (object_holder.texture_overlay != null && !textures.ContainsKey(object_holder.texture_overlay)) {
                        textures.Add(object_holder.texture_overlay, get_content<Texture2D>(object_holder.texture_overlay));
                    }
                }
            }
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

        public static T get_asset<T>(string name, T fallback = null) where T : Object {
            if (assets == null) return fallback;
            return assets.LoadAsset<T>(name);
        }

        public static T get_content<T>(string path, T fallback = null) where T : Object {
            if (assets == null) return fallback;
            return ContentFinder<T>.Get(path);
        }

        public static Texture2D get_texture(string path) {
            if (path == null || !textures.ContainsKey(path)) return null;
            return textures[path];
        }
    }
}
