using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RimNauts2 {
    public static class SatelliteContainer {
        public static Dictionary<int, Satellite> satellites = new Dictionary<int, Satellite>();

        public static void add(Satellite satellite) {
            satellites.Add(satellite.Tile, satellite);
        }

        public static void remove(Satellite satellite) {
            satellites.Remove(satellite.Tile);
        }

        public static bool exists(int tile) {
            return satellites.ContainsKey(tile);
        }

        public static void clear() {
            satellites.Clear();
        }

        public static int total() {
            return satellites.Count;
        }
    }
}
