using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public readonly static int total_satellite_amount = 1000;
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

        public override int SeedPart {
            get {
                return 133714088;
            }
        }

        public override void GenerateFresh(string seed) {
            generate_satellites();
        }

        public override void GenerateFromScribe(string seed) {
            generate_satellites();
        }

        private void generate_satellites() {
            int i = 0;
            while (i < total_satellite_amount) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite(i, Satellite_Type.Asteroid, asteroid_defs);
                i++;
            }
        }
    }
}
