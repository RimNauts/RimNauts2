using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2.Game {
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
            tick = tick_manager.TicksGame;
            camera_position = camera.transform.position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
        }
    }

    [HarmonyPatch(typeof(Verse.Game), "FinalizeInit")]
    public class Game_FinalizeInit {
        public static void Postfix() => Loop.init();
    }

    [HarmonyPatch(typeof(Verse.Game), "UpdatePlay")]
    public class Game_UpdatePlay {
        public static void Prefix() => Loop.update();
    }
}
