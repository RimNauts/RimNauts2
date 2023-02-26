using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2.Biome.Patch {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PawnGroupMakerUtility), "GeneratePawns")]
    class PawnGroupMakerUtility_GeneratePawns {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result, RimWorld.PawnGroupMakerParms parms, bool warnOnZeroResults) {
            if (__result == null || !__result.Any()) yield break;
            bool no_oxygen = Universum.Utilities.Cache.allowed_utility(Find.WorldGrid[parms.tile].biome, "universum.vacuum_suffocation");
            bool decompression = Universum.Utilities.Cache.allowed_utility(Find.WorldGrid[parms.tile].biome, "universum.vacuum_decompression");
            bool requires_spacesuit = no_oxygen || decompression;
            foreach (Pawn pawn in __result) {
                if (requires_spacesuit && pawn != null) {
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
