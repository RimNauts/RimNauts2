using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        private void generate_satellites() {
            // generate asteroids
            for (int i = 0; i < SatelliteDef.total_satellite_amount * SatelliteDef.asteroid_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    SatelliteDef.asteroid_defs
                );
            }
            // generate ores
            for (int i = 0; i < SatelliteDef.total_satellite_amount * SatelliteDef.ore_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    SatelliteDef.asteroid_ore_defs
                );
            }
            // generate junk
            for (int i = 0; i < SatelliteDef.total_satellite_amount * SatelliteDef.junk_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    SatelliteDef.junk_defs
                );
            }
        }

        public override void GenerateFresh(string seed) {
            generate_satellites();
        }

        public override void GenerateFromScribe(string seed) {
            generate_satellites();
        }

        public override int SeedPart {
            get {
                return 133714088;
            }
        }
    }
}
