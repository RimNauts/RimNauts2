using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldRendererUtility), nameof(RimWorld.Planet.WorldRendererUtility.HiddenBehindTerrainNow))]
    internal static class HideIcons {
        [HarmonyPostfix]
        internal static void Postfix(Vector3 pos, ref bool __result) {
            Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            // ignore icons on surface (settlements)
            if (Vector3.Distance(pos, center) < 110) return;
            float alt_percent = Find.WorldCameraDriver.AltitudePercent;
            // ignore satellites when zoomed in
            if (alt_percent < 0.22) {
                __result = true;
                return;
            }
            // reduce degree of visability dependent on zoom
            float degree = 165 + ((15 * alt_percent) * 1.5f);
            if (alt_percent > 0.30 && alt_percent < 0.35) degree = 173;
            if (alt_percent > 0.35 && alt_percent < 0.37) degree = 174;
            if (alt_percent > 0.37 && alt_percent < 0.46) degree = 175;
            if (alt_percent > 0.46 && alt_percent < 0.49) degree = 176;
            if (alt_percent > 0.49 && alt_percent < 0.53) degree = 177;
            if (alt_percent > 0.53 && alt_percent < 0.63) degree = 178;
            if (alt_percent > 0.63 && alt_percent < 0.87) degree = 179;
            if (alt_percent > 0.87 && alt_percent < 0.93) degree = 179.5f;
            if (degree < 160 || alt_percent < 0.24) degree = 160;
            if (degree > 180) degree = 180;
            float min_alt = 125;
            float max_alt = 1100;
            float alt = alt_percent * (max_alt - min_alt) + max_alt;
            __result = Vector3.Angle(pos.normalized, center) > (Math.Acos(115 / alt) + Math.Acos(115 / pos.magnitude)) * (degree / 3.14);
            if (pos.magnitude < 115) __result = Vector3.Angle(pos.normalized, center) > 73f;
        }
    }
}
