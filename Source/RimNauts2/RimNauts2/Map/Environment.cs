using System.Linq;
using Verse;

namespace RimNauts2 {
    class Environment : GenStep {
        public float elevation = 100f;
        public RimWorld.Planet.Hilliness hilliness = RimWorld.Planet.Hilliness.Flat;
        public float rainfall = 0f;
        public float swampiness = 0f;
        public float temperature = -40f;
        public bool apply_SOS2_weather = true;

        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            if (apply_SOS2_weather) {
                foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                    if (weather.defName.Equals("OuterSpaceWeather")) {
                        map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                        if (Prefs.DevMode) Log.Message("RimNauts2: Found SOS2 space weather.");
                        break;
                    }
                }
            }
            Find.World.grid.tiles.ElementAt(map.Tile).elevation = elevation;
            Find.World.grid.tiles.ElementAt(map.Tile).hilliness = hilliness;
            Find.World.grid.tiles.ElementAt(map.Tile).rainfall = rainfall;
            Find.World.grid.tiles.ElementAt(map.Tile).swampiness = swampiness;
            Find.World.grid.tiles.ElementAt(map.Tile).temperature = temperature;
        }
    }
}
