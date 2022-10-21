using System.Linq;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public static int crashing_asteroids_in_world = 0;

        public override int SeedPart {
            get {
                SetSatelliteBiome.i = 0;
                return 133714088;
            }
        }

        public override void GenerateFresh(string seed) => generate_satellites();

        private void generate_satellites() {
            crashing_asteroids_in_world = 0;
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (biome_def == "Ocean" || SatelliteContainer.size() >= SatelliteDefOf.Satellite.TotalSatelliteObjects) break;
                if (SatelliteDefOf.Satellite.Biomes.Contains(biome_def)) add_satellite(i, Satellite_Type_Methods.get_type_from_biome(biome_def));
            }
        }

        public static void add_satellite(int tile_id, Satellite_Type type) {
            string def_name = type.WorldObjects().RandomElement();
            if (Find.WorldObjects.AnyWorldObjectAt(tile_id)) {
                Satellite old_satellite = Find.WorldObjects.WorldObjectAt<Satellite>(tile_id);
                if (old_satellite.type == type) return;
                old_satellite.Destroy();
            }
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def_name, true)
            );
            satellite.Tile = tile_id;
            satellite.def_name = def_name;
            satellite.set_default_values(type);
            if (type == Satellite_Type.Moon) Find.World.grid.tiles.ElementAt(tile_id).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(Moon.get_moon_biome(def_name));
            Find.WorldObjects.Add(satellite);
        }

        public static Satellite copy_satellite(
            int tile_id,
            string def_name,
            Satellite_Type type,
            Vector3 max_orbits,
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
            satellite.orbit_position = max_orbits;
            satellite.orbit_spread = spread;
            satellite.period = period;
            satellite.time_offset = time_offset;
            satellite.orbit_speed = speed;
            Find.WorldObjects.Add(satellite);
            return satellite;
        }
    }
}
