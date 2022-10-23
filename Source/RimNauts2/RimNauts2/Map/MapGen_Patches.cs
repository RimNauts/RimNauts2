using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_Terrain), nameof(RimWorld.GenStep_Terrain.Generate))]
    public static class GenStep_Terrain {
        public static bool Prefix(Map map, GenStepParams parms) {
            switch (map.Biome.defName) {
                case "RimNauts2_MoonBarren_Biome":
                    BarrenMoon.GenStep_Terrain(map);
                    return false;
                default:
                    return true;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.GenStep_RocksFromGrid), nameof(RimWorld.GenStep_RocksFromGrid.Generate))]
    public static class GenStep_RocksFromGrid {
        public static bool Prefix(Map map, GenStepParams parms) {
            switch (map.Biome.defName) {
                case "RimNauts2_MoonBarren_Biome":
                    BarrenMoon.GenStep_RocksFromGrid(map, parms);
                    return false;
                default:
                    return true;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.GenStep_ElevationFertility), nameof(RimWorld.GenStep_ElevationFertility.Generate))]
    public static class GenStep_ElevationFertility {
        public static bool Prefix(Map map, GenStepParams parms) {
            switch (map.Biome.defName) {
                case "RimNauts2_MoonBarren_Biome":
                    BarrenMoon.GenStep_ElevationFertility(map);
                    return false;
                default:
                    return true;
            }
        }
    }
}
