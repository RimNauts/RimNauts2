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
}
