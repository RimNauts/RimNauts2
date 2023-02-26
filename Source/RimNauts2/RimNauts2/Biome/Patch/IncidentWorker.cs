namespace RimNauts2.Biome.Patch {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            if (parms.forced) return;
            try {
                if (!__result || !World.Cache.exists(parms.target.Tile)) return;
                if (SatelliteDefOf.Satellite.AllowedSatelliteIncidents.Contains(__instance.def.defName)) return;
                __result = false;
            } catch { }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PawnsArrivalModeWorker), "CanUseWith")]
    class PawnsArrivalModeWorker_CanUseWith {
        public static void Postfix(RimWorld.IncidentParms parms, ref RimWorld.PawnsArrivalModeWorker __instance, bool __result) {
            if (__result && World.Cache.exists(parms.target.Tile)) __instance.def.minTechLevel = RimWorld.TechLevel.Industrial;
        }
    }
}
