using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace RimNauts2 {
    public class RimNauts_GameComponent : GameComponent {
        public static readonly Dictionary<int, Satellite> satellites = new Dictionary<int, Satellite>();

        public RimNauts_GameComponent(Game game) : base() { }
    }

    public static class SatelliteContainer {
        public static void add(Satellite satellite) {
            remove(satellite);
            RimNauts_GameComponent.satellites.Add(satellite.Tile, satellite);
        }

        public static void remove(Satellite satellite) {
            RimNauts_GameComponent.satellites.Remove(satellite.Tile);
        }

        public static void remove(int tile_id) {
            RimNauts_GameComponent.satellites.Remove(tile_id);
        }

        public static bool exists(int tile_id) {
            return RimNauts_GameComponent.satellites.ContainsKey(tile_id);
        }

        public static void clear() {
            RimNauts_GameComponent.satellites.Clear();
        }

        public static int size() {
            return RimNauts_GameComponent.satellites.Count;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "AddToCache")]
    public static class WorldObjectRegister {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite satellite && !SatelliteContainer.exists(o.Tile)) {
                SatelliteContainer.add(satellite);
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "RemoveFromCache")]
    public static class WorldObjectDeregister {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite) {
                SatelliteContainer.remove(o.Tile);
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "Recache")]
    public static class WorldObjectRecache {
        public static void Prefix() {
            SatelliteContainer.clear();
        }
    }
}
