using HarmonyLib;
using RimWorld;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(PawnsArrivalModeWorker), "CanUseWith")]
    public class PawnsArrivalModeWorker_CanUseWith_Patch {
        public static void Postfix(bool __result, IncidentParms parms) {
            if (__result && moon_biome(parms) && lower_tech_level(parms.raidArrivalMode, threshold: TechLevel.Spacer)) {
                __result = false;
            }
        }

        private static bool moon_biome(IncidentParms parms) {
            return Find.WorldGrid[parms.target.Tile].biome == MoonDefOf.RockMoonBiome;
        }

        private static bool lower_tech_level(PawnsArrivalModeDef raid, TechLevel threshold) {
            return raid.minTechLevel < threshold;
        }
    }
}
