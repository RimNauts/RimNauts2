using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public readonly static int total_satellite_amount = 1000;
        public readonly static int total_moon_amount = 1;
        private readonly static List<string> asteroid_defs = new List<string>() {
            "RimNauts2_Asteroid_1",
            "RimNauts2_Asteroid_2",
            "RimNauts2_Asteroid_3",
            "RimNauts2_Asteroid_4",
            "RimNauts2_Asteroid_5",
            "RimNauts2_Asteroid_6",
            "RimNauts2_Asteroid_7",
            "RimNauts2_Asteroid_8",
            "RimNauts2_Asteroid_9",
        };
        private readonly List<string> asteroid_ore_defs = new List<string>() {
            "RimNauts2_Ore_Steel",
            "RimNauts2_Ore_Gold",
            "RimNauts2_Ore_Plasteel",
        };
        private readonly List<string> junk_defs = new List<string>() {
            "RimNauts2_Waste_Satellite",
        };
        private readonly List<string> moon_defs = new List<string>() {
            "Moon",
        };

        public override int SeedPart {
            get {
                SetSatelliteBiome.i = 0;
                return 133714088;
            }
        }

        public override void GenerateFresh(string seed) => generate_satellites();

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
            string def_name = defs.RandomElement();
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def_name, true)
            );
            satellite.Tile = tile_id;
            satellite.def_name = def_name;
            satellite.type = type;
            Find.WorldObjects.Add(satellite);
            SatelliteContainer.add(satellite);
        }

        public static Satellite copy_satellite(
            int tile_id,
            string def_name,
            Satellite_Type type,
            Vector3 max_orbits,
            Vector3 shift_orbits,
            Vector3 spread,
            float period,
            int time_offset,
            float speed
        ) {
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def_name, true)
            );
            satellite.Tile = tile_id;
            satellite.def_name = def_name;
            satellite.type = type;
            satellite.max_orbits = max_orbits;
            satellite.shift_orbits = shift_orbits;
            satellite.spread = spread;
            satellite.period = period;
            satellite.time_offset = time_offset;
            satellite.speed = speed;
            Find.WorldObjects.Add(satellite);
            SatelliteContainer.add(satellite);
            return satellite;
        }
    }
}
