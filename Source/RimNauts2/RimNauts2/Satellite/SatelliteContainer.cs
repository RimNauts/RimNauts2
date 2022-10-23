using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace RimNauts2 {
    public class RimNauts_GameComponent : GameComponent {
        public static readonly Dictionary<int, Satellite> satellites = new Dictionary<int, Satellite>();
        public static int total_asteroids;
        public static int total_moons;
        public static int total_artifical_satellites;

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

        public static bool exists(int tile_id) {
            return RimNauts_GameComponent.satellites.ContainsKey(tile_id);
        }

        public static void clear() {
            RimNauts_GameComponent.satellites.Clear();
        }

        public static int size() {
            return RimNauts_GameComponent.satellites.Count;
        }

        public static int size(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return RimNauts_GameComponent.total_asteroids;
                case Satellite_Type.Moon:
                    return RimNauts_GameComponent.total_moons;
                case Satellite_Type.Artifical_Satellite:
                    return RimNauts_GameComponent.total_artifical_satellites;
                default:
                    return -1;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "AddToCache")]
    public static class WorldObjectRegister {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite satellite && !SatelliteContainer.exists(o.Tile)) {
                SatelliteContainer.add(satellite);
                switch (satellite.type) {
                    case Satellite_Type.Asteroid:
                        RimNauts_GameComponent.total_asteroids++;
                        return;
                    case Satellite_Type.Moon:
                        RimNauts_GameComponent.total_moons++;
                        return;
                    case Satellite_Type.Artifical_Satellite:
                        RimNauts_GameComponent.total_artifical_satellites++;
                        return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "RemoveFromCache")]
    public static class WorldObjectDeregister {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is Satellite satellite) {
                SatelliteContainer.remove(satellite);
                switch (satellite.type) {
                    case Satellite_Type.Asteroid:
                        RimNauts_GameComponent.total_asteroids--;
                        return;
                    case Satellite_Type.Moon:
                        RimNauts_GameComponent.total_moons--;
                        return;
                    case Satellite_Type.Artifical_Satellite:
                        RimNauts_GameComponent.total_artifical_satellites--;
                        return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "Recache")]
    public static class WorldObjectRecache {
        public static void Prefix() {
            SatelliteContainer.clear();
            RimNauts_GameComponent.total_asteroids = 0;
            RimNauts_GameComponent.total_moons = 0;
            RimNauts_GameComponent.total_artifical_satellites = 0;
        }
    }
}
