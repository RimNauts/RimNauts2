using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.AsteroidOre {
    class RocksFromGrid : GenStep {
        public string rock_def_name = "Limestone";
        public string ore_def_name = "MineableSteel";

        public override int SeedPart => 262606459;

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
