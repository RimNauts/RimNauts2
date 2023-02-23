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

        public static void add_object_holder(
            int amount,
            World.Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            Vector3? current_position = null,
            string object_holder_def = null
        ) {
            for (int i = 0; i < amount; i++) {
                Defs.ObjectHolder defs = Defs.Loader.get_object_holder(type, object_holder_def);
                if (defs == null) return;
                int tile = get_free_tile(new_biome_def: defs.biome_def);
                if (tile == -1) return;
                World.ObjectHolder object_holder = (World.ObjectHolder) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                    DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RimNauts2_ObjectHolder")
                );
                string random_texture_path = null;
                if (texture_path == null && !defs.texture_paths.NullOrEmpty()) random_texture_path = defs.texture_paths.RandomElement();
                if (random_texture_path == null) random_texture_path = texture_path;
                object_holder.Tile = tile;
                object_holder.map_generator = defs.map_generator;
                object_holder.label = defs.label;
                object_holder.description = defs.description;
                object_holder.keep_after_abandon = defs.keep_after_abandon;
                object_holder.add_visual_object(
                    type,
                    random_texture_path,
                    orbit_position,
                    orbit_speed,
                    draw_size,
                    period,
                    time_offset,
                    orbit_direction,
                    color,
                    rotation_angle,
                    current_position
                );
                if (defs.limited_days_between != null) {
                    Vector2 days_between = (Vector2) defs.limited_days_between;
                    object_holder.add_expiration_date(days_between.x, days_between.y);
                }
                Find.WorldObjects.Add(object_holder);
                World.Cache.add(object_holder);
            }
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
            add_render_manager();
            foreach (var object_generation_step in Defs.Loader.object_generation_steps) {
                if (Defs.Loader.get_object_holder((World.Type) object_generation_step.type) != null) {
                    add_object_holder(
                        object_generation_step.amount,
                        (World.Type) object_generation_step.type,
                        object_generation_step.texture_path,
                        object_generation_step.orbit_position,
                        object_generation_step.orbit_speed,
                        object_generation_step.draw_size,
                        object_generation_step.period,
                        object_generation_step.time_offset,
                        object_generation_step.orbit_direction,
                        object_generation_step.color,
                        object_generation_step.rotation_angle,
                        object_generation_step.current_position
                    );
                } else {
                    World.Caching_Handler.render_manager.populate(
                        object_generation_step.amount,
                        (World.Type) object_generation_step.type,
                        object_generation_step.texture_path,
                        object_generation_step.orbit_position,
                        object_generation_step.orbit_speed,
                        object_generation_step.draw_size,
                        object_generation_step.period,
                        object_generation_step.time_offset,
                        object_generation_step.orbit_direction,
                        object_generation_step.color,
                        object_generation_step.rotation_angle,
                        object_generation_step.current_position
                    );
                }
            }
        }
    }
}
