using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class GenStep_MoonTerrain : GenStep {
        public override int SeedPart {
            get {
                return 262606459;
            }
        }

        public override void Generate(Map map, GenStepParams parms) {
            if (map.Biome.defName != "RimNauts2_MoonBarren_Biome") return;
            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                TerrainDef terrainDef;
                terrainDef = TerrainFrom(current, map.Center, map.Size, fertility[current]);
                terrainGrid.SetTerrain(current, terrainDef);
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            RimWorld.BeachMaker.Cleanup();
            foreach (RimWorld.TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
                current2.Cleanup();
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }

        private TerrainDef TerrainFrom(IntVec3 current, IntVec3 center, IntVec3 size, float fertility) {
            TerrainDef terrainDef = DefDatabase<TerrainDef>.GetNamed("RimNauts2_Vacuum", true);
            // if the tile is within radius of the center, get the rock terrain
            int radius = (size.x / 2) - 9;
            if (!(((current.x - (center.x - 1)) * (current.x - center.x)) + ((current.z - (center.z - 1)) * (current.z - center.z)) >= (radius * radius))) {
                ThingDef rock = GenStep_MoonRocks.RockDefAt(fertility);
                terrainDef = rock.building.naturalTerrain;
            }
            return terrainDef;
        }
    }
}
