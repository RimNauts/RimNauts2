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
        public Dictionary<byte, VisualObject[]> cached_objects = new Dictionary<byte, VisualObject[]>();
        public Dictionary<byte, string[]> cached_texture_paths = new Dictionary<byte, string[]>();
        public Dictionary<byte, Material[]> cached_materials = new Dictionary<byte, Material[]>();
        public int total_objects = 0;
        public List<byte> object_types = new List<byte>();
        public Matrix4x4[] matrices;
        public Material[] materials;
        public bool matrices_dirty = true;
        public bool materials_dirty = true;

        public override void PostAdd() {
            base.PostAdd();
            // increase far clipping plane to see asteroids further back when zoomed out
            Find.WorldCamera.farClipPlane = 1600.0f;
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
            matrices_dirty = true;
            materials_dirty = true;
            total_objects += amount;
            object_types.Add(id);
            int total_materials = texture_paths.Length;
            cached_texture_paths[id] = texture_paths;
            cached_materials[id] = new Material[total_materials];
            cached_objects[id] = new VisualObject[amount];
            float avg_tile_size = Find.WorldGrid.averageTileSize;
            for (int i = 0; i < amount; i++) {
                VisualObject visual_object = new VisualObject(
                    total_materials,
                    orbit_position_default,
                    orbit_spread,
                    orbit_speed_between,
                    size_between,
                    avg_tile_size,
                    random_direction
                );
                cached_objects[id][i] = visual_object;
            }
        }

        public void depopulate(byte id) {
            matrices_dirty = true;
            materials_dirty = true;
            total_objects -= cached_objects[id].Length;
            object_types.Remove(id);
            cached_objects.Remove(id);
        }

        public void recache_matrices() {
            if (!matrices_dirty) return;
            matrices_dirty = false;
            matrices = new Matrix4x4[total_objects];
            List<VisualObject> visualObjects = new List<VisualObject>();
            foreach (var id in object_types) visualObjects.AddRange(cached_objects[id]);
            Parallel.ForEach(visualObjects, (visual_object, state, index) => {
                matrices[index] = visual_object.transformation_matrix;
            });
        }

        public void recache_materials() {
            if (!materials_dirty) return;
            materials_dirty = false;
            materials = new Material[total_objects];
            int index = 0;
            foreach (var id in object_types) {
                for (int j = 0; j < cached_objects[id].Length; j++) {
                    materials[index] = get_material(id, cached_objects[id][j].material_index);
                    index++;
                }
            }
        }

        public override void Draw() {
            int tick = Find.TickManager.TicksGame;
            Vector3 cam_pos = Find.WorldCamera.transform.position;
            bool unpaused = tick != prev_tick;
            bool camera_moved = cam_pos != prev_cam_pos;
            // update objects
            if (unpaused || camera_moved) {
                matrices_dirty = true;
                Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
                Vector3 cam_forward = Find.WorldCamera.transform.forward;
                foreach (var (id, visual_objects) in cached_objects) {
                    Parallel.ForEach(visual_objects, visual_object => {
                        if (unpaused) visual_object.update_position(tick);
                        visual_object.update_transformation_matrix(center);
                        visual_object.update_distance_from_camera(cam_pos, cam_forward);
                    });
                }
                prev_tick = tick;
                prev_cam_pos = cam_pos;
            }
            // update cache
            recache_matrices();
            recache_materials();
            // draw objects
            for (int i = 0; i < total_objects; i++) {
                Graphics.DrawMesh(
                    MeshPool.plane10,
                    matrices[i],
                    materials[i],
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

        public Material get_material(byte id, int index) {
            Material material = cached_materials[id][index];
            if (material != null) return material;
            material = MaterialPool.MatFrom(
                cached_texture_paths[id][index],
                ShaderDatabase.WorldOverlayCutout,
                RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
            );
            cached_materials[id][index] = material;
            return material;
        }
    }
}
