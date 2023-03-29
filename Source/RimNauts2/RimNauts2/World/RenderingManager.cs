﻿using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;

namespace RimNauts2.World {
    [StaticConstructorOnStartup]
    class RenderingManager : GameComponent {
        public static TickManager tick_manager;
        public static RimWorld.Planet.WorldCameraDriver camera_driver;
        public static Camera camera;

        public static int tick;
        private static int prev_tick;
        public static int spawn_ore_tick;

        public static Vector3 camera_position;
        private static Vector3 prev_camera_position = Vector3.zero;

        public static Vector3 center;
        public static float altitude_percent;

        public static bool unpaused;
        public static bool camera_moved;
        public static bool frame_changed;
        private static bool wait;
        public static bool force_update;
        private static bool world_map_switch;
        private static bool reset_trail;

        public static Matrix4x4[] cached_matrices;
        public static Material[] cached_materials;
        public static FeatureMesh[] cached_features;
        public static Trail[] cached_trails;

        public static bool dirty_features;

        public static int total_objects;

        public static List<Objects.NEO> visual_objects;

        private static List<Type> expose_type;
        private static List<string> expose_texture_path;
        private static List<Vector3> expose_orbit_position;
        private static List<float> expose_orbit_speed;
        private static List<Vector3> expose_draw_size;
        private static List<int> expose_period;
        private static List<int> expose_time_offset;
        private static List<int> expose_orbit_direction;
        private static List<float> expose_color;
        private static List<float> expose_rotation_angle;
        private static List<float> expose_transformation_rotation_angle;
        private static List<Vector3> expose_current_position;

        public RenderingManager(Game game) : base() => reset_instance();

        public static void reset_instance() {
            tick_manager = null;
            camera_driver = null;
            camera = null;
            tick = 0;
            prev_tick = -1;
            spawn_ore_tick = get_ore_timer();
            camera_position = Vector3.one;
            prev_camera_position = Vector3.zero;
            center = Vector3.zero;
            altitude_percent = 0.0f;
            unpaused = false;
            camera_moved = false;
            frame_changed = false;
            wait = false;
            force_update = true;
            world_map_switch = false;
            reset_trail = false;
            cached_matrices = new Matrix4x4[0];
            cached_materials = new Material[0];
            cached_features = new FeatureMesh[0];
            cached_trails = new Trail[0];
            dirty_features = true;
            total_objects = 0;
            visual_objects = new List<Objects.NEO>();
            // for file exposing
            expose_type = new List<Type>();
            expose_texture_path = new List<string>();
            expose_orbit_position = new List<Vector3>();
            expose_orbit_speed = new List<float>();
            expose_draw_size = new List<Vector3>();
            expose_period = new List<int>();
            expose_time_offset = new List<int>();
            expose_orbit_direction = new List<int>();
            expose_color = new List<float>();
            expose_rotation_angle = new List<float>();
            expose_transformation_rotation_angle = new List<float>();
            expose_current_position = new List<Vector3>();
        }

        public override void FinalizeInit() {
            init();
            get_frame_data();
        }

        public override void LoadedGame() {
            if (expose_type.NullOrEmpty()) {
                Generator.generate_fresh();
                return;
            }
            if (!cached_trails.NullOrEmpty()) for (int i = 0; i < cached_trails.Length; i++) cached_trails[i]?.set_active(false);
            foreach (var (_, object_holder) in Caching_Handler.object_holders) object_holder.visual_object = null;
            visual_objects = new List<Objects.NEO>();
            total_objects = expose_type.Count;
            for (int i = 0; i < total_objects; i++) {
                float? transformation_rotation_angle = null;
                if (!expose_transformation_rotation_angle.NullOrEmpty()) transformation_rotation_angle = expose_transformation_rotation_angle[i];
                Objects.NEO neo = expose_type[i].neo(
                    expose_texture_path[i],
                    expose_orbit_position[i],
                    expose_orbit_speed[i],
                    expose_draw_size[i],
                    expose_period[i],
                    expose_time_offset[i],
                    expose_orbit_direction[i],
                    expose_color[i],
                    expose_rotation_angle[i],
                    transformation_rotation_angle,
                    expose_current_position[i]
                );
                visual_objects.Add(neo);
            }
            recache();
            foreach (var (_, object_holder) in Caching_Handler.object_holders) object_holder.ExposeData();
        }

