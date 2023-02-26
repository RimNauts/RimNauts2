using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    public class Generator : WorldGenStep {
        public override int SeedPart => 111001;

        public override void GenerateFresh(string seed) => generate_fresh();

        public static void generate_fresh() {
            RenderingManager.visual_objects = new List<Objects.NEO>();
            foreach (var (type, amount) in Settings.Container.get_object_generation_steps) {
                if (type.object_holder()) {
                    add_object_holder(amount, type);
                } else {
                    add_visual_object(amount, type);
                }
            }
        }

        public static void regenerate() {
            int diff;
            foreach (var (type, amount) in Settings.Container.get_object_generation_steps) {
                if (type.object_holder()) {
                    if (amount == Cache.get_total(type)) continue;
                    List<ObjectHolder> object_holders_buffer = Caching_Handler.object_holders.Values.ToList();
                    foreach (var object_holder in object_holders_buffer) {
                        if (object_holder != null && object_holder.type == type && !object_holder.HasMap) {
                            object_holder.keep_after_abandon = false;
                            object_holder.Destroy();
                            Cache.remove(object_holder);
                        }
                    }
                    diff = amount - Cache.get_total(type);
                    add_object_holder(diff, type);
                } else {
                    if (amount == RenderingManager.get_total(type)) continue;
                    remove_visual_object(type);
                    diff = amount - RenderingManager.get_total(type);
                    add_visual_object(diff, type);
                }
            }
        }

        public static void add_asteroid_ore() {
            RenderingManager.spawn_ore_tick = RenderingManager.get_ore_timer();
            if (!Settings.Container.get_asteroid_ore_toggle || Settings.Container.get_max_asteroid_ores <= Cache.get_total(Type.AsteroidOre)) return;
            ObjectHolder object_holder = add_object_holder(Type.AsteroidOre);
            if (!Settings.Container.get_asteroid_ore_verbose) return;
            RenderingManager.update();
            Find.LetterStack.ReceiveLetter(
                "RimNauts.Label.asteroid_spawned".Translate(),
                "RimNauts.Description.asteroid_spawned".Translate(),
                RimWorld.LetterDefOf.NeutralEvent,
                (LookTargets) object_holder
            );
        }

        public static void remove_object_holder(Type type) {
            RenderingManager.visual_objects.RemoveAll(visual_object => visual_object.type == type && visual_object.object_holder == null);
            RenderingManager.recache();
        }

        public static void remove_visual_object(Type type) {
            RenderingManager.visual_objects.RemoveAll(visual_object => visual_object.type == type && visual_object.object_holder == null);
            RenderingManager.recache();
        }

        public static void remove_visual_object(int amount, Type type) {
            RenderingManager.visual_objects.RemoveAll(visual_object => {
                if (visual_object.type == type && visual_object.object_holder == null && amount > 0) {
                    amount--;
                    return true;
                } else return false;
            });
            RenderingManager.recache();
        }

        public static void remove_visual_object(Objects.NEO neo) {
            RenderingManager.visual_objects.RemoveAll(visual_object => visual_object.index == neo.index);
            RenderingManager.recache();
        }

        public static void add_visual_object(
            int amount,
            Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            Vector3? current_position = null
        ) {
            for (int i = 0; i < amount; i++) {
                add_visual_object(
                    type,
                    texture_path,
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
            }
        }

        public static Objects.NEO add_visual_object(
            Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            Vector3? current_position = null
        ) {
            Objects.NEO visual_object = type.neo(
                texture_path,
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
            RenderingManager.visual_objects.Add(visual_object);
            RenderingManager.recache();
            return visual_object;
        }

        public static void add_object_holder(
            int amount,
            Type type,
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
                add_object_holder(
                    type,
                    texture_path,
                    orbit_position,
                    orbit_speed,
                    draw_size,
                    period,
                    time_offset,
                    orbit_direction,
                    color,
                    rotation_angle,
                    current_position,
                    object_holder_def
                );
            }
        }

        public static ObjectHolder add_object_holder(
            Type type,
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
            string object_holder_def = null,
            int start_index = 0
        ) {
            Defs.ObjectHolder defs = Defs.Loader.get_object_holder(type, object_holder_def);
            if (defs == null) {
                Logger.print(
                    Logger.Importance.Error,
                    key: "RimNauts.Error.object_holder_missing_def",
                    prefix: Style.name_prefix
                );
                return null;
            }
            int tile = get_free_tile(start_index);
            if (tile == -1) return null;
            ObjectHolder object_holder = (ObjectHolder) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
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
            object_holder.texture_overlay = defs.texture_overlay;
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
            Cache.add(object_holder);
            return object_holder;
        }

        public static int get_free_tile(int start_index = 0) {
            for (int i = start_index; i < Find.World.grid.TilesCount; i++) {
                if (Find.World.grid.tiles.ElementAt(i).biome.defName == "Ocean" && !Find.World.worldObjects.AnyWorldObjectAt(i)) {
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
                    return i;
                }
            }
            return -1;
        }
    }
}
