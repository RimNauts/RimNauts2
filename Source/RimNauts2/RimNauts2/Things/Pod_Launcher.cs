using System;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyLib.HarmonyPatch(typeof(RimWorld.FuelingPortUtility), "GetFuelingPortCell", new Type[] { typeof(Building) })]
    public static class FuelingPortUtility_GetFuelingPortCell {
        public static bool Prefix(Building podLauncher, ref IntVec3 __result) {
            if (podLauncher.def.defName != "RimNauts2_PodLauncher") return true;
            __result = podLauncher.Position;
            return false;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(RimWorld.PlaceWorker_FuelingPort), "DrawGhost")]
    public static class PlaceWorker_FuelingPort_DrawGhost {
        public static bool Prefix(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null) {
            if (def.defName != "RimNauts2_PodLauncher") return true;
            Map currentMap = Find.CurrentMap;
            rot = Rot4.South;
            center.x++;
            if (def.building == null || !def.building.hasFuelingPort || !RimWorld.FuelingPortUtility.GetFuelingPortCell(center, rot).Standable(currentMap)) return false;
            RimWorld.PlaceWorker_FuelingPort.DrawFuelingPortCell(center, rot);
            return false;
        }
    }
}
