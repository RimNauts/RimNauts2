using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    public class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, ref bool __result) {
            if (!__result) return;
            incident = __instance.def.defName;
        }

        public static void Postfix(RimWorld.IncidentParms parms, ref bool __result) {
            if (!__result) return;
            bool incident_not_on_moon_biome = Find.WorldGrid[parms.target.Tile].biome != MoonDefOf.RockMoonBiome;
            if (incident_not_on_moon_biome) return;
            if (allowed_incidents.Contains(incident)) return;
            __result = false;
        }

        private static string incident = "";

        private static readonly List<string> allowed_incidents = new List<string>() {
            "Disease_AnimalFlu",
            "Disease_AnimalPlague",
            "Disease_FibrousMechanites",
            "Disease_Flu",
            "Disease_Plague",
            "Disease_SensoryMechanites",
            "Disease_SleepingSickness",
            "MeteoriteImpact",
            "OrbitalTraderArrival",
            "PsychicDrone",
            "PsychicSoothe",
            "RefugeePodCrash",
            "ResourcePodCrash",
            "ShipChunkDrop",
        };
    }
}
