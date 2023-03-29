using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class Assets {
        private static AssetBundle assets;
        public static Shader neo_shader;
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Material> materials = new Dictionary<string, Material>();
        public static Dictionary<string, Color> texture_colors = new Dictionary<string, Color>();
        public static GameObject game_object_world_feature;

        public static void init() {
            get_assets();
            neo_shader = get_asset("neo.shader", ShaderDatabase.WorldOverlayCutout);
            game_object_world_feature = Resources.Load<GameObject>("Prefabs/WorldText");
            foreach (var (type, object_holders) in Defs.Loader.object_holders) {
                foreach (var object_holder in object_holders) {
                    if (object_holder.texture_overlay != null && !textures.ContainsKey(object_holder.texture_overlay)) {
                        textures.Add(object_holder.texture_overlay, get_content<Texture2D>(object_holder.texture_overlay));
                    }
                }
            }
            foreach (var (type, object_holders) in Defs.Loader.object_holders) {
                foreach (var object_holder in object_holders) {
                    if (object_holder.texture_paths.NullOrEmpty()) continue;
                    foreach (var texture_path in object_holder.texture_paths) {
                        if (!materials.ContainsKey(texture_path)) {
                            materials.Add(
                                texture_path,
                                MaterialPool.MatFrom(
                                    texture_path,
                                    neo_shader,
                                    RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
                                )
                            );
                        }
                    }
                }
            }
            foreach (var (type, object_metadata) in Defs.Loader.object_metadata) {
                if (object_metadata.texture_paths.NullOrEmpty()) continue;
                foreach (var texture_path in object_metadata.texture_paths) {
                    if (!materials.ContainsKey(texture_path)) {
                        materials.Add(
                            texture_path,
                            MaterialPool.MatFrom(
                                texture_path,
                                neo_shader,
                                RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
                            )
                        );
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

        public static Color get_average_color_from_texture(string path) {
            if (path == null) return Color.white;
            if (texture_colors.TryGetValue(path, out Color value)) return value;
            texture_colors[path] = average_color_from_texture(get_content<Texture2D>(path));
            return texture_colors[path];
        }

        private static Color average_color_from_texture(Texture2D tex) {
            if (tex == null) return Color.white;
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                tex.width,
                tex.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );
            Graphics.Blit(tex, tmp);
            RenderTexture.active = tmp;
            // Create a new readable Texture2D to copy the pixels to it
            Texture2D myTexture2D = new Texture2D(tex.width, tex.height);
            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            // Reset the active RenderTexture
            //RenderTexture.active = previous;
            // Release the temporary RenderTexture

            Color[] texColors = myTexture2D.GetPixels();
            RenderTexture.ReleaseTemporary(tmp);
            int total = texColors.Length;
            float r = 0;
            float g = 0;
            float b = 0;
            for (int i = 0; i < total; i++) {
                r += texColors[i].r;
                g += texColors[i].g;
                b += texColors[i].b;
            }
            return new Color(r / total, g / total, b / total, 1.0f);
        }
    }
}
