using HarmonyLib;
using System.Collections.Generic;
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

    [HarmonyPatch(typeof(RimWorld.PlaceWorker_NotUnderRoof), "AllowsPlacing")]
    class PlaceWorker_NotUnderRoof_AllowsPlacing {
        public static bool Prefix(ref AcceptanceReport __result, BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore, Thing thing) {
            if (map.roofGrid.RoofAt(loc) != Defs.Loader.roof_magnetic_field) return true;
            __result = (AcceptanceReport) true;
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.CompLaunchable), "AnyInGroupIsUnderRoof", MethodType.Getter)]
    class CompLaunchable_AnyInGroupIsUnderRoof {
        public static void Postfix(ref RimWorld.CompLaunchable __instance, ref bool __result) {
            if (!__result) return;
            List<RimWorld.CompTransporter> transportersInGroup = __instance.TransportersInGroup;
            for (int index = 0; index < transportersInGroup.Count; ++index) {
                if (transportersInGroup[index].parent.Position.Roofed(__instance.parent.Map) && transportersInGroup[index].parent.Position.GetRoof(__instance.parent.Map) != Defs.Loader.roof_magnetic_field) {
                    __result = true;
                    return;
                }
            }
            __result = false;
            return;
        }
    }

    [HarmonyPatch(typeof(RoofCollapserImmediate), "DropRoofInCellPhaseOne")]
    class RoofCollapserImmediate_DropRoofInCellPhaseOne {
        public static bool Prefix(IntVec3 c, Map map, List<Thing> outCrushedThings) {
            if (map.roofGrid.RoofAt(c) != Defs.Loader.roof_magnetic_field) return true;
            return false;
        }
    }

    [HarmonyPatch(typeof(RoofCollapserImmediate), "DropRoofInCellPhaseTwo")]
    class RoofCollapserImmediate_DropRoofInCellPhaseTwo {
        public static bool Prefix(IntVec3 c, Map map) {
            if (map.roofGrid.RoofAt(c) != Defs.Loader.roof_magnetic_field) return true;
            return false;
        }
    }
}
