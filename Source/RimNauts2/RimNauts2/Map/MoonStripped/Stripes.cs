using System;
using Verse;

namespace RimNauts2.MoonStripped {
    class Stripes : GenStep {
        public override int SeedPart => 986567587;

        public override void Generate(Map map, GenStepParams parms) {
            for (int ind = 1; ind <= Rand.RangeInclusive(4, 10); ind++) {
                int dz = (int) ((Rand.Value - 0.5) * map.Size.z);
                for (int dx = 1; dx <= map.Size.x - 1; dx++) {
                    int set = (int) (-22f * Math.Sin((3.14f / map.Size.x) * dx - 0) + map.Center.z + dz * 0.75);
                    wide_brush(new IntVec3(dx, 0, set), "Slate", map, Rand.RangeInclusive(1, 4));
                }
            }
        }

        public void wide_brush(IntVec3 pos, string defName, Map map, int halfWidth) {
            for (int dz = -halfWidth; dz <= halfWidth; dz++) {
                if (new IntVec3(pos.x, 0, pos.z + dz).InBounds(map)) {
                    map.terrainGrid.SetTerrain(new IntVec3(pos.x, 0, pos.z + dz), DefDatabase<ThingDef>.GetNamed(defName).building.naturalTerrain);
                }
            }
        }
    }
}
