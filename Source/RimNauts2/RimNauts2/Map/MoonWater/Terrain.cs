using System;
using Verse;

namespace RimNauts2.MoonWater {
    class Terrain : GenStep {
        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            foreach (IntVec3 current in map.AllCells) {
                map.terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("WaterShallow"));
            }
        }
    }
}
