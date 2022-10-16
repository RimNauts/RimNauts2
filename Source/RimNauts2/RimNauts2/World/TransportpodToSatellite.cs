using System.Linq;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("End", MethodType.Getter)]
    public static class TransportpodToSatellite {
        [HarmonyPostfix]
        public static void EndAtShip(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {

            int destinationTile = __instance.destinationTile;
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == destinationTile) __result = worldObject.DrawPos;
            }
        }
    }
}
