using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGenStep_Terrain), "BiomeFrom")]
    internal static class SetSatelliteBiome {
        public static int i = 0;

        [HarmonyPostfix]
        internal static void Postfix(RimWorld.Planet.Tile ws, int tileID, ref RimWorld.BiomeDef __result) {
            if (i < Generate_Satellites.total_moon_amount && __result.defName == "Ocean") {
                __result = RimWorld.BiomeDef.Named("SatelliteBiome");
                i++;
            }
        }
    }
}
