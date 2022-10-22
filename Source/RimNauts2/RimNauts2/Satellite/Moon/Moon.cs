using System.Linq;
using Verse;

namespace RimNauts2 {
    public static class Moon {
        public static void generate_moon(Satellite satellite) {
            // generate new satellite with the values saved from before (this process is done to get the new texture)
            Satellite new_satellite = Generate_Satellites.copy_satellite(satellite, get_moon_base(satellite.def_name), Satellite_Type.Moon);
            satellite.Destroy();
            // generate map
            generate_moon_map(new_satellite);
            new_satellite.has_map = true;
            new_satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }

        private static void generate_moon_map(Satellite satellite) {
            applySatelliteSurface(satellite.Tile);
            Map map = MapGenerator.GenerateMap(new IntVec3(250, 1, 250), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                if (weather.defName.Equals("OuterSpaceWeather")) {
                    map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                    if (Prefs.DevMode) Log.Message("RimNauts2: Found SOS2 space weather.");
                    break;
                }
            }
        }

        private static void applySatelliteSurface(int tile_id) {
            Find.World.grid.tiles.ElementAt(tile_id).elevation = 100f;
            Find.World.grid.tiles.ElementAt(tile_id).hilliness = RimWorld.Planet.Hilliness.Flat;
            Find.World.grid.tiles.ElementAt(tile_id).rainfall = 0f;
            Find.World.grid.tiles.ElementAt(tile_id).swampiness = 0f;
            Find.World.grid.tiles.ElementAt(tile_id).temperature = -100f;
        }

        private static string get_moon_base(string moon) => moon + "_Base";
        public static string get_moon_biome(string moon) => "RimNauts2_MoonBarren_Biome"; // moon + "_Biome"; <<< needs other biomes to work
    }
}
