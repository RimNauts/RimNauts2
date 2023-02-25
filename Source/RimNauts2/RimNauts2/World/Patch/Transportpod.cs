using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class WorldGrid_TraversalDistanceBetween {
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool to_orbit = Cache.exists(end);
            bool from_orbit = Cache.exists(start);
            if (to_orbit || from_orbit) __result = 100;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods), "Start", MethodType.Getter)]
    public static class TravelingTransportPods_Start {
        public static bool Prefix(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int initialTile = (int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is ObjectHolder
                                                                select o) {
                if (worldObject.Tile == initialTile) {
                    __result = worldObject.DrawPos;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods), "End", MethodType.Getter)]
    public static class TravelingTransportPods_End {
        public static bool Prefix(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is ObjectHolder
                                                                select o) {
                if (worldObject.Tile == __instance.destinationTile) {
                    __result = worldObject.DrawPos;
                    return false;
                }
            }
            return true;
        }
    }
}
