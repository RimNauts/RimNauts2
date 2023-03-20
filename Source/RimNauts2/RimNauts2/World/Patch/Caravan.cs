using HarmonyLib;
using UnityEngine;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.CaravanTweenerUtility), nameof(RimWorld.Planet.CaravanTweenerUtility.PatherTweenedPosRoot))]
    public static class CaravanTweenerUtility_PatherTweenedPosRoot {
        public static bool Prefix(RimWorld.Planet.Caravan caravan, ref Vector3 __result) {
            if (Cache.exists(caravan.Tile)) {
                __result = Cache.get(caravan.Tile).DrawPos;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), "GetTileCenter")]
    class WorldGrid_GetTileCenter {
        public static bool Prefix(ref Vector3 __result, int tileID) {
            if (!Cache.exists(tileID)) return true;
            __result = Cache.get(tileID).position;
            return false;
        }
    }
}
