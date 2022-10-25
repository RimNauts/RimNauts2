using Verse;

namespace RimNauts2 {
    public class MoonWater {
        public static void GenStep_Terrain(Map map) {
            foreach (IntVec3 current in map.AllCells) {
                map.terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("WaterShallow"));
            }
        }
    }
}
