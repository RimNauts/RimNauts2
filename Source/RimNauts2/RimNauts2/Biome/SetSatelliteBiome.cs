using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGenStep_Terrain), "BiomeFrom")]
    class SetSatelliteBiome {
        public static int i = 0;

        public static void Postfix(RimWorld.Planet.Tile ws, int tileID, ref RimWorld.BiomeDef __result) {
            if (i < Settings.TotalSatelliteObjects && __result == RimWorld.BiomeDefOf.Ocean) {
                List<int> neighbors = new List<int>();
                Find.World.grid.GetTileNeighbors(tileID, neighbors);
                foreach (var neighbour in neighbors) {
                    var neighbour_tile = Find.World.grid.tiles.ElementAtOrDefault(neighbour);
                    if (neighbour_tile != default(RimWorld.Planet.Tile)) {
                        if (neighbour_tile.biome != RimWorld.BiomeDefOf.Ocean) {
                            return;
                        }
                    }
                }
                __result = RimWorld.BiomeDef.Named("RimNauts2_Satellite_Biome");
                i++;
            } else if (i < Settings.TotalSatelliteObjects) {
                List<int> neighbors = new List<int>();
                Find.World.grid.GetTileNeighbors(tileID, neighbors);
                foreach (var neighbour in neighbors) {
                    var neighbour_tile = Find.World.grid.tiles.ElementAtOrDefault(neighbour);
                    if (neighbour_tile != default(RimWorld.Planet.Tile)) {
                        if (neighbour_tile.biome.defName.Contains("RimNauts2")) {
                            __result = RimWorld.BiomeDefOf.Ocean;
                            ws.elevation = -50;
                            ws.hilliness = RimWorld.Planet.Hilliness.Flat;
                            return;
                        }
                    }
                }
            }
        }
    }
}
