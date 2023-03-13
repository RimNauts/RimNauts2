using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Patch {
    [HarmonyPatch(typeof(RoofGrid), "GetCellExtraColor")]
    public static class RoofGrid_GetCellExtraColor {
        public static bool Prefix(ref RoofGrid __instance, ref Color __result, int index) {
            if (__instance.roofGrid[index] != Defs.Loader.roof_magnetic_field) return true;
            __result = Color.blue;
            return false;
        }
    }
}
