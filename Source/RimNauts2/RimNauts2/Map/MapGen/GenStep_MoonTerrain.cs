using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class GenStep_MoonTerrain : GenStep {
        private int mapRadiusSize = 125;

        public override int SeedPart {
            get {
                return 262606459;
            }
        }

        public override void Generate(Map map, GenStepParams parms) {
            if (map.Biome.defName != "RockMoonBiome") return;
            mapRadiusSize = map.Size.x / 2;
            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                TerrainDef terrainDef;
                terrainDef = TerrainFrom(current, fertility[current]);
                terrainGrid.SetTerrain(current, terrainDef);
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            RimWorld.BeachMaker.Cleanup();
            foreach (RimWorld.TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
                current2.Cleanup();
            }
        }

        private TerrainDef TerrainFrom(IntVec3 current, float fertility) {
            TerrainDef terrainDef = DefDatabase<TerrainDef>.GetNamed("OpenSpace", true);
            // if the tile is within radius of the center, get the rock terrain
            if (Math.Sqrt(Math.Pow(current.x - mapRadiusSize, 2) + Math.Pow(current.z - mapRadiusSize, 2)) < mapRadiusSize) {
                ThingDef rock = GenStep_MoonRocks.RockDefAt(fertility);
                terrainDef = rock.building.naturalTerrain;
            }
            return terrainDef;
        }
    }
}
