using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public static class Moon {
        public static void generate_moon(Satellite satellite) {
            // store satellite values for later
            int buffer_tile_id = satellite.Tile;
            string buffer_def_name = satellite.def_name;
            Vector3 buffer_max_orbits = satellite.max_orbits;
            Vector3 buffer_shift_orbits = satellite.shift_orbits;
            Vector3 buffer_spread = satellite.spread;
            float buffer_period = satellite.period;
            int buffer_time_offset = satellite.time_offset;
            float buffer_speed = satellite.speed;
            // destroy satellite as an asteroid to keep the tile open
            satellite.type = Satellite_Type.Asteroid;
            satellite.Destroy();
            // generate new satellite with the values saved from before (this process is done to get the new texture)
            satellite = Generate_Satellites.copy_satellite(
                buffer_tile_id,
                buffer_def_name + "_Base",
                Satellite_Type.Moon,
                buffer_max_orbits,
                buffer_shift_orbits,
                buffer_spread,
                buffer_period,
                buffer_time_offset,
                buffer_speed
            );
            // generate map
            generate_moon_map(satellite);
            satellite.has_map = true;
            satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }

        private static void generate_moon_map(Satellite satellite) {
            applySatelliteSurface(satellite.Tile);
            Map map = MapGenerator.GenerateMap(satellite.Map.Size, satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                if (weather.defName.Equals("OuterSpaceWeather")) {
                    if (Prefs.DevMode) Log.Message("RimNauts2: Found SOS2 space weather.");
                    map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                    break;
                }
            }
        }

        private static void applySatelliteSurface(int tileNum) {
            List<int> neighbors = new List<int>();
            Find.World.grid.GetTileNeighbors(tileNum, neighbors);
            foreach (int tile in neighbors) {
                Find.World.grid.tiles.ElementAt(tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(BiomeDefOf.RockMoonBiome.defName);
            }
            Find.World.grid.tiles.ElementAt(tileNum).elevation = 100f;
            Find.World.grid.tiles.ElementAt(tileNum).hilliness = RimWorld.Planet.Hilliness.Flat;
            Find.World.grid.tiles.ElementAt(tileNum).rainfall = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).swampiness = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).temperature = -100f;
            Find.World.grid.tiles.ElementAt(tileNum).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(BiomeDefOf.RockMoonBiome.defName);
        }
    }
}
