using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldRendererUtility), "HiddenBehindTerrainNow")]
    internal static class HideIcons {
        internal static void Postfix(Vector3 pos, ref bool __result) {
            Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            // ignore icons on surface (settlements)
            if (Vector3.Distance(pos, center) < 110) return;
            // hide NEOs when zooming in
            if ((int) Find.WorldCameraDriver.CurrentZoom < (int) RimWorld.Planet.WorldCameraZoomRange.VeryFar) {
                __result = true;
                return;
            }
            float alt_percent = Find.WorldCameraDriver.AltitudePercent;
            // reduce degree of visability dependent on zoom
            float degree = 165 + (15 * alt_percent);
            float min_alt = 125;
            float max_alt = 1100;
            float alt = alt_percent * (max_alt - min_alt) + max_alt;
            __result = Vector3.Angle(pos.normalized, center) > (Math.Acos(115 / alt) + Math.Acos(115 / pos.magnitude)) * (degree / 3.14);
            if (pos.magnitude < 115) __result = Vector3.Angle(pos.normalized, center) > 73f;
        }
    }
}
