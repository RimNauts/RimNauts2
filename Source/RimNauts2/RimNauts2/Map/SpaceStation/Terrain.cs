using Verse;

namespace RimNauts2.SpaceStation {
    class Terrain : GenStep {
        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            foreach (IntVec3 current in map.AllCells) {
                map.terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("RimNauts2_Vacuum"));
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }
    }
}
