using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    public static class Loop {
        public static RimWorld.Planet.WorldCameraDriver camera_driver;
        public static Camera camera;
        public static TickManager tick_manager;
        public static int tick;
        public static Vector3 camera_position;
        public static Vector3 center;

        public static void init() {
            camera_driver = Find.WorldCameraDriver;
            camera = Find.WorldCamera.GetComponent<Camera>();
            camera.farClipPlane = 1600.0f;
            tick_manager = Find.TickManager;
        }

        public static void update() {
            if (!camera_driver.enabled) return;
            tick = tick_manager.TicksGame;
            camera_position = camera.transform.position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
            RimNauts_GameComponent.render_manager.update();
        }

        public static void render() {

        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.World), "FinalizeInit")]
    public class World_FinalizeInit {
        public static void Postfix() => Loop.init();
    }

    [HarmonyPatch(typeof(RimWorld.Planet.World), "WorldTick")]
    public class World_WorldTick {
        public static void Prefix() => Loop.update();
    }

    [HarmonyPatch(typeof(RimWorld.Planet.World), "WorldUpdate")]
    public class World_WorldUpdate {
        public static void Prefix() => Loop.render();
    }
}
