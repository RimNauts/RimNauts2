using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;

namespace RimNauts2.World {
    [StaticConstructorOnStartup]
    class RenderingManager : GameComponent {
        private static TickManager tick_manager;
        private static RimWorld.Planet.WorldCameraDriver camera_driver;
        private static Camera camera;

        public static int tick;
        private static int prev_tick;

        public static Vector3 camera_position;
        private static Vector3 prev_camera_position = Vector3.zero;

        public static Vector3 center;
        public static float altitude_percent;

        public static bool unpaused;
        public static bool camera_moved;
        public static bool frame_changed;
        private static bool wait;
        public static bool force_update;
        public static bool materials_dirty;

        public static Matrix4x4[] cached_matrices;
        public static Material[] cached_materials;

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
        private static List<Vector3> expose_current_position;

        public RenderingManager(Game game) : base() {
            tick_manager = null;
            camera_driver = null;
            camera = null;
            tick = 0;
            prev_tick = -1;
            camera_position = Vector3.one;
            prev_camera_position = Vector3.zero;
            center = Vector3.zero;
            altitude_percent = 0.0f;
            unpaused = false;
            camera_moved = false;
            frame_changed = false;
            wait = false;
            force_update = true;
            materials_dirty = true;
            cached_matrices = new Matrix4x4[0];
            cached_materials = new Material[0];
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
            expose_current_position = new List<Vector3>();
        }

        public override void FinalizeInit() {
            tick_manager = Find.TickManager;
            camera_driver = Find.WorldCameraDriver;
            camera = Find.WorldCamera.GetComponent<Camera>();
            camera.farClipPlane = 1600.0f;
            get_frame_data();
        }

        public override void StartedNewGame() {
            visual_objects = new List<Objects.NEO>();
            foreach (var (type, amount) in Settings.Container.get_object_generation_steps) {
                if (Defs.Loader.get_object_holder(type) != null) {
                    Generator.add_object_holder(amount, type);
                } else {
                    Generator.add_visual_object(amount, type);
                }
            }
        }

        public override void LoadedGame() {
            if (expose_type.NullOrEmpty()) {
                StartedNewGame();
                return;
            }
            visual_objects = new List<Objects.NEO>();
            total_objects = expose_type.Count;
            for (int i = 0; i < total_objects; i++) {
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
                    expose_current_position[i]
                );
                visual_objects.Add(neo);
            }
            recache();
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
            Scribe_Collections.Look(ref expose_current_position, "expose_current_position", LookMode.Value);
            if (visual_objects.NullOrEmpty() && expose_type.Count > 0) LoadedGame();
        }

        public override void GameComponentUpdate() {
            // ran every frame
            get_frame_data();
            if (wait) return;
            update();
            recache_materials();
            render();
        }

        public static void update() {
            if (!frame_changed) return;
            Parallel.For(0, total_objects, i => {
                visual_objects[i].update();
                if (unpaused) visual_objects[i].update_when_unpaused();
                if (camera_moved) visual_objects[i].update_when_camera_moved();
                cached_matrices[i] = visual_objects[i].get_transformation_matrix(center);
            });
        }

        public static void render() {
            for (int i = 0; i < total_objects; i++) {
                Graphics.DrawMesh(
                    MeshPool.plane10,
                    cached_matrices[i],
                    cached_materials[i],
                    RimWorld.Planet.WorldCameraManager.WorldLayer,
                    camera: null,
                    submeshIndex: 0,
                    properties: null,
                    ShadowCastingMode.Off,
                    receiveShadows: false,
                    probeAnchor: null,
                    lightProbeUsage: LightProbeUsage.BlendProbes,
                    lightProbeProxyVolume: null
                );
            }
        }

        public static int get_total(Type type) => visual_objects.Where(visual_object => visual_object.type == type).Count();

        public static void recache() {
            force_update = true;
            materials_dirty = true;
            total_objects = visual_objects.Count;
            cached_matrices = new Matrix4x4[total_objects];
            update();
        }

        public static void recache_materials() {
            if (!materials_dirty) return;
            materials_dirty = false;
            cached_materials = new Material[total_objects];
            for (int i = 0; i < total_objects; i++) cached_materials[i] = visual_objects[i].get_material();
        }

        public static void get_frame_data() {
            wait = (!RimWorld.Planet.WorldRendererUtility.WorldRenderedNow || total_objects <= 0) && !force_update;
            if (!wait) force_update = false;
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
}
