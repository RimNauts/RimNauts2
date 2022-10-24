using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimNauts2 {
    class AsteroidOre {
        public static void GenStep_ElevationFertility(Map map) {
            // Map generation is based mostly on these two grids. We're making custom grids.
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            // the size of terrain features. Smaller numbers = bigger terrain features. Vanilla = 0.021
            float mountainSize = Rand.Range(0.025f, 0.035f);
            // Overall shape. Smaller numbers = smoother. Vanilla = 2.0
            float mountainSmoothness = Rand.Range(0.0f, 1.0f);
            // the overal shape of the mountains and map features
            ModuleBase elevationGrid = new Perlin(mountainSize, mountainSmoothness, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
            // Make the mountains bigger
            double elevationScaling = 1.0f;
            elevationGrid = new Multiply(elevationGrid, new Const(elevationScaling));
            // By setting fertility = elevation, we ensure that the shape of the terrain will follow the shape of the mountains
            foreach (IntVec3 tile in map.AllCells) {
                fertility[tile] = elevationGrid.GetValue(tile);
            }
            // This changes the relative amount of light and dark mountains
            float offset = 0.6f;
            // Skews the grid towards the center to create a "sea"
            IntVec3 center = map.Center;
            int size = map.Size.x / 2;
            float seaAmount = 2.0f;
            foreach (IntVec3 tile in map.AllCells) {
                float distance = (float) Math.Sqrt(Math.Pow(tile.x - center.x, 2) + Math.Pow(tile.z - center.z, 2));
                //float difference = seaAmount * distance / size - 0.5f;
                float difference = Math.Min(1, seaAmount * distance / size - 0.5f);
                fertility[tile] += difference;
                // use Abs so that there's mountains on both ends of the grid.
                elevation[tile] = Mathf.Abs(fertility[tile] - offset);
            }
        }

        public static void GenStep_Terrain(Map map, string rock_def_name, string ore_def_name) {
            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                ThingDef rock = rock_at(fertility[current], rock_def_name);
                if (rock.defName == rock_def_name) {
                    TerrainDef terrainDef = rock.building.naturalTerrain;
                    terrainGrid.SetTerrain(current, terrainDef);
                    GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(Rand.Value > 0.40 ? rock_def_name : ore_def_name, true), current, map);
                } else terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("RimNauts2_Vacuum"));
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            RimWorld.BeachMaker.Cleanup();
            foreach (RimWorld.TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
                current2.Cleanup();
            }
        }

        public static void GenStep_RocksFromGrid(Map map, string rock_def_name, string ore_def_name) {
            map.regionAndRoomUpdater.Enabled = false;
            float num = 0.7f;
            List<RoofThreshold> list = new List<RoofThreshold> {
                new RoofThreshold {
                    roofDef = RimWorld.RoofDefOf.RoofRockThick,
                    minGridVal = num * 1.14f
                },
                new RoofThreshold {
                    roofDef = RimWorld.RoofDefOf.RoofRockThin,
                    minGridVal = num * 1.04f
                }
            };
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            MapGenFloatGrid caves = MapGenerator.Caves;
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            foreach (IntVec3 current in map.AllCells) {
                float num2 = elevation[current];
                if (num2 > num) {
                    if (caves[current] <= 0f) {
                        ThingDef def = rock_at(fertility[current], rock_def_name);
                        if (def == ThingDef.Named(rock_def_name)) {
                            GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(Rand.Value > 0.60 ? rock_def_name : ore_def_name, true), current, map);
                        } else continue;
                    }
                    for (int i = 0; i < list.Count; i++) {
                        if (num2 > list[i].minGridVal) {
                            map.roofGrid.SetRoof(current, list[i].roofDef);
                            break;
                        }
                    }
                }
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }

        public static ThingDef rock_at(float fertility, string rock_def_name) {
            ThingDef thingDef = ThingDef.Named(rock_def_name);
            // Changes the ratio of rock types
            if (fertility > 0.1f) thingDef = ThingDef.Named("Slate");
            return thingDef;
        }

        private class RoofThreshold {
            public RoofDef roofDef;
            public float minGridVal;
        }
    }
}