        public override void ExposeData() {
            // ran for each save and load
            expose_type = new List<Type>();
            expose_texture_path = new List<string>();
            expose_orbit_position = new List<Vector3>();
            expose_orbit_speed = new List<float>();
            expose_draw_size = new List<Vector3>();
            expose_period = new List<int>();
            expose_time_offset = new List<int>();
            expose_orbit_direction = new List<int>();
            expose_color = new List<float>();
            expose_rotation_angle = new List<float>();
            expose_transformation_rotation_angle = new List<float>();
            expose_current_position = new List<Vector3>();
            for (int i = 0; i < total_objects; i++) {
                if (visual_objects[i].object_holder != null) continue;
                expose_type.Add(visual_objects[i].type);
                expose_texture_path.Add(visual_objects[i].texture_path);
                expose_orbit_position.Add(visual_objects[i].orbit_position);
                expose_orbit_speed.Add(visual_objects[i].orbit_speed);
                expose_draw_size.Add(visual_objects[i].draw_size);
                expose_period.Add(visual_objects[i].period);
                expose_time_offset.Add(visual_objects[i].time_offset);
                expose_orbit_direction.Add(visual_objects[i].orbit_direction);
                expose_color.Add(visual_objects[i].color);
                expose_rotation_angle.Add(visual_objects[i].rotation_angle);
                expose_transformation_rotation_angle.Add(visual_objects[i].transformation_rotation_angle);
                expose_current_position.Add(visual_objects[i].current_position);
            }
            Scribe_Collections.Look(ref expose_type, "expose_type", LookMode.Value);
            Scribe_Collections.Look(ref expose_texture_path, "expose_texture_path", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_position, "expose_orbit_position", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_speed, "expose_orbit_speed", LookMode.Value);
            Scribe_Collections.Look(ref expose_draw_size, "expose_draw_size", LookMode.Value);
            Scribe_Collections.Look(ref expose_period, "expose_period", LookMode.Value);
            Scribe_Collections.Look(ref expose_time_offset, "expose_time_offset", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_direction, "expose_orbit_direction", LookMode.Value);
            Scribe_Collections.Look(ref expose_color, "expose_color", LookMode.Value);
            Scribe_Collections.Look(ref expose_rotation_angle, "expose_rotation_angle", LookMode.Value);
            Scribe_Collections.Look(ref expose_transformation_rotation_angle, "expose_transformation_rotation_angle", LookMode.Value);
            Scribe_Collections.Look(ref expose_current_position, "expose_current_position", LookMode.Value);
            if (visual_objects.NullOrEmpty() && expose_type.Count > 0) LoadedGame();
        }

        public override void GameComponentTick() {
            if (tick > spawn_ore_tick) Generator.add_asteroid_ore();
        }

        public override void GameComponentUpdate() {
            get_frame_data();
            if (wait) return;
            if (frame_changed || force_update) update();
            if (dirty_features) recache_features();
            render();
        }

        public static void init() {
            tick_manager = Find.TickManager;
            camera_driver = Find.WorldCameraDriver;
            camera = Find.WorldCamera.GetComponent<Camera>();
            camera.farClipPlane = 500.0f + Defs.Of.general.max_altitude;
            camera.fieldOfView = Defs.Of.general.field_of_view;
            RimWorld.Planet.WorldCameraManager.worldSkyboxCameraInt.farClipPlane = 500.0f + Defs.Of.general.max_altitude;
        }

        public static void update() {
            if (Settings.Container.get_multi_threaded_update) {
                Parallel.For(0, total_objects, new ParallelOptions { MaxDegreeOfParallelism = 4 }, i => {
                    visual_objects[i].update();
                    if (unpaused || force_update) visual_objects[i].update_when_unpaused();
                    if (camera_moved || force_update) visual_objects[i].update_when_camera_moved();
                    cached_matrices[i] = visual_objects[i].get_transformation_matrix(center);
                });
                for (int i = 0; i < total_objects; i++) {
                    cached_features[i]?.update_transformation(
                        visual_objects[i].get_position(),
                        visual_objects[i].draw_size.x,
                        visual_objects[i].camera_rotation
                    );
                    if (unpaused) cached_trails[i]?.update_transformation(visual_objects[i].get_position());
                }
            } else {
                for (int i = 0; i < total_objects; i++) {
                    visual_objects[i].update();
                    if (unpaused || force_update) visual_objects[i].update_when_unpaused();
                    if (camera_moved || force_update) visual_objects[i].update_when_camera_moved();
                    cached_matrices[i] = visual_objects[i].get_transformation_matrix(center);
                    cached_features[i]?.update_transformation(
                        visual_objects[i].get_position(),
                        visual_objects[i].draw_size.x,
                        visual_objects[i].camera_rotation
                    );
                    if (unpaused) cached_trails[i]?.update_transformation(visual_objects[i].get_position());
                }
            }
        }

