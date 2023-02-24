using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.World {
    /**
     * Keeps all the cache collections and their methods.
     */
    public class Caching_Handler : GameComponent {
        public static Dictionary<int, ObjectHolder> object_holders;
        public static int total_asteroid_ores;
        public static int total_moons;
        public static int total_satellites;
        public static int total_space_stations;

        public Caching_Handler(Game game) : base() {
            // initialize empty caches
            clear();
            Cache.caching_handler = this;
        }

        public ObjectHolder get(int tile) => object_holders[tile];

        public bool exists(int tile) => object_holders.ContainsKey(tile);

        public void add(int tile, ObjectHolder object_holder) {
            update_stats(object_holder.type, 1);
            remove(tile);
            object_holders.Add(tile, object_holder);
        }

        public void remove(int tile) {
            if (exists(tile)) update_stats(get(tile).type, -1);
            object_holders.Remove(tile);
        }

        public void update_stats(Type type, int update) {
            switch (type) {
                case Type.AsteroidOre:
                    total_asteroid_ores += update;
                    break;
                case Type.Moon:
                    total_moons += update;
                    break;
                case Type.Satellite:
                    total_satellites += update;
                    break;
                case Type.SpaceStation:
                    total_space_stations += update;
                    break;
            }
        }

        public void clear() {
            object_holders = new Dictionary<int, ObjectHolder>();
            total_asteroid_ores = 0;
            total_moons = 0;
            total_satellites = 0;
            total_space_stations = 0;
        }
    }

    /**
     * API for interacting with Caching_Handler class.
     */
    public static class Cache {
        public static Caching_Handler caching_handler;
        public static bool stop;

        public static ObjectHolder get(int tile) => caching_handler.get(tile);

        public static bool exists(int tile) => caching_handler.exists(tile);

        public static void add(int tile, ObjectHolder object_holder) => caching_handler.add(tile, object_holder);

        public static void add(ObjectHolder object_holder) => caching_handler.add(object_holder.Tile, object_holder);

        public static void remove(int tile) => caching_handler.remove(tile);

        public static void remove(ObjectHolder object_holder) => caching_handler.remove(object_holder.Tile);

        public static void clear() => caching_handler.clear();
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "AddToCache")]
    public static class WorldObjectsHolder_AddToCache {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is ObjectHolder object_holder && !Cache.exists(o.Tile)) Cache.add(object_holder);
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "RemoveFromCache")]
    public static class WorldObjectsHolder_RemoveFromCache {
        public static void Prefix(RimWorld.Planet.WorldObject o) {
            if (o is ObjectHolder object_holder) Cache.remove(object_holder);
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectsHolder), "Recache")]
    public static class WorldObjectsHolder_Recache {
        public static void Prefix() {
            if (!Cache.stop) Cache.clear();
        }
    }
}
