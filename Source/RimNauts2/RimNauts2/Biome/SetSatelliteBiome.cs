using HarmonyLib;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGenStep_Terrain), "BiomeFrom")]
    internal static class SetSatelliteBiome {
        public static int i = 0;
        internal static void Postfix(RimWorld.Planet.Tile ws, int tileID, ref RimWorld.BiomeDef __result) {
            if (i < Generate_Satellites.total_satellite_amount && __result == RimWorld.BiomeDefOf.Ocean) {
                __result = BiomeDefOf.SatelliteBiome;
                i++;
            }
        }
    }
}
