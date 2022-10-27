using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.MoonBarren {
    class Terrain : GenStep {
        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                ThingDef rock = rock_at(fertility[current]);
                TerrainDef terrainDef = rock.building.naturalTerrain;
                terrainGrid.SetTerrain(current, terrainDef);
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            RimWorld.BeachMaker.Cleanup();
            foreach (RimWorld.TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
                current2.Cleanup();
            }
        }
        public static ThingDef rock_at(float fertility) {
            ThingDef thingDef = ThingDef.Named("Marble");
            // Changes the ratio of rock types
            if (fertility > 0.4f) thingDef = ThingDef.Named("Slate");
            return thingDef;
        }
    }
}
