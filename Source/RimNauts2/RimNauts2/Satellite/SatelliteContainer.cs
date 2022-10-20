using System.Collections.Generic;

namespace RimNauts2 {
    public static class SatelliteContainer {
        public static Dictionary<int, Satellite> satellites = new Dictionary<int, Satellite>();

        public static void add(Satellite satellite) {
            satellites.Add(satellite.Tile, satellite);
        }

        public static void remove(Satellite satellite) {
            satellites.Remove(satellite.Tile);
        }

        public static void remove(int tile_id) {
            satellites.Remove(tile_id);
        }

        public static bool exists(int tile) {
            return satellites.ContainsKey(tile);
        }

        public static void clear() {
            satellites.Clear();
        }
    }
}
