using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_RocksFromGrid), "Generate")]
    static class RocksFromGridPatch {
        static bool Prefix(Map map, GenStepParams parms) {
            // check if it's our biome. If not, skip the patch
            if (map.Biome.defName != "RockMoonBiome") {
                return true;
            }

            // if this is our biome, bypass the original for the new version
            (new GenStep_MoonRocks()).Generate(map, parms);
            return false;
        }
    }
}
