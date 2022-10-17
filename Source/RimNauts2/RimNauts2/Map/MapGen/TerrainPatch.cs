using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_Terrain), nameof(RimWorld.GenStep_Terrain.Generate))]
    public static class TerrainPatch {
        [HarmonyPrefix]
        public static bool Prefix(Map map, GenStepParams parms) {
            if (map.Biome != BiomeDefOf.RockMoonBiome) return true;
            // use custom terrain generator
            new GenStep_MoonTerrain().Generate(map, parms);
            return false;
        }
    }
}
