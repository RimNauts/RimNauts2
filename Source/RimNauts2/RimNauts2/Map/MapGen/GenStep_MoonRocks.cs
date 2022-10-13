using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimNauts2 {
    public class GenStep_MoonRocks : GenStep {
        private int mapRadiusSize = 125;

        public override int SeedPart {
            get {
                return 1182952823;
            }
        }

        private class RoofThreshold {
            public RoofDef roofDef;

            public float minGridVal;
        }

        private float maxMineableValue = 3.40282347E+38f;

        private const int MinRoofedCellsPerGroup = 20;

        public static ThingDef RockDefAt(float fertility) {
            ThingDef thingDef = ThingDef.Named("BiomesNEO_HighlandRock");

            // Changes the ratio of rock types
            float threshhold = 0.4f;
            if (fertility > threshhold) {
                thingDef = ThingDef.Named("BiomesNEO_MariaRock");
            }
            return thingDef;
        }

        /// <summary>
        /// This is a customized copy of the vanilla version
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parms"></param>
        public override void Generate(Map map, GenStepParams parms) {
            if (map.Biome.defName != "RockMoonBiome") {
                return;
            }

            mapRadiusSize = map.Size.x / 2;

            map.regionAndRoomUpdater.Enabled = false;
            float num = 0.7f;
            List<GenStep_MoonRocks.RoofThreshold> list = new List<GenStep_MoonRocks.RoofThreshold>();
            list.Add(new GenStep_MoonRocks.RoofThreshold {
                roofDef = RoofDefOf.RoofRockThick,
                minGridVal = num * 1.14f
            });
            list.Add(new GenStep_MoonRocks.RoofThreshold {
                roofDef = RoofDefOf.RoofRockThin,
                minGridVal = num * 1.04f
            });
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            MapGenFloatGrid caves = MapGenerator.Caves;
            MapGenFloatGrid fertility = MapGenerator.Fertility;

            foreach (IntVec3 current in map.AllCells) {
                if (IsInRadius(current)) {
                    float num2 = elevation[current];
                    if (num2 > num) {
                        if (caves[current] <= 0f) {
                            ThingDef def = GenStep_MoonRocks.RockDefAt(fertility[current]);


                            GenSpawn.Spawn(def, current, map, WipeMode.Vanish);
                        }
                        for (int i = 0; i < list.Count; i++) {
                            if (num2 > list[i].minGridVal) {
                                map.roofGrid.SetRoof(current, list[i].roofDef);
                                break;
                            }
                        }
                    }
                }
            }

            BoolGrid visited = new BoolGrid(map);
            List<IntVec3> toRemove = new List<IntVec3>();
            foreach (IntVec3 current2 in map.AllCells) {
                if (!visited[current2]) {
                    if (this.IsNaturalRoofAt(current2, map)) {
                        toRemove.Clear();
                        map.floodFiller.FloodFill(current2, (IntVec3 x) => this.IsNaturalRoofAt(x, map), delegate (IntVec3 x) {
                            visited[x] = true;
                            toRemove.Add(x);
                        }, 2147483647, false, null);
                        if (toRemove.Count < 20) {
                            for (int j = 0; j < toRemove.Count; j++) {
                                map.roofGrid.SetRoof(toRemove[j], null);
                            }
                        }
                    }
                }
            }
            GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
            genStep_ScatterLumpsMineable.maxValue = this.maxMineableValue;
            float num3 = 10f;

            genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
            genStep_ScatterLumpsMineable.Generate(map, parms);
            map.regionAndRoomUpdater.Enabled = true;
        }

        private bool IsNaturalRoofAt(IntVec3 c, Map map) {
            return c.Roofed(map) && c.GetRoof(map).isNatural;
        }

        private bool IsInRadius(IntVec3 current) {
            bool inRadius = false;

            if (Math.Sqrt(Math.Pow(current.x - mapRadiusSize, 2) + Math.Pow(current.z - mapRadiusSize, 2)) < mapRadiusSize) // consider using long form to save on computational time if needed
            {
                inRadius = true;
            }
            return inRadius;
        }

    }
}
