using System.Linq;
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
            int satellite_objects = 0;
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (biome_def == "Ocean" || satellite_objects >= SatelliteDefOf.Satellite.TotalSatelliteObjects) break;
                if (SatelliteDefOf.Satellite.Biomes.Contains(biome_def)) add_satellite(i, Satellite_Type_Methods.get_type_from_biome(biome_def));
                satellite_objects++;
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

        public static Satellite copy_satellite(Satellite satellite, string new_def_name = "", Satellite_Type new_type = Satellite_Type.None) {
            string def_name;
            if (new_def_name != "") {
                def_name = new_def_name;
            } else def_name = satellite.def_name;

            Satellite_Type type;
            if (new_type != Satellite_Type.None) {
                type = new_type;
            } else type = satellite.type;

            Satellite new_satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def_name, true)
            );

            new_satellite.Tile = satellite.Tile;
            new_satellite.def_name = def_name;
            new_satellite.type = type;
            new_satellite.orbit_position = satellite.orbit_position;
            new_satellite.orbit_spread = satellite.orbit_spread;
            new_satellite.orbit_speed = satellite.orbit_speed;
            new_satellite.period = satellite.period;
            new_satellite.time_offset = satellite.time_offset;
            new_satellite.can_out_of_bounds = satellite.can_out_of_bounds;
            new_satellite.out_of_bounds_offset = satellite.out_of_bounds_offset;
            new_satellite.current_out_of_bounds = satellite.current_out_of_bounds;
            new_satellite.out_of_bounds_direction_towards_surface = satellite.out_of_bounds_direction_towards_surface;
            new_satellite.orbit_random_direction = satellite.orbit_random_direction;

            Find.WorldObjects.Add(new_satellite);
            return new_satellite;
        }
    }
}
