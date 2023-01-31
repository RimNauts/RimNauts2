using System.Linq;
using Verse;

namespace RimNauts2 {
    class Environment : GenStep {
        public RimWorld.Planet.Hilliness hilliness = RimWorld.Planet.Hilliness.Flat;
        public bool apply_SOS2_weather = true;

        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            if (apply_SOS2_weather && ModsConfig.IsActive("kentington.saveourship2")) {
                foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                    if (weather.defName.Equals("OuterSpaceWeather")) {
                        map.weatherManager.lastWeather = WeatherDef.Named("OuterSpaceWeather");
                        map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                        if (Prefs.DevMode) {
                            Logger.print(
                                Logger.Importance.Info,
                                key: "RimNauts.Info.sos_found",
                                prefix: Style.name_prefix
                            );
                        }
                        break;
                    }
                }
            } else {
                map.weatherManager.lastWeather = WeatherDef.Named("RimNauts2_OuterSpaceWeather");
                map.weatherManager.curWeather = WeatherDef.Named("RimNauts2_OuterSpaceWeather");
            }
            Find.World.grid.tiles.ElementAt(map.Tile).hilliness = hilliness;
        }
    }
}
