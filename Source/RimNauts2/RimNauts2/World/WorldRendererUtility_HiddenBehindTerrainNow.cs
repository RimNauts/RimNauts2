using System;
using HarmonyLib;
using RimWorld.Planet;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(WorldRendererUtility), "HiddenBehindTerrainNow")]
    internal static class WorldRendererUtility_HiddenBehindTerrainNow {
        internal static void Postfix(Vector3 pos, ref bool __result) {
            Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            float distance = Vector3.Distance(pos, currentlyLookingAtPointOnSphere);
            // ignore icons on surface
            if (distance < 110) return;
            // hide icons when zooming in
            if ((int) Find.WorldCameraDriver.CurrentZoom != (int) WorldCameraZoomRange.VeryFar) {
                __result = true;
                return;
            }
            float altper = Find.WorldCameraDriver.AltitudePercent;
            float degree = 165 + (15 * altper);
            float minAlt = 125;
            float maxAlt = 1100;
            float alt = altper * (maxAlt - minAlt) + maxAlt;
            Vector3 normalized = pos.normalized;
            __result = Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > (Math.Acos(115 / alt) + Math.Acos(115 / pos.magnitude)) * (degree / 3.14);
            if (pos.magnitude < 115) __result = Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f;
        }
    }
}
