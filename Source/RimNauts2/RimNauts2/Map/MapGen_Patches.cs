using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_Terrain), nameof(RimWorld.GenStep_Terrain.Generate))]
    public static class GenStep_Terrain {
        public static bool Prefix(Map map, GenStepParams parms) {
            switch (map.Biome.defName) {
                case "RimNauts2_MoonBarren_Biome":
                    MoonBarren.GenStep_Terrain(map);
                    return false;
                case "RimNauts2_MoonStripped_Biome":
                    MoonBarren.GenStep_Terrain(map);
                    return false;
                case "RimNauts2_OreSteel_Biome":
                    AsteroidOre.GenStep_Terrain(map, "Limestone", "MineableSteel");
                    return false;
                case "RimNauts2_OreGold_Biome":
                    AsteroidOre.GenStep_Terrain(map, "Limestone", "MineableGold");
                    return false;
                case "RimNauts2_OrePlasteel_Biome":
                    AsteroidOre.GenStep_Terrain(map, "Limestone", "MineablePlasteel");
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
                    MoonBarren.GenStep_RocksFromGrid(map, parms);
                    return false;
                case "RimNauts2_MoonStripped_Biome":
                    MoonBarren.GenStep_RocksFromGrid(map, parms);
                    return false;
                case "RimNauts2_OreSteel_Biome":
                    AsteroidOre.GenStep_RocksFromGrid(map, "Limestone", "MineableSteel");
                    return false;
                case "RimNauts2_OreGold_Biome":
                    AsteroidOre.GenStep_RocksFromGrid(map, "Limestone", "MineableGold");
                    return false;
                case "RimNauts2_OrePlasteel_Biome":
                    AsteroidOre.GenStep_RocksFromGrid(map, "Limestone", "MineablePlasteel");
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
                    MoonBarren.GenStep_ElevationFertility(map);
                    return false;
                case "RimNauts2_MoonStripped_Biome":
                    MoonBarren.GenStep_ElevationFertility(map);
                    return false;
                case "RimNauts2_OreSteel_Biome":
                    AsteroidOre.GenStep_ElevationFertility(map);
                    return false;
                case "RimNauts2_OreGold_Biome":
                    AsteroidOre.GenStep_ElevationFertility(map);
                    return false;
                case "RimNauts2_OrePlasteel_Biome":
                    AsteroidOre.GenStep_ElevationFertility(map);
                    return false;
                default:
                    return true;
            }
        }
    }
}
