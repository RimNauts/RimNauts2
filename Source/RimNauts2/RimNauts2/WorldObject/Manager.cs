using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;

namespace RimNauts2.WorldObject {
    [StaticConstructorOnStartup]
    public class Manager : RimWorld.Planet.WorldObject {
        public int prev_tick = 0;
        public Vector3 prev_cam_pos = new Vector3();
        public List<VisualObject> visual_objects = new List<VisualObject>();
        public int total_objects = 0;
        public Matrix4x4[] cached_matrices = new Matrix4x4[0];
        public Material[] cached_materials = new Material[0];
        public bool materials_dirty = true;

        public override void PostAdd() {
            base.PostAdd();
            // increase far clipping plane to see asteroids further back when zoomed out
            Find.WorldCamera.farClipPlane = 1600.0f;
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
            for (int i = 0; i < total_objects; i++) {
                cached_materials[i] = visual_objects[i].get_material();
            }
        }
    }
}
