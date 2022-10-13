using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimNauts2 {
    public class GenStep_MoonTerrain : GenStep {
        private int mapRadiusSize = 125;

        public override int SeedPart {
            get {
                return 262606459;
            }
        }

        /// <summary>
        /// This is a heavily simplified version of the vanilla GenStep_Terrain
        /// Most of the original isn't needed, and there are several changes to the parts that are left
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parms"></param>
        public override void Generate(Map map, GenStepParams parms) {
            //check if it's our biome. If not, skip
            if (map.Biome.defName != "RockMoonBiome") {
                return;
            }

            mapRadiusSize = map.Size.x / 2;

            List<IntVec3> list = new List<IntVec3>();
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            MapGenFloatGrid caves = MapGenerator.Caves;
            TerrainGrid terrainGrid = map.terrainGrid;
            foreach (IntVec3 current in map.AllCells) {
                Building edifice = current.GetEdifice(map);
                TerrainDef terrainDef;
                terrainDef = this.TerrainFrom(current, fertility[current]);

                terrainGrid.SetTerrain(current, terrainDef);
            }
            RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
            BeachMaker.Cleanup();
            foreach (TerrainPatchMaker current2 in map.Biome.terrainPatchMakers) {
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
