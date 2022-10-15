using HarmonyLib;
using UnityEngine;

namespace RimNauts2 {
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

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "AddToCache")]
    public static class WorldObjectRegister {
        [HarmonyPrefix]
        public static void Postfix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite satellite && !Satellites.cachedWorldObjectTiles.ContainsKey(o.Tile)) {
                Satellites.cachedWorldObjectTiles.Add(o.Tile, satellite);
            }
        }
    }

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
