using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    public class Generate_Satellites : WorldGenStep {
        public static int crashing_asteroids_in_world;
        public static int mineral_asteroids_in_world;
        public static bool halt_caching = false;

        public override int SeedPart {
            get {
                //SetSatelliteBiome.i = 0;
                return 133714088;
            }
        }

        public static int get_free_tile(int start_index = 0, string new_biome_def = null) {
            for (int i = start_index; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (biome_def == "Ocean") {
                    List<int> neighbors = new List<int>();
                    Find.World.grid.GetTileNeighbors(i, neighbors);
                    var flag = false;
                    foreach (var neighbour in neighbors) {
                        var neighbour_tile = Find.World.grid.tiles.ElementAtOrDefault(neighbour);
                        if (neighbour_tile != default(RimWorld.Planet.Tile)) {
                            if (neighbour_tile.biome != RimWorld.BiomeDefOf.Ocean) {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) continue;
                    if (new_biome_def != null) Find.World.grid.tiles.ElementAt(i).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(new_biome_def);
                    return i;
                }
            }
            return -1;
        }

        public static World.ObjectHolder add_object_holder(string def_name) {
            Defs.ObjectHolder defs = (Defs.ObjectHolder) GenDefDatabase.GetDef(typeof(Defs.ObjectHolder), def_name);
            int tile = get_free_tile(new_biome_def: defs.biome_def);
            if (tile == -1) return null;
            World.ObjectHolder object_holder = (World.ObjectHolder) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RimNauts2_ObjectHolder")
            );
            object_holder.Tile = tile;
            object_holder.def.mapGenerator = defs.map_generator;
            object_holder.def.label = defs.label;
            object_holder.def.description = defs.description;
            object_holder.keep_after_abandon = defs.keep_after_abandon;
            string texture_path = null;
            if (!defs.texture_paths.NullOrEmpty()) texture_path = defs.texture_paths.RandomElement();
            object_holder.add_visual_object(
                type: (World.Type) defs.type,
                texture_path: texture_path
            );
            if (defs.limited_days_between != null) {
                Vector2 days_between = (Vector2) defs.limited_days_between;
                object_holder.add_expiration_date(days_between.x, days_between.y);
            }
            Find.WorldObjects.Add(object_holder);
            World.Cache.add(object_holder);
            return object_holder;
        }

        public static void add_render_manager() {
            int tile = get_free_tile(new_biome_def: "RimNauts2_Satellite_Biome");
            if (tile == -1) return;
            World.RenderManager render_manager = (World.RenderManager) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RimNauts2_RenderManager")
            );
            render_manager.Tile = tile;
            Find.WorldObjects.Add(render_manager);
        }

        public override void GenerateFresh(string seed) {
            //generate_satellites();
            add_render_manager();
            RimNauts_GameComponent.render_manager.populate(amount: 1000, World.Type.Asteroid);
            RimNauts_GameComponent.render_manager.populate(amount: 50, World.Type.AsteroidCrashing);

            add_object_holder(def_name: "RimNauts2_ObjectHolder_WaterMoon");
        }

        public static void regenerate_satellites() {
            halt_caching = true;
            int satellite_biomes_added = 0;
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.starting_satellite_objects_cleanup",
                prefix: Style.name_prefix,
                args: new NamedArgument[] { SatelliteContainer.size() }
            );
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (biome_def.Contains("RimNauts2")) {
                    if (Find.WorldObjects.AnyWorldObjectAt<Satellite>(i)) {
                        Satellite satellite = Find.WorldObjects.WorldObjectAt<Satellite>(i);
                        if (!satellite.HasMap && (satellite.type == Satellite_Type.Asteroid || satellite.type == Satellite_Type.Asteroid_Ore || satellite.type == Satellite_Type.Buffer || satellite.type == Satellite_Type.None)) {
                            satellite.type = Satellite_Type.None;
                            satellite.Destroy();
                        } else satellite_biomes_added++;
                    }
                }
            }
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.finished_satellite_objects_cleanup",
                prefix: Style.name_prefix,
                args: new NamedArgument[] { SatelliteContainer.size() }
            );
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (biome_def == "Ocean") {
                    List<int> neighbors = new List<int>();
                    Find.World.grid.GetTileNeighbors(i, neighbors);
                    var flag = false;
                    foreach (var neighbour in neighbors) {
                        var neighbour_tile = Find.World.grid.tiles.ElementAtOrDefault(neighbour);
                        if (neighbour_tile != default(RimWorld.Planet.Tile)) {
                            if (neighbour_tile.biome != RimWorld.BiomeDefOf.Ocean) {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) continue;
                    Find.World.grid.tiles.ElementAt(i).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("RimNauts2_Satellite_Biome");
                    satellite_biomes_added++;
                    if (satellite_biomes_added >= Settings.TotalSatelliteObjects) break;
                }
            }
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.finished_adding_back_biomes",
                prefix: Style.name_prefix,
                args: new NamedArgument[] { satellite_biomes_added }
            );
            generate_satellites(overwrite: false);
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.finished_adding_back_satellite_objects",
                prefix: Style.name_prefix,
                args: new NamedArgument[] { satellite_biomes_added }
            );
            halt_caching = false;
        }

        private static void generate_satellites(bool overwrite = true) {
            crashing_asteroids_in_world = 0;
            mineral_asteroids_in_world = 0;
            SatelliteContainer.reset();
            halt_caching = true;
            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                if (!overwrite && Find.WorldObjects.AnyWorldObjectAt<Satellite>(i)) {
                    Satellite satellite = Find.WorldObjects.WorldObjectAt<Satellite>(i);
                    SatelliteContainer.add(satellite);
                    switch (satellite.type) {
                        case Satellite_Type.Asteroid:
                            RimNauts_GameComponent.total_asteroids++;
                            break;
                        case Satellite_Type.Moon:
                            RimNauts_GameComponent.total_moons++;
                            break;
                        case Satellite_Type.Artifical_Satellite:
                            RimNauts_GameComponent.total_artifical_satellites++;
                            break;
                    }
                    continue;
                }
                string biome_def = Find.World.grid.tiles.ElementAt(i).biome.defName;
                if (SatelliteContainer.size() >= Settings.TotalSatelliteObjects) break;
                if (biome_def == "RimNauts2_Satellite_Biome") try { add_satellite(i, Satellite_Type.Asteroid); } catch { }
            }
            halt_caching = false;
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
            Find.WorldObjects.Add(satellite);

            return satellite;
        }

        public static SatelliteSettings copy_satellite(Satellite satellite, string new_def_name = "", Satellite_Type new_type = Satellite_Type.None) {
            SatelliteSettings satellite_settings = new SatelliteSettings();

            string def_name;
            if (new_def_name != "") {
                def_name = new_def_name;
            } else def_name = satellite.def_name;

            Satellite_Type type;
            if (new_type != Satellite_Type.None) {
                type = new_type;
            } else type = satellite.type;

            satellite_settings.Tile = satellite.Tile;
            satellite_settings.def_name = def_name;
            satellite_settings.type = type;
            satellite_settings.orbit_position = satellite.orbit_position;
            satellite_settings.orbit_spread = satellite.orbit_spread;
            satellite_settings.orbit_speed = satellite.orbit_speed;
            satellite_settings.period = satellite.period;
            satellite_settings.time_offset = satellite.time_offset;
            satellite_settings.can_out_of_bounds = satellite.can_out_of_bounds;
            satellite_settings.out_of_bounds_offset = satellite.out_of_bounds_offset;
            satellite_settings.current_out_of_bounds = satellite.current_out_of_bounds;
            satellite_settings.out_of_bounds_direction_towards_surface = satellite.out_of_bounds_direction_towards_surface;
            satellite_settings.orbit_random_direction = satellite.orbit_random_direction;
            satellite_settings.mineral_rich = satellite.mineral_rich;
            satellite_settings.mineral_rich_transform_wait = SatelliteDefOf.Satellite.MineralAppearWait;
            satellite_settings.mineral_rich_abondon = SatelliteDefOf.Satellite.MineralAbondonWait;
            satellite_settings.currently_mineral_rich = satellite.currently_mineral_rich;
            return satellite_settings;
        }

        public static Satellite paste_satellite(SatelliteSettings old_satellite) {
            Satellite new_satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(old_satellite.def_name, true)
            );
            new_satellite.Tile = old_satellite.Tile;
            new_satellite.def_name = old_satellite.def_name;
            new_satellite.type = old_satellite.type;
            new_satellite.orbit_position = old_satellite.orbit_position;
            new_satellite.orbit_spread = old_satellite.orbit_spread;
            new_satellite.orbit_speed = old_satellite.orbit_speed;
            new_satellite.period = old_satellite.period;
            new_satellite.time_offset = old_satellite.time_offset;
            new_satellite.can_out_of_bounds = old_satellite.can_out_of_bounds;
            new_satellite.out_of_bounds_offset = old_satellite.out_of_bounds_offset;
            new_satellite.current_out_of_bounds = old_satellite.current_out_of_bounds;
            new_satellite.out_of_bounds_direction_towards_surface = old_satellite.out_of_bounds_direction_towards_surface;
            new_satellite.orbit_random_direction = old_satellite.orbit_random_direction;
            new_satellite.mineral_rich = old_satellite.mineral_rich;
            new_satellite.mineral_rich_transform_wait = old_satellite.mineral_rich_transform_wait;
            new_satellite.mineral_rich_abondon = old_satellite.mineral_rich_abondon;
            new_satellite.currently_mineral_rich = old_satellite.currently_mineral_rich;
            Find.WorldObjects.Add(new_satellite);
            return new_satellite;
        }
    }

    public struct SatelliteSettings {
        public int Tile;
        public string def_name;
        public Satellite_Type type;
        public Vector3 orbit_position;
        public Vector3 orbit_spread;
        public float orbit_speed;
        public float period;
        public int time_offset;
        public bool can_out_of_bounds;
        public float out_of_bounds_offset;
        public float current_out_of_bounds;
        public bool out_of_bounds_direction_towards_surface;
        public int orbit_random_direction;
        public bool mineral_rich;
        public int mineral_rich_transform_wait;
        public int mineral_rich_abondon;
        public bool currently_mineral_rich;
    }
}
