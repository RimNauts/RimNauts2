using UnityEngine;
using HarmonyLib;

namespace RimNauts2 {
    /*[HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.GetTileCenter))]
    public class RimNauts2_WorldGrid_GetTileCenter {
        public static bool Prefix(int tileID, ref Vector3 __result) {
            if (RimNauts_GameComponent.satellites.TryGetValue(tileID, out Satellite satellite)) {
                __result = satellite.DrawPos;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.PlanetShapeGenerator), "DoGenerate")]
    public class RimNauts2_PlanetShapeGenerator_DoGenerate {
        public static bool Prefix() {
            SatelliteContainer.reset();
            return true;
        }
    }*/
}
