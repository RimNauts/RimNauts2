using System.Collections.Generic;

namespace RimNauts2.Biome.Patch {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    class IncidentWorker_CanFireNow {
        public static void Prefix(ref RimWorld.IncidentWorker __instance) {
            try {
                if (__instance.def.allowedBiomes != null) return;
                if (Defs.Of.general.allowed_incidents.Contains(__instance.def.defName)) return;
                if (__instance.def.disallowedBiomes == null) __instance.def.disallowedBiomes = new List<RimWorld.BiomeDef>();
                __instance.def.disallowedBiomes.Add(RimWorld.BiomeDef.Named("RimNauts2_Satellite_Biome"));
                __instance.def.disallowedBiomes.Add(RimWorld.BiomeDef.Named("RimNauts2_MoonWater_Biome"));
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
