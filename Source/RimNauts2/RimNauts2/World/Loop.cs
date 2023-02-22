using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    public static class Loop {
        public static RimWorld.Planet.WorldCameraDriver camera_driver;
        public static Camera camera;
        public static TickManager tick_manager;
        public static int tick;
        private static int prev_tick = -1;
        public static Vector3 camera_position;
        private static Vector3 prev_camera_position = Vector3.zero;
        public static Vector3 center;
        public static bool unpaused;
        public static bool camera_moved;

        public static void init() {
            camera_driver = Find.WorldCameraDriver;
            camera = Find.WorldCamera.GetComponent<Camera>();
            camera.farClipPlane = 1600.0f;
            tick_manager = Find.TickManager;
            internal_update();
        }

        public static void update() {
            if (!RimWorld.Planet.WorldRendererUtility.WorldRenderedNow) return;
            internal_update();
            RimNauts_GameComponent.render_manager.recache_materials();
            RimNauts_GameComponent.render_manager.update();
            RimNauts_GameComponent.render_manager.draw();
        }

        private static void internal_update() {
            tick = tick_manager.TicksGame;
            unpaused = tick != prev_tick;
            prev_tick = tick;
            camera_position = camera.transform.position;
            camera_moved = camera_position != prev_camera_position;
            prev_camera_position = camera_position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.World), "FinalizeInit")]
    public class World_FinalizeInit {
        public static void Postfix() => Loop.init();
    }

    [HarmonyPatch(typeof(RimWorld.Planet.World), "WorldUpdate")]
    public class World_WorldUpdate {
        public static void Postfix() => Loop.update();
    }
}
