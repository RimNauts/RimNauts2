using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("Start", MethodType.Getter)]
    public static class OutgoingTransportpod {
        [HarmonyPostfix]
        public static void StartAtShip(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int num = (int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                where o is Satellite
                                                select o) {
                if (worldObject.Tile == num) {
                    __result = worldObject.DrawPos;
                }
            }

        }
    }
}
