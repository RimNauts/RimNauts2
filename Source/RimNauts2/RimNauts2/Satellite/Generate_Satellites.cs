using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        private void generate_satellites() {
            // generate asteroids
            for (int i = 0; i < total_satellite_amount * asteroid_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    asteroid_defs
                );
            }
            // generate ores
            for (int i = 0; i < total_satellite_amount * ore_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    asteroid_ore_defs
                );
            }
            // generate junk
            for (int i = 0; i < total_satellite_amount * junk_percent; i++) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(
                    Find.World.grid.TilesCount - 1,
                    junk_defs
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

        private readonly int total_satellite_amount = 200;
        private readonly float asteroid_percent = 0.90f;
        private readonly float ore_percent = 0.05f;
        private readonly float junk_percent = 0.05f;
        private readonly List<string> asteroid_defs = new List<string>() {
            "asteroid_1",
            "asteroid_2",
            "asteroid_3",
            "asteroid_4",
            "asteroid_5",
            "asteroid_6",
            "asteroid_7",
            "asteroid_8",
            "asteroid_9",
        };
        private readonly List<string> asteroid_ore_defs = new List<string>() {
            "ore_steel",
            "ore_gold",
            "ore_plasteel",
        };
        private readonly List<string> junk_defs = new List<string>() {
            "junk_1",
            "junk_2",
            "junk_3",
            "junk_4",
        };
    }
}
