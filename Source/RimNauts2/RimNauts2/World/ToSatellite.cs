using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("End", MethodType.Getter)]
    public static class ToSatellite {
        [HarmonyPostfix]
        public static void EndAtShip(TravelingTransportPods __instance, ref Vector3 __result) {

            int destinationTile = __instance.destinationTile;
            foreach (WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                where o is Satellite
                                                select o) {
                if (worldObject.Tile == destinationTile) {
                    __result = worldObject.DrawPos;
                }
            }

        }
    }
}
