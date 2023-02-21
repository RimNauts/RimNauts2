using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    public static class Info {
        public static readonly string name = "RimNauts 2";
        public static readonly string version = "3.6.5";
    }

    public static class Style {
        public static readonly string name_prefix = Info.name + ": ";
        public static readonly string tab = "        ";
    }

    [HarmonyPatch(typeof(Game), "UpdatePlay")]
    public class Game_UpdatePlay {
        public static RimWorld.Planet.WorldCameraDriver camera_driver;
        public static Camera camera;
        public static TickManager tick_manager;
        public static int tick;
        public static Vector3 camera_position;
        public static Vector3 center;

        public static void Prefix() {
            tick = tick_manager.TicksGame;
            camera_position = camera.transform.position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
        }
    }

    [HarmonyPatch(typeof(Game), "FinalizeInit")]
    public class Game_FinalizeInit {
        public static void Postfix() {
            Game_UpdatePlay.camera_driver = Find.WorldCameraDriver;
            Game_UpdatePlay.camera = Find.WorldCamera.GetComponent<Camera>();
            Game_UpdatePlay.camera.farClipPlane = 1600.0f;
            Game_UpdatePlay.tick_manager = Find.TickManager;
        }
    }
}
