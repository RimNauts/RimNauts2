using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class GenStep_MoonRocks : GenStep {
        private readonly float maxMineableValue = 3.40282347E+38f;

        public override int SeedPart {
            get {
                return 1182952823;
            }
        }

        private class RoofThreshold {
            public RoofDef roofDef;
            public float minGridVal;
        }

        public static ThingDef RockDefAt(float fertility) {
            ThingDef thingDef = ThingDef.Named("Marble");
            // Changes the ratio of rock types
            if (fertility > 0.4f) thingDef = ThingDef.Named("Slate");
            return thingDef;
        }

        public override void Generate(Map map, GenStepParams parms) {
            if (map.Biome.defName != "RimNauts2_Moon_Biome") return;
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
                if (IsInRadius(current, map.Center, map.Size)) {
                    float num2 = elevation[current];
                    if (num2 > num) {
                        if (caves[current] <= 0f) {
                            ThingDef def = RockDefAt(fertility[current]);
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
                    if (IsNaturalRoofAt(current2, map)) {
                        toRemove.Clear();
                        map.floodFiller.FloodFill(current2, (IntVec3 x) => IsNaturalRoofAt(x, map), delegate (IntVec3 x) {
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
            RimWorld.GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new RimWorld.GenStep_ScatterLumpsMineable {
                maxValue = maxMineableValue
            };
            float num3 = 10f;
            genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
            genStep_ScatterLumpsMineable.Generate(map, parms);
            map.regionAndRoomUpdater.Enabled = true;
        }

        private bool IsNaturalRoofAt(IntVec3 c, Map map) {
            return c.Roofed(map) && c.GetRoof(map).isNatural;
        }

        private bool IsInRadius(IntVec3 current, IntVec3 center, IntVec3 size) {
            bool inRadius = true;
            int radius = (size.x / 2) - 9;
            if (((current.x - (center.x - 1)) * (current.x - center.x)) + ((current.z - (center.z - 1)) * (current.z - center.z)) >= (radius * radius)) inRadius = false;
            return inRadius;
        }
    }
}
