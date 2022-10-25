using System.Linq;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public static int crashing_asteroids_in_world;
        public static int mineral_asteroids_in_world;

        public override int SeedPart {
            get {
                SetSatelliteBiome.i = 0;
                return 133714088;
            }
        }

        public override void GenerateFresh(string seed) => generate_satellites();

        private void generate_satellites() {
            crashing_asteroids_in_world = 0;
            mineral_asteroids_in_world = 0;
            SatelliteContainer.reset();
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (SatelliteContainer.size() >= SatelliteDefOf.Satellite.TotalSatelliteObjects) break;
                if (SatelliteDefOf.Satellite.Biomes.Contains(biome_def)) add_satellite(i, Satellite_Type_Methods.get_type_from_biome(biome_def));
            }
        }

        public static Satellite add_satellite(int tile_id, Satellite_Type type, string def_name = "") {
            if (def_name == "") def_name = type.WorldObjects().RandomElement();
            if (Find.WorldObjects.AnyWorldObjectAt(tile_id)) {
                Satellite old_satellite = Find.WorldObjects.WorldObjectAt<Satellite>(tile_id);
                if (old_satellite.type == type) return old_satellite;
                old_satellite.type = Satellite_Type.Buffer;
                old_satellite.Destroy();
            }
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def_name, true)
            );
            satellite.Tile = tile_id;
            satellite.def_name = def_name;
            satellite.set_default_values(type);
            if (type == Satellite_Type.Moon) {
                Find.World.grid.tiles.ElementAt(tile_id).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(Moon.get_moon_biome(def_name));
            } else if (type == Satellite_Type.Artifical_Satellite) {
                Find.World.grid.tiles.ElementAt(tile_id).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("RimNauts2_Artifical_Satellite_Biome");
            } else if (type == Satellite_Type.Space_Station) {
                Find.World.grid.tiles.ElementAt(tile_id).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("RimNauts2_SpaceStation_Biome");
            }
            Find.WorldObjects.Add(satellite);
            return satellite;
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
            new_satellite.mineral_rich = satellite.mineral_rich;
            new_satellite.mineral_rich_transform_wait = (int) Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.y);
            new_satellite.mineral_rich_abondon = (int) Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.y);
            new_satellite.currently_mineral_rich = satellite.currently_mineral_rich;

            Find.WorldObjects.Add(new_satellite);
            return new_satellite;
        }
    }
}
