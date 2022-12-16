using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    public class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            try {
                if (!__result) return;
                bool incident_not_on_satellite = !Find.WorldGrid[parms.target.Tile].biome.defName.Contains("RimNauts2");
                if (incident_not_on_satellite) return;
                if (SatelliteDefOf.Satellite.AllowedSatelliteIncidents.Contains(__instance.def.defName)) return;
                __result = false;
            } catch { }
        }
    }

    [HarmonyPatch(typeof(RimWorld.PawnsArrivalModeWorker), nameof(RimWorld.PawnsArrivalModeWorker.CanUseWith))]
    public class PawnsArrivalModeWorker_CanUseWith_Patch {
        public static void Postfix(RimWorld.IncidentParms parms, ref bool __result) {
            if (__result && Find.WorldGrid[parms.target.Tile].biome.defName.Contains("RimNauts2")) {
                if (parms.raidArrivalMode.minTechLevel >= RimWorld.TechLevel.Industrial) {
                    __result = false;
                }
            }
        }
    }
}
