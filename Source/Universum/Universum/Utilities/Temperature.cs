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
}
