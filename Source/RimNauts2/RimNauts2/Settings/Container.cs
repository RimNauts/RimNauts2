using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.Settings {
    public class Container : ModSettings {
        public static Dictionary<World.Type, int> object_generation_steps = new Dictionary<World.Type, int>();
        public static bool? asteroid_ore_toggle;
        public static int? max_asteroid_ores;
        public static bool? asteroid_ore_verbose;
        public static bool? multi_threaded_update;
        public static bool? incident_patch;
        public static bool? world_feature_name;

        public override void ExposeData() {
            Scribe_Collections.Look(ref object_generation_steps, "object_generation_steps", LookMode.Value, LookMode.Value);
            Scribe_Values.Look(ref asteroid_ore_toggle, "asteroid_ore_toggle");
            Scribe_Values.Look(ref max_asteroid_ores, "max_asteroid_ores");
            Scribe_Values.Look(ref asteroid_ore_verbose, "asteroid_ore_verbose");
            Scribe_Values.Look(ref multi_threaded_update, "multi_threaded_update");
            Scribe_Values.Look(ref incident_patch, "incident_patch");
            Scribe_Values.Look(ref world_feature_name, "world_feature_name");
        }

        public static void clear() {
            object_generation_steps = null;
            asteroid_ore_toggle = null;
            max_asteroid_ores = null;
            asteroid_ore_verbose = null;
            multi_threaded_update = null;
            incident_patch = null;
            world_feature_name = null;
        }

        public static Dictionary<World.Type, int> get_object_generation_steps {
            get {
                if (!object_generation_steps.NullOrEmpty()) return object_generation_steps;
                // return default from def if not found
                object_generation_steps = new Dictionary<World.Type, int>();
                foreach (var (type, def) in Defs.Loader.object_generation_steps) {
                    object_generation_steps.Add(type, def.amount);
                }
                return object_generation_steps;
            }
        }

        public static bool get_asteroid_ore_toggle {
            get {
                if (asteroid_ore_toggle != null) return (bool) asteroid_ore_toggle;
                asteroid_ore_toggle = true;
                return (bool) asteroid_ore_toggle;
            }
        }

        public static int get_max_asteroid_ores {
            get {
                if (max_asteroid_ores != null) return (int) max_asteroid_ores;
                max_asteroid_ores = 30;
                return (int) max_asteroid_ores;
            }
        }

        public static bool get_asteroid_ore_verbose {
            get {
                if (asteroid_ore_verbose != null) return (bool) asteroid_ore_verbose;
                asteroid_ore_verbose = true;
                return (bool) asteroid_ore_verbose;
            }
        }

        public static bool get_multi_threaded_update {
            get {
                if (multi_threaded_update != null) return (bool) multi_threaded_update;
                multi_threaded_update = true;
                return (bool) multi_threaded_update;
            }
        }

        public static bool get_incident_patch {
            get {
                if (incident_patch != null) return (bool) incident_patch;
                incident_patch = true;
                return (bool) incident_patch;
            }
        }

        public static bool get_world_feature_name {
            get {
                if (world_feature_name != null) return (bool) world_feature_name;
                world_feature_name = true;
                return (bool) world_feature_name;
            }
        }
    }
}
