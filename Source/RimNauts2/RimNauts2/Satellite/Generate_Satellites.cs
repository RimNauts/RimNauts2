using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        private void generate_satellites() {
            // generate asteroids
            for (int i = 0; i < total_satellite_amount; i++) {
                // branch to generate junk
                if (i < total_satellite_amount * 0.05f) {
                    Current.Game.GetComponent<Satellites>().tryGenSatellite(i, junk_defs);
                // branch to generate ore
                } else if (i < total_satellite_amount * 0.05f) {
                    Current.Game.GetComponent<Satellites>().tryGenSatellite(i, asteroid_ore_defs);
                // branch to generate asteroid
                } else if (i < total_satellite_amount * 0.90f) {
                    Current.Game.GetComponent<Satellites>().tryGenSatellite(i, asteroid_defs);
                }
            }
            Log.Message("Generated asteroids");
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

        public readonly static int total_satellite_amount = 200;
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
