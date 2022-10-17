using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public readonly static int total_satellite_amount = 1000;
        public readonly static int total_moon_amount = 1;
        private readonly static List<string> asteroid_defs = new List<string>() {
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
        private readonly List<string> moon_defs = new List<string>() {
            "RockMoon",
        };

        public override int SeedPart {
            get {
                SetSatelliteBiome.i = 0;
                return 133714088;
            }
        }

        public override void GenerateFresh(string seed) {
            generate_satellites();
        }

        public override void GenerateFromScribe(string seed) {
            GenerateFresh(seed);
        }

        private void generate_satellites() {
            SatelliteContainer.clear();
            List<int> suitable_tile_ids = new List<int>();
            int moons_added = 0;
            int satellites_added = 0;

            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                if (Find.World.grid.tiles.ElementAt(i).biome == BiomeDefOf.RockMoonBiome) moons_added++;
                if (Find.World.grid.tiles.ElementAt(i).biome == BiomeDefOf.SatelliteBiome) {
                    suitable_tile_ids.Add(i);

                    if (moons_added < total_moon_amount) {
                        add_satellite(i, moon_defs, Satellite_Type.Moon);
                        moons_added++;
                    } else if (satellites_added < total_satellite_amount) {
                        add_satellite(i, asteroid_defs, Satellite_Type.Asteroid);
                        satellites_added++;
                    } else break;
                }
            }
        }

        private void add_satellite(int tile_id, List<string> defs, Satellite_Type type) {
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                        DefDatabase<RimWorld.WorldObjectDef>.GetNamed(defs.RandomElement(), true)
                );
            satellite.Tile = tile_id;
            satellite.type = type;
            Find.WorldObjects.Add(satellite);
            SatelliteContainer.add(satellite);
        }
    }
}
