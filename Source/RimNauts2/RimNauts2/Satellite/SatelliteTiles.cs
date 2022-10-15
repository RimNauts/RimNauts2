using HarmonyLib;
using UnityEngine;

namespace RimNauts2 {
    /// <summary>
    /// Covers all bases for grabbing Tile's rendering location due to DrawPos of Satellite being overridden and moved to orbit
    /// </summary>
    /// <remarks>
    /// Concerned about compatibility and "applying" this to every single tile / world object? You can do a postfix instead, but you'll take a
    /// performance hit due to looping through all tile vertices as opposed to only performing the bucket lookup operation
    /// </remarks>
    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.GetTileCenter))]
    public static class SatelliteTiles {
        [HarmonyPrefix]
        public static bool Prefix(int tileID, ref Vector3 __result) {
            if (Satellites.cachedWorldObjectTiles.TryGetValue(tileID, out var satellite)) {
                __result = satellite.DrawPos;
                return false;
            }
            return true;
        }
    }

    public static class SatelliteTiles_Utilities {
        public static void add_satellite(Satellite satellite) {
            Satellites.cachedWorldObjectTiles[satellite.Tile].PostRemove();
            Satellites.cachedWorldObjectTiles.Remove(satellite.Tile);
            Satellites.cachedWorldObjectTiles.Add(satellite.Tile, satellite);
        }

        public static void remove_satellite(Satellite satellite) {
            Satellites.cachedWorldObjectTiles.Remove(satellite.Tile);
        }
    }

    /// <summary>
    /// Register Satellite WorldObject in cache post AddToCache
    /// </summary>
    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "AddToCache")]
    public static class WorldObjectRegister {
        [HarmonyPrefix]
        public static void Postfix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite satellite && !Satellites.cachedWorldObjectTiles.ContainsKey(o.Tile)) {
                Satellites.cachedWorldObjectTiles.Add(o.Tile, satellite);
            }
        }
    }

    /// <summary>
    /// Remove Satellite WorldObject from cache post RemoveFromCache
    /// </summary>
    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "RemoveFromCache")]
    public static class WorldObjectDeregister {
        [HarmonyPrefix]
        public static void Postfix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite) {
                Satellites.cachedWorldObjectTiles.Remove(o.Tile);
            }
        }
    }
}
