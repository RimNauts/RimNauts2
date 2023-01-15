using System.Collections.Generic;

namespace Universum.Utilities {
    /**
     * Keeps all the cache collections and their methods.
     */
    public class Caching_Handler : Verse.GameComponent {
        public Dictionary<int, Dictionary<string, bool>> map_utilities;
        public Dictionary<string, Dictionary<string, bool>> biome_utilities;
        public Dictionary<string, Dictionary<string, bool>> terrain_utilities;
        public Dictionary<int, float> map_temperature;
        
        public Caching_Handler(Verse.Game game) : base() {
            map_utilities = new Dictionary<int, Dictionary<string, bool>>();
            biome_utilities = new Dictionary<string, Dictionary<string, bool>>();
            terrain_utilities = new Dictionary<string, Dictionary<string, bool>>();
            map_temperature = new Dictionary<int, float>();
            Cache.caching_handler = this;
        }

        public bool allowed_utility(Verse.Map map, string utility) {
            if (map == null) return false;
            // branch if map is cached
            if (map_utilities.TryGetValue(map.uniqueID, out var utilities)) {
                // branch if utility is cached
                if (utilities.TryGetValue(utility, out var cached_property_value)) return cached_property_value;
            } else map_utilities.Add(map.uniqueID, new Dictionary<string, bool>());
            // get value and cache result
            var property_value = Biome.Handler.get_properties(map.Biome).allowed_utilities.Contains(utility);
            map_utilities[map.uniqueID].Add(utility, property_value);
            return property_value;
        }

        public bool allowed_utility(RimWorld.BiomeDef biome, string utility) {
            if (biome == null) return false;
            // branch if map is cached
            if (biome_utilities.TryGetValue(biome.defName, out var utilities)) {
                // branch if utility is cached
                if (utilities.TryGetValue(utility, out var cached_property_value)) return cached_property_value;
            } else biome_utilities.Add(biome.defName, new Dictionary<string, bool>());
            // get value and cache result
            var property_value = Biome.Handler.get_properties(biome).allowed_utilities.Contains(utility);
            biome_utilities[biome.defName].Add(utility, property_value);
            return property_value;
        }

        public bool allowed_utility(Verse.TerrainDef terrain, string utility) {
            if (terrain == null) return false;
            // branch if map is cached
            if (terrain_utilities.TryGetValue(terrain.defName, out var utilities)) {
                // branch if utility is cached
                if (utilities.TryGetValue(utility, out var cached_property_value)) return cached_property_value;
            } else terrain_utilities.Add(terrain.defName, new Dictionary<string, bool>());
            // get value and cache result
            var property_value = Terrain.Handler.get_properties(terrain).allowed_utilities.Contains(utility);
            terrain_utilities[terrain.defName].Add(utility, property_value);
            return property_value;
        }

        public float temperature(Verse.Map map) {
            if (map == null) return 0.0f;
            // branch if map is cached
            if (map_temperature.TryGetValue(map.uniqueID, out var temp)) return temp;
            // get value and cache result
            var property_value = Biome.Handler.get_properties(map.Biome).temperature;
            map_temperature.Add(map.uniqueID, property_value);
            return property_value;
        }

        public void remove(Verse.Map map) {
            map_utilities.Remove(map.uniqueID);
            map_temperature.Remove(map.uniqueID);
        }

        public void clear() {
            map_utilities = new Dictionary<int, Dictionary<string, bool>>();
            biome_utilities = new Dictionary<string, Dictionary<string, bool>>();
            terrain_utilities = new Dictionary<string, Dictionary<string, bool>>();
            map_temperature = new Dictionary<int, float>();
        }
    }

    /**
     * API for interacting with Caching_Handler class.
     */
    public static class Cache {
        public static Caching_Handler caching_handler;

        public static bool allowed_utility(Verse.Map map, string utility) => caching_handler.allowed_utility(map, utility);

        public static bool allowed_utility(RimWorld.BiomeDef biome, string utility) => caching_handler.allowed_utility(biome, utility);

        public static bool allowed_utility(Verse.TerrainDef terrain, string utility) => caching_handler.allowed_utility(terrain, utility);

        public static float temperature(Verse.Map map) => caching_handler.temperature(map);

        public static void remove(Verse.Map map) => caching_handler.remove(map);

        public static void clear() => caching_handler.clear();
    }

    /**
     * Remove map from cache when map is deleted.
     */
    [HarmonyLib.HarmonyPatch(typeof(Verse.MapDeiniter), "Deinit_NewTemp")]
    public static class MapParent_Deinit_NewTemp {
        public static void Postfix(Verse.Map map) => Cache.remove(map);
    }

    /**
     * Makes sure all cache is cleared when switching between worlds.
     */
    [HarmonyLib.HarmonyPatch(typeof(Verse.Game), "ClearCaches")]
    public static class Game_ClearCaches {
        public static void Postfix() => Cache.clear();
    }
}
