using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;

namespace RimNauts2.World {
    [StaticConstructorOnStartup]
    public class RenderManager : RimWorld.Planet.WorldObject {
        public int prev_tick = -1;
        public Vector3 prev_cam_pos = new Vector3();
        public List<VisualObject> visual_objects = new List<VisualObject>();
        public int total_objects = 0;
        public Matrix4x4[] cached_matrices = new Matrix4x4[0];
        public Material[] cached_materials = new Material[0];
        public bool materials_dirty = true;
        private List<int> expose_type = new List<int>();
        private List<string> expose_texture_path = new List<string>();
        private List<Vector3> expose_orbit_position = new List<Vector3>();
        private List<float> expose_orbit_speed = new List<float>();
        private List<Vector3> expose_draw_size = new List<Vector3>();
        private List<int> expose_period = new List<int>();
        private List<int> expose_time_offset = new List<int>();
        private List<int> expose_orbit_direction = new List<int>();
        private List<float> expose_color = new List<float>();
        private List<float> expose_angle = new List<float>();
        private List<Vector3> expose_current_position = new List<Vector3>();

        public override void PostAdd() {
            base.PostAdd();
            // increase far clipping plane to see asteroids further back when zoomed out
            Find.WorldCamera.farClipPlane = 1600.0f;
        }

        public override void ExposeData() {
            base.ExposeData();
            for (int i = 0; i < total_objects; i++) {
                expose_type.Add(visual_objects[i].type);
                expose_texture_path.Add(visual_objects[i].texture_path);
                expose_orbit_position.Add(visual_objects[i].orbit_position);
                expose_orbit_speed.Add(visual_objects[i].orbit_speed);
                expose_draw_size.Add(visual_objects[i].draw_size);
                expose_period.Add(visual_objects[i].period);
                expose_time_offset.Add(visual_objects[i].time_offset);
                expose_orbit_direction.Add(visual_objects[i].orbit_direction);
                expose_color.Add(visual_objects[i].color);
                expose_angle.Add(visual_objects[i].angle);
                expose_current_position.Add(visual_objects[i].current_position);
            }
            Scribe_Values.Look(ref total_objects, "total_objects");
            Scribe_Collections.Look(ref expose_type, "expose_type", LookMode.Value);
            Scribe_Collections.Look(ref expose_texture_path, "expose_texture_path", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_position, "expose_orbit_position", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_speed, "expose_orbit_speed", LookMode.Value);
            Scribe_Collections.Look(ref expose_draw_size, "expose_draw_size", LookMode.Value);
            Scribe_Collections.Look(ref expose_period, "expose_period", LookMode.Value);
            Scribe_Collections.Look(ref expose_time_offset, "expose_time_offset", LookMode.Value);
            Scribe_Collections.Look(ref expose_orbit_direction, "expose_orbit_direction", LookMode.Value);
            Scribe_Collections.Look(ref expose_color, "expose_color", LookMode.Value);
            Scribe_Collections.Look(ref expose_angle, "expose_angle", LookMode.Value);
            Scribe_Collections.Look(ref expose_current_position, "expose_current_position", LookMode.Value);
            for (int i = 0; i < total_objects; i++) {
                VisualObject visual_object = new VisualObject(
                    expose_type[i],
                    expose_texture_path[i],
                    expose_orbit_position[i],
                    expose_orbit_speed[i],
                    expose_draw_size[i],
                    expose_period[i],
                    expose_time_offset[i],
                    expose_orbit_direction[i],
                    expose_color[i],
                    expose_angle[i],
                    expose_current_position[i]
                );
                visual_objects.Add(visual_object);
            }
            recache();
            expose_type = new List<int>();
            expose_texture_path = new List<string>();
            expose_orbit_position = new List<Vector3>();
            expose_orbit_speed = new List<float>();
            expose_draw_size = new List<Vector3>();
            expose_period = new List<int>();
            expose_time_offset = new List<int>();
            expose_orbit_direction = new List<int>();
            expose_color = new List<float>();
            expose_angle = new List<float>();
            expose_current_position = new List<Vector3>();
        }

        public override void Draw() {
            int tick = Find.TickManager.TicksGame;
            Vector3 cam_pos = Find.WorldCamera.transform.position;
            bool unpaused = tick != prev_tick;
            bool camera_moved = cam_pos != prev_cam_pos;
            // update cache
            recache_materials();
            // update objects
            if (unpaused || camera_moved) {
                Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
                Parallel.For(0, total_objects, i => {
                    if (unpaused) visual_objects[i].update_position(tick);
                    cached_matrices[i] = visual_objects[i].get_transformation_matrix(center);
                });
                prev_tick = tick;
                prev_cam_pos = cam_pos;
            }
            // draw objects
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

        public void populate(
            byte id,
            int amount,
            string[] texture_paths,
            Vector3 orbit_position_default,
            Vector3 orbit_spread,
            Vector2 orbit_speed_between,
            Vector2 size_between,
            Vector2 color_between,
            bool random_angle,
            bool random_direction
        ) {
            total_objects += amount;
            recache();
            float avg_tile_size = Find.WorldGrid.averageTileSize;
            System.Random rnd = new System.Random();
            for (int i = 0; i < amount; i++) {
                VisualObject visual_object = new VisualObject(
                    id,
                    texture_paths[rnd.Next(texture_paths.Length)],
                    orbit_position_default,
                    orbit_spread,
                    orbit_speed_between,
                    size_between,
                    color_between,
                    random_angle,
                    avg_tile_size,
                    random_direction
                );
                visual_objects.Add(visual_object);
            }
        }

        public void depopulate(byte id) {
            int removed_amount = visual_objects.RemoveAll(visual_object => visual_object.type == id);
            total_objects -= removed_amount;
            recache();
        }

        public void recache() {
            cached_matrices = new Matrix4x4[total_objects];
            materials_dirty = true;
        }

        public void recache_materials() {
            if (!materials_dirty) return;
            materials_dirty = false;
            cached_materials = new Material[total_objects];
            for (int i = 0; i < total_objects; i++) cached_materials[i] = visual_objects[i].get_material();
        }
    }
}
