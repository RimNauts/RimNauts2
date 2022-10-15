using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_Terrain), "Generate")]
    public static class TerrainPatch {
        public static bool Prefix(Map map, GenStepParams parms) {
            // check if it's our biome. If not, skip the patch
            if (map.Biome.defName != "RockMoonBiome") {
                return true;
            }

            new GenStep_MoonTerrain().Generate(map, parms);
            return false;
        }
    }
}
