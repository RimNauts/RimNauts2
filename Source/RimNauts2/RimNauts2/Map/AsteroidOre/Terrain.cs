using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2.AsteroidOre {
    class Terrain : GenStep {
        public string rock_def_name = "Limestone";
        public string ore_def_name = "MineableSteel";

        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                ThingDef rock = rock_at(fertility[current], rock_def_name);
                if (rock.defName == rock_def_name) {
                    TerrainDef terrainDef = rock.building.naturalTerrain;
                    terrainGrid.SetTerrain(current, terrainDef);
                    GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(Rand.Value > 0.40 ? rock_def_name : ore_def_name, true), current, map);
                } else terrainGrid.SetTerrain(current, DefDatabase<TerrainDef>.GetNamed("RimNauts2_Vacuum"));
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            RimWorld.BeachMaker.Cleanup();
            foreach (RimWorld.TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
                current2.Cleanup();
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
            Find.World.grid.tiles.ElementAt(map.Tile).temperature = -100f;
        }

        public static ThingDef rock_at(float fertility, string rock_def_name) {
            ThingDef thingDef = ThingDef.Named(rock_def_name);
            // Changes the ratio of rock types
            if (fertility > 0.1f) thingDef = ThingDef.Named("Slate");
            return thingDef;
        }
    }
}
