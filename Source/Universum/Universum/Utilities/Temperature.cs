using System.Reflection;

namespace Universum.Utilities {
    [HarmonyLib.HarmonyPatch(typeof(Verse.MapTemperature), "OutdoorTemp", HarmonyLib.MethodType.Getter)]
    public static class MapTemperature_OutdoorTemp {
        public static void Postfix(ref float __result, Verse.Map ___map) {
            if (Cache.allowed_utility(___map, "(Universum) Temperature")) __result = Cache.temperature(___map);
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Verse.MapTemperature), "SeasonalTemp", HarmonyLib.MethodType.Getter)]
    public static class MapTemperature_SeasonalTemp {
        public static void Postfix(ref float __result, Verse.Map ___map) {
            if (Cache.allowed_utility(___map, "(Universum) Temperature")) __result = Cache.temperature(___map);
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Verse.RoomTempTracker), "WallEqualizationTempChangePerInterval")]
    public static class RoomTempTracker_WallEqualizationTempChangePerInterval {
        public static void Postfix(ref float __result, Verse.RoomTempTracker __instance) {
            Verse.Room room = (Verse.Room) typeof(Verse.RoomTempTracker).GetField("room", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            if (Cache.allowed_utility(room.Map, "(Universum) Temperature")) __result *= 0.01f;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(Verse.RoomTempTracker), "ThinRoofEqualizationTempChangePerInterval")]
    public static class RoomTempTracker_ThinRoofEqualizationTempChangePerInterval {
        public static void Postfix(ref float __result, Verse.RoomTempTracker __instance) {
            Verse.Room room = (Verse.Room) typeof(Verse.RoomTempTracker).GetField("room", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            if (Cache.allowed_utility(room.Map, "(Universum) Temperature")) __result *= 0.01f;
        }
    }
}
