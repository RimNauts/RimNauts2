using Verse;

namespace RimNauts2 {
    class Vacuum : GenStep {
        public override int SeedPart => 45768545;

        public override void Generate(Map map, GenStepParams parms) {
            foreach (IntVec3 current in map.AllCells) {
                map.terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("RimNauts2_Vacuum"));
                MapGenerator.Elevation[current] = 0.0f;
                MapGenerator.Fertility[current] = 0.0f;
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }
    }
}
