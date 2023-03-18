using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.MoonBarren {
    class RocksFromGrid : GenStep {
        public override int SeedPart => 709489798;

        public override void Generate(Map map, GenStepParams parms) {
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
                        ThingDef def = rock_at(fertility[current]);
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
            RimWorld.GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new RimWorld.GenStep_ScatterLumpsMineable {
                maxValue = 3.40282347E+38f
            };
            genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(10f, 10f);
            genStep_ScatterLumpsMineable.Generate(map, parms);
            map.regionAndRoomUpdater.Enabled = true;
        }

        public static ThingDef rock_at(float fertility) {
            ThingDef thingDef = ThingDef.Named("Marble");
            // Changes the ratio of rock types
            if (fertility > 0.4f) thingDef = ThingDef.Named("Slate");
            return thingDef;
        }

        private class RoofThreshold {
            public RoofDef roofDef;
            public float minGridVal;
        }
    }
}
