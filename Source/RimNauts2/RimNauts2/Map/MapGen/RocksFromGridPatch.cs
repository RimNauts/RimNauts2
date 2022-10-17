using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_RocksFromGrid), nameof(RimWorld.GenStep_RocksFromGrid.Generate))]
    public static class RocksFromGridPatch {
        [HarmonyPrefix]
        public static bool Prefix(Map map, GenStepParams parms) {
            if (map.Biome != BiomeDefOf.RockMoonBiome) return true;
            // use custom rock generator
            (new GenStep_MoonRocks()).Generate(map, parms);
            return false;
        }
    }
}
