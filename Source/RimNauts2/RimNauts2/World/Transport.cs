using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class TransportpodSatelliteIgnoreMaxRange {
        [HarmonyPostfix]
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool to_moon = Find.World.grid.tiles.ElementAt(start).biome == RimWorld.BiomeDef.Named("RockMoonBiome");
            bool from_moon = Find.World.grid.tiles.ElementAt(end).biome == RimWorld.BiomeDef.Named("RockMoonBiome");
            if (to_moon || from_moon) {
                __result = 1;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("Start", MethodType.Getter)]
    public static class TransportpodFromSatelliteAnimation {
        [HarmonyPostfix]
        public static void StartAtShip(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int num = (int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == num) __result = worldObject.DrawPos;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("End", MethodType.Getter)]
    public static class TransportpodToSatelliteAnimation {
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
