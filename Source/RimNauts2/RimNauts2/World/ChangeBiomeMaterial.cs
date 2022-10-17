using HarmonyLib;

namespace RimNauts2.World {
    [HarmonyPatch(typeof(RimWorld.BiomeDef), nameof(RimWorld.BiomeDef.DrawMaterial), MethodType.Getter)]
    public static class ChangeBiomeMaterial {
        public static void Prefix(ref RimWorld.BiomeDef __instance) {
            if (__instance == BiomeDefOf.SatelliteBiome || __instance == BiomeDefOf.RockMoonBiome) {
                __instance = RimWorld.BiomeDefOf.Ocean;
            }
        }
    }
}
