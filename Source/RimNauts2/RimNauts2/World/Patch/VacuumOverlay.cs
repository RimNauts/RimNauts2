using HarmonyLib;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(Universum.Utilities.MapDrawer_DrawMapMesh), "get_world_map_render")]
    class MapDrawer_DrawMapMesh_get_world_map_render {
        public static void Prefix() {
            for (int i = 0; i < RenderingManager.total_objects; i++) {
                if (RenderingManager.cached_features[i] != null) {
                    RenderingManager.cached_features[i].block = true;
                    RenderingManager.cached_features[i].set_active(false);
                }
            }
        }

        public static void Postfix() {
            for (int i = 0; i < RenderingManager.total_objects; i++) {
                if (RenderingManager.cached_features[i] != null) {
                    RenderingManager.cached_features[i].block = false;
                }
            }
        }
    }
}