        public static void render() {
            for (int i = 0; i < total_objects; i++) {
                Graphics.Internal_DrawMesh_Injected(
                    MeshPool.plane10,
                    submeshIndex: 0,
                    ref cached_matrices[i],
                    cached_materials[i],
                    RimWorld.Planet.WorldCameraManager.WorldLayer,
                    camera: null,
                    properties: null,
                    ShadowCastingMode.Off,
                    receiveShadows: false,
                    probeAnchor: null,
                    lightProbeUsage: LightProbeUsage.BlendProbes,
                    lightProbeProxyVolume: null
                );
                if (cached_features[i] != null && !cached_features[i].block) cached_features[i].set_active(true);
                cached_trails[i]?.set_active(true);
                if (reset_trail) cached_trails[i]?.clear_trail();
            }
            if (reset_trail) reset_trail = false;
        }

        public static int get_ore_timer() => (int) Rand.Range(0.5f * 60000, 1.5f * 60000) + tick;

        public static int get_total(Type type) => visual_objects.Where(visual_object => visual_object.type == type).Count();

        public static void recache() {
            init();
            get_frame_data();
            for (int i = 0; i < cached_trails.Length; i++) cached_trails[i]?.set_active(false);
            dirty_features = true;
            force_update = true;
            total_objects = visual_objects.Count;
            cached_matrices = new Matrix4x4[total_objects];
            cached_features = new FeatureMesh[total_objects];
            cached_trails = new Trail[total_objects];
            for (int i = 0; i < total_objects; i++) visual_objects[i].index = i;
            recache_materials();
            if (tick_manager != null && camera_driver != null && camera != null) update();
        }

        public static void recache_features() {
            dirty_features = false;
            cached_features = new FeatureMesh[total_objects];
            cached_trails = new Trail[total_objects];
            for (int i = 0; i < total_objects; i++) {
                if (Settings.Container.get_world_feature_name) {
                    cached_features[i] = visual_objects[i].get_feature();
                } else visual_objects[i].object_holder?.feature_mesh?.set_active(false);
                if (Settings.Container.get_neo_trails) {
                    cached_trails[i] = visual_objects[i].get_trail();
                } else visual_objects[i].trail_renderer?.set_active(false);
            }
        }

        public static void recache_materials() {
            cached_materials = new Material[total_objects];
            for (int i = 0; i < total_objects; i++) {
                cached_materials[i] = visual_objects[i].get_material();
            }
        }

        public static void get_frame_data() {
            if (visual_objects.NullOrEmpty()) {
                wait = true;
                return;
            }
            wait = !RimWorld.Planet.WorldRendererUtility.WorldRenderedNow && !force_update;
            if (!wait) force_update = false;
            if (!world_map_switch && RimWorld.Planet.WorldRendererUtility.WorldRenderedNow) {
                world_map_switch = true;
                reset_trail = true;
                force_update = true;
                wait = false;
            } else if (world_map_switch && !RimWorld.Planet.WorldRendererUtility.WorldRenderedNow) world_map_switch = false;
            tick = tick_manager.TicksGame;
            unpaused = tick != prev_tick;
            prev_tick = tick;
            camera_position = camera.transform.position;
            camera_moved = camera_position != prev_camera_position;
            prev_camera_position = camera_position;
            center = camera_driver.CurrentlyLookingAtPointOnSphere;
            altitude_percent = camera_driver.AltitudePercent;
            frame_changed = unpaused || camera_moved;
        }
    }

    [HarmonyPatch(typeof(Verse.Profile.MemoryUtility), "ClearAllMapsAndWorld")]
    public class MemoryUtility_ClearAllMapsAndWorld {
        public static void Prefix() {
            if (!RenderingManager.cached_trails.NullOrEmpty()) for (int i = 0; i < RenderingManager.cached_trails.Length; i++) RenderingManager.cached_trails[i]?.set_active(false);
            RenderingManager.reset_instance();
        }
    }
}
