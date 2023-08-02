using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods), "TraveledPctStepPerTick", MethodType.Getter)]
    public static class TravelingTransportPods_TraveledPctStepPerTick {
        public static void Postfix(RimWorld.Planet.TravelingTransportPods __instance, ref float __result) {
            if (!Cache.exists(__instance.destinationTile)) return;
            Vector3 start = __instance.Start;
            Vector3 end = __instance.End;
            if (start == end) {
                __result = 1f;
                return;
            }
            Vector3 current_position = __instance.DrawPos;
            float normalized_distance_from_planet = 1 - ((Vector3.Distance(current_position, RenderingManager.center) - 50.0f) / (500.0f - 50.0f));
            float speed = normalized_distance_from_planet * 0.001f;
            __result = speed;
            return;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class WorldGrid_TraversalDistanceBetween {
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool from_orbit = Cache.exists(start);
            if (from_orbit) {
                __result = 20;
                return;
            }
            bool to_orbit = Cache.exists(end);
            if (to_orbit) __result = 100;
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
