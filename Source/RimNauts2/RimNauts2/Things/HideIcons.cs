using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectSelectionUtility), nameof(RimWorld.Planet.WorldObjectSelectionUtility.HiddenBehindTerrainNow))]
    internal static class WorldObjectSelectionUtility_HiddenBehindTerrainNow {
        internal static bool Prefix(RimWorld.Planet.WorldObject o, ref bool __result) {
            if (!(o is World.ObjectHolder)) return true;
            Vector3 pos = o.DrawPos;
            Vector3 normalized = pos.normalized;
            float mag = pos.magnitude;
            float min_alt = 125;
            float max_alt = 1100;
            float alt = Find.WorldCameraDriver.AltitudePercent * (max_alt - min_alt) + max_alt;
            Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            __result = Vector3.Angle(normalized, center) > (Math.Acos(115 / alt) + Math.Acos(115 / mag)) * (180 / 3.14d);
            if (mag < 115) __result = Vector3.Angle(normalized, center) > 73.0f;
            return false;
        }
    }
}
