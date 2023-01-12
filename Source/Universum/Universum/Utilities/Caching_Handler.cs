using System.Collections.Generic;

namespace Universum.Utilities {
    public class Caching_Handler : Verse.GameComponent {
        public Dictionary<int, Dictionary<string, bool>> map_utilities;

        public Caching_Handler(Verse.Game game) : base() {
            map_utilities = new Dictionary<int, Dictionary<string, bool>>();
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

        public void recache() => map_utilities = new Dictionary<int, Dictionary<string, bool>>();
    }

    public static class Cache {
        public static Caching_Handler caching_handler;

        public static bool allowed_utility(Verse.Map map, string utility) => caching_handler.allowed_utility(map, utility);

        public static void remove(Verse.Map map) => caching_handler.map_utilities.Remove(map.uniqueID);
    }

    [HarmonyLib.HarmonyPatch(typeof(Verse.MapDeiniter), "Deinit_NewTemp")]
    public static class MapParent_Deinit_NewTemp {
        public static void Postfix(Verse.Map map) => Cache.remove(map);
    }
}
