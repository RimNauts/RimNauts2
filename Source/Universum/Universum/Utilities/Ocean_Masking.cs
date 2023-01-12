namespace Universum.Utilities {
    /**
     * Masks biome tile as ocean to apply ocean material when drawing planet.
     */
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.BiomeDef), "DrawMaterial", HarmonyLib.MethodType.Getter)]
    public static class BiomeDef_DrawMaterial {
        public static void Prefix(ref RimWorld.BiomeDef __instance) {
            if (Cache.allowed_utility(__instance, "(Universum) Ocean Masking")) __instance = RimWorld.BiomeDefOf.Ocean;
        }
    }
}
