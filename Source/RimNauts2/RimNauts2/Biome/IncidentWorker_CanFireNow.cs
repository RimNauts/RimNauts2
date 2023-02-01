using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    public class IncidentWorker_CanFireNow {
        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            if (parms.forced) return;
            try {
                if (!__result) return;
                bool incident_not_on_satellite = !Find.WorldGrid[parms.target.Tile].biome.defName.Contains("RimNauts2");
                if (incident_not_on_satellite) return;
                if (SatelliteDefOf.Satellite.AllowedSatelliteIncidents.Contains(__instance.def.defName)) return;
                __result = false;
            } catch { }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PawnsArrivalModeWorker), nameof(RimWorld.PawnsArrivalModeWorker.CanUseWith))]
    public class PawnsArrivalModeWorker_CanUseWith_Patch {
        public static void Postfix(RimWorld.IncidentParms parms, ref RimWorld.PawnsArrivalModeWorker __instance, bool __result) {
            if (__result && Find.WorldGrid[parms.target.Tile].biome.defName.Contains("RimNauts2")) {
                __instance.def.minTechLevel = RimWorld.TechLevel.Industrial;
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PawnGroupMakerUtility), nameof(RimWorld.PawnGroupMakerUtility.GeneratePawns))]
    public class PawnGroupMakerUtility_GeneratePawns {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result, RimWorld.PawnGroupMakerParms parms, bool warnOnZeroResults) {
            bool no_oxygen = Universum.Utilities.Cache.allowed_utility(Find.WorldGrid[parms.tile].biome, "universum.vacuum_suffocation");
            bool decompression = Universum.Utilities.Cache.allowed_utility(Find.WorldGrid[parms.tile].biome, "universum.vacuum_decompression");
            bool requires_spacesuit = no_oxygen || decompression;
            foreach (Pawn pawn in __result) {
                if (requires_spacesuit) {
                    RimWorld.Apparel space_helmet = (RimWorld.Apparel) ThingMaker.MakeThing(ThingDef.Named("RimNauts2_Apparel_SpaceSuit_Head"));
                    RimWorld.Apparel space_suit = (RimWorld.Apparel) ThingMaker.MakeThing(ThingDef.Named("RimNauts2_Apparel_SpaceSuit_Body"));
                    pawn.apparel.Wear(space_helmet, false);
                    pawn.apparel.Wear(space_suit, false);
                }
                yield return pawn;
            }
        }
    }
}
