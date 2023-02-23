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
        public static float altitude_percent;
        public static bool unpaused;
        public static bool camera_moved;
        private static bool wait;

        public static void init() {
            camera_driver = Find.WorldCameraDriver;
            camera = Find.WorldCamera.GetComponent<Camera>();
            camera.farClipPlane = 1600.0f;
            tick_manager = Find.TickManager;
            if (Caching_Handler.render_manager == null) Generator.add_render_manager();
            internal_update();
        }

        public static void update() {
            internal_update();
            if (wait) return;
            Caching_Handler.render_manager.recache_materials();
            Caching_Handler.render_manager.update();
            Caching_Handler.render_manager.draw();
            Patch.HideIcons.check_object_holders();
        }

        private static void internal_update() {
            wait = Caching_Handler.render_manager == null;
            wait = wait || (!RimWorld.Planet.WorldRendererUtility.WorldRenderedNow && !Caching_Handler.render_manager.draw_now);
            tick = tick_manager.TicksGame;
            unpaused = tick != prev_tick;
            prev_tick = tick;
            camera_position = camera.transform.position;
            camera_moved = camera_position != prev_camera_position;
            prev_camera_position = camera_position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
            altitude_percent = camera_driver.AltitudePercent;
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
