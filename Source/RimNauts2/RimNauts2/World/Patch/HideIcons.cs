using System;
using HarmonyLib;
using UnityEngine;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldRendererUtility), "HiddenBehindTerrainNow")]
    internal static class WorldRendererUtility_HiddenBehindTerrainNow {
        internal static bool Prefix(Vector3 pos, ref bool __result) {
            // ignore icons on surface (settlements)
            if (Vector3.Distance(pos, Loop.center) < 110) return true;
            // ignore icons when zoomed in
            if (Loop.altitude_percent < 0.15) {
                __result = true;
                return false;
            }
            Vector3 normalized = pos.normalized;
            float mag = pos.magnitude;
            float min_alt = 125;
            float max_alt = 1100;
            float alt = Loop.altitude_percent * (max_alt - min_alt) + max_alt;
            __result = Vector3.Angle(normalized, Loop.center) > (Math.Acos(115 / alt) + Math.Acos(115 / mag)) * (180 / 3.14d);
            if (mag < 115) __result = Vector3.Angle(normalized, Loop.center) > 73.0f;
            return false;
        }
    }
}
