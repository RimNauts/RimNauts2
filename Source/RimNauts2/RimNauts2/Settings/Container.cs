using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.Settings {
    public class Container : ModSettings {
        public static bool? asteroid_ore_verbose;
        public static bool? incident_patch;
        public static bool? allow_raids_on_neos;
        public static bool? allow_quests_on_neos;

        public override void ExposeData() {
            Scribe_Values.Look(ref asteroid_ore_verbose, "asteroid_ore_verbose");
            Scribe_Values.Look(ref incident_patch, "incident_patch");
            Scribe_Values.Look(ref allow_raids_on_neos, "allow_raids_on_neos");
            Scribe_Values.Look(ref allow_quests_on_neos, "allow_quests_on_neos");
        }

        public static void clear() {
            asteroid_ore_verbose = null;
            incident_patch = null;
            allow_raids_on_neos = null;
            allow_quests_on_neos = null;
        }

        public static bool get_asteroid_ore_verbose {
            get {
                if (asteroid_ore_verbose != null) return (bool) asteroid_ore_verbose;
                asteroid_ore_verbose = true;
                return (bool) asteroid_ore_verbose;
            }
        }

        public static bool get_incident_patch {
            get {
                if (incident_patch != null) return (bool) incident_patch;
                incident_patch = true;
                return (bool) incident_patch;
            }
        }

        public static bool get_allow_raids_on_neos {
            get {
                if (allow_raids_on_neos != null) return (bool) allow_raids_on_neos;
                allow_raids_on_neos = true;
                return (bool) allow_raids_on_neos;
            }
        }

        public static bool get_allow_quests_on_neos {
            get {
                if (allow_quests_on_neos != null) return (bool) allow_quests_on_neos;
                allow_quests_on_neos = true;
                return (bool) allow_quests_on_neos;
            }
        }
    }
}
