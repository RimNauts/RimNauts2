using System.Linq;
using Verse;

namespace RimNauts2 {
    class Environment : GenStep {
        public RimWorld.Planet.Hilliness hilliness = RimWorld.Planet.Hilliness.Flat;
        public bool apply_SOS2_weather = true;
        public string weather_def = null;

        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            if (apply_SOS2_weather) {
                if (ModsConfig.IsActive("kentington.saveourship2")) {
                    map.weatherManager.lastWeather = WeatherDef.Named("OuterSpaceWeather");
                    map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                    if (Prefs.DevMode) {
                        Logger.print(
                            Logger.Importance.Info,
                            key: "RimNauts.Info.sos_found",
                            prefix: Style.name_prefix
                        );
                    }
                } else if (weather_def != null) {
                    map.weatherManager.lastWeather = WeatherDef.Named(weather_def);
                    map.weatherManager.curWeather = WeatherDef.Named(weather_def);
                }
            } else if (weather_def != null) {
                map.weatherManager.lastWeather = WeatherDef.Named(weather_def);
                map.weatherManager.curWeather = WeatherDef.Named(weather_def);
            }
            Find.World.grid.tiles.ElementAt(map.Tile).hilliness = hilliness;
        }
    }
}
