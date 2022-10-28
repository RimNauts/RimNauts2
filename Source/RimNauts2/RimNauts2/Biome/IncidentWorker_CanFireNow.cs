using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    public class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            try {
                if (!__result) return;
                bool incident_not_on_moon_biome = Find.WorldGrid[parms.target.Tile].biome.defName != "RimNauts2_Satellite_Biome";
                if (incident_not_on_moon_biome) return;
                if (SatelliteDefOf.Satellite.AllowedSatelliteIncidents.Contains(__instance.def.defName)) return;
                __result = false;
            } catch { }
            return;
        }
    }

    [HarmonyPatch(typeof(RimWorld.PawnsArrivalModeWorker), nameof(RimWorld.PawnsArrivalModeWorker.CanUseWith))]
    public class PawnsArrivalModeWorker_CanUseWith_Patch {
        public static void Postfix(RimWorld.IncidentParms parms, ref bool __result) {
            if (__result && Find.WorldGrid[parms.target.Tile].biome.defName.Contains("RimNauts2")) {
                if (parms.raidArrivalMode.minTechLevel >= RimWorld.TechLevel.Industrial) {
                    __result = false;
                    return;
                }
            }
        }
    }
}
