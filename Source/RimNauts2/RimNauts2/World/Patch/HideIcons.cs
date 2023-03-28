using System;
using HarmonyLib;
using UnityEngine;

namespace RimNauts2.World.Patch {
    public static class HideIcons {
        public static bool check_object_holder(Vector3 pos) {
            // ignore icons when zoomed in
            if ((Vector3.Distance(pos, RenderingManager.center) + 2.3f) > Vector3.Distance(RenderingManager.center, RenderingManager.camera_position)) {
                return true;
            } else return check(pos);
        }

        public static bool check(Vector3 pos) {
            // reduce degree of visability dependent on zoom (aka 'conditional statements of doom')
            float degree = 165 + (15 * RenderingManager.altitude_percent * 1.5f);
            if (degree > 180) {
                degree = 180;
            } else if (degree < 160 || RenderingManager.altitude_percent < 0.24) {
                degree = 160;
            } else if (RenderingManager.altitude_percent > 0.87 && RenderingManager.altitude_percent < 0.93) {
                degree = 179.5f;
            } else if (RenderingManager.altitude_percent > 0.63 && RenderingManager.altitude_percent < 0.87) {
                degree = 179;
            } else if (RenderingManager.altitude_percent > 0.53 && RenderingManager.altitude_percent < 0.63) {
                degree = 178;
            } else if (RenderingManager.altitude_percent > 0.49 && RenderingManager.altitude_percent < 0.53) {
                degree = 177;
            } else if (RenderingManager.altitude_percent > 0.46 && RenderingManager.altitude_percent < 0.49) {
                degree = 176;
            } else if (RenderingManager.altitude_percent > 0.37 && RenderingManager.altitude_percent < 0.46) {
                degree = 175;
            } else if (RenderingManager.altitude_percent > 0.35 && RenderingManager.altitude_percent < 0.37) {
                degree = 174;
            } else if (RenderingManager.altitude_percent > 0.30 && RenderingManager.altitude_percent < 0.35) {
                degree = 173;
            }
            Vector3 normalized = pos.normalized;
            float mag = pos.magnitude;
            float min_alt = RimWorld.Planet.WorldCameraDriver.MinAltitude;
            float max_alt = Defs.Of.general.max_altitude;
            float alt = RenderingManager.altitude_percent * (max_alt - min_alt) + max_alt;
            bool hide = Vector3.Angle(normalized, RenderingManager.center) > (Math.Acos(115 / alt) + Math.Acos(115 / mag)) * (degree / 3.14d);
            if (mag < 115) hide = Vector3.Angle(normalized, RenderingManager.center) > 73.0f;
            return hide;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldRendererUtility), "HiddenBehindTerrainNow")]
    static class WorldRendererUtility_HiddenBehindTerrainNow {
        public static bool Prefix(Vector3 pos, ref bool __result) {
            // ignore icons on surface (settlements)
            if (Vector3.Distance(pos, RenderingManager.center) < 110) return true;
            __result = HideIcons.check(pos);
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldObjectSelectionUtility), "HiddenBehindTerrainNow")]
    static class WorldObjectSelectionUtility_HiddenBehindTerrainNow {
        public static bool Prefix(RimWorld.Planet.WorldObject o, ref bool __result) {
            if (!(o is ObjectHolder)) return true;
            __result = ((ObjectHolder) o).hide_now;
            return false;
        }
    }
}
