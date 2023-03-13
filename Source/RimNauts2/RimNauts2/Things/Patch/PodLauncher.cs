using System;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace RimNauts2.Things.Patch {
    [HarmonyPatch(typeof(RimWorld.FuelingPortUtility), "GetFuelingPortCell", new Type[] { typeof(Building) })]
    class FuelingPortUtility_GetFuelingPortCell {
        public static bool Prefix(Building podLauncher, ref IntVec3 __result) {
            if (podLauncher.def.defName != "RimNauts2_PodLauncher") return true;
            __result = podLauncher.Position;
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.PlaceWorker_FuelingPort), "DrawGhost")]
    class PlaceWorker_FuelingPort_DrawGhost {
        public static bool Prefix(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null) {
            if (def.defName != "RimNauts2_PodLauncher") return true;
            if (def.building == null || !def.building.hasFuelingPort) return false;
            center.x++;
            if (!RimWorld.FuelingPortUtility.GetFuelingPortCell(center, Rot4.South).Standable(Find.CurrentMap)) return false;
            RimWorld.PlaceWorker_FuelingPort.DrawFuelingPortCell(center, Rot4.South);
            return false;
        }
    }
}
