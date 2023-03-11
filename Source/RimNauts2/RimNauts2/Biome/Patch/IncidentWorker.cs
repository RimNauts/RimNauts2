using Verse;

namespace RimNauts2.Biome.Patch {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            if (parms.forced) return;
            try {
                if (!__result || !World.Cache.exists(parms.target.Tile)) return;
                parms.raidArrivalMode = RimWorld.PawnsArrivalModeDefOf.EdgeDrop;
                if (Defs.Of.general.allowed_incidents.Contains(__instance.def.defName)) return;
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

    [HarmonyLib.HarmonyPatch(typeof(RimWorld.IncidentWorker_Raid), "ResolveRaidArriveMode")]
    class IncidentWorker_Raid_ResolveRaidArriveMode {
        public static bool Prefix(RimWorld.IncidentParms parms) {
            if (!World.Cache.exists(parms.target.Tile)) return true;
            parms.raidArrivalMode = parms.raidArrivalMode = (Rand.Value < 0.6f) ? RimWorld.PawnsArrivalModeDefOf.EdgeDrop : RimWorld.PawnsArrivalModeDefOf.CenterDrop;
            return false;
        }
    }
}
