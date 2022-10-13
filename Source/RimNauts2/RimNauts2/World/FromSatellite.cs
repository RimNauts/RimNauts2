using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("Start", MethodType.Getter)]
    public static class FromSatellite {
        [HarmonyPostfix]
        public static void StartAtShip(TravelingTransportPods __instance, ref Vector3 __result) {
            int num = (int) typeof(TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                where o is WorldObjectChild_Satellite
                                                select o) {
                if (worldObject.Tile == num) {
                    __result = worldObject.DrawPos;
                }
            }

        }
    }
}
