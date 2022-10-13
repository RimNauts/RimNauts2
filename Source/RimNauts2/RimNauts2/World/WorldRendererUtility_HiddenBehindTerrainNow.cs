using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld.Planet;
using RimWorld;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(WorldRendererUtility), "HiddenBehindTerrainNow")]
    internal static class WorldRendererUtility_HiddenBehindTerrainNow {

        internal static void Postfix(Vector3 pos, ref bool __result) {
            Vector3 normalized = pos.normalized;
            Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
            float mag = pos.magnitude;
            float altper = Find.WorldCameraDriver.AltitudePercent;
            float minAlt = 125;
            float maxAlt = 1100;
            float alt = altper * (maxAlt - minAlt) + maxAlt;
            __result = Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > (Math.Acos(115 / alt) + Math.Acos(115 / mag)) * (180 / 3.14);
            if (mag < 115) __result = Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f;

        }
    }
}
