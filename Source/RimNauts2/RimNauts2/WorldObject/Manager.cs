using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Verse;

namespace RimNauts2.WorldObject {
    [StaticConstructorOnStartup]
    public class Manager : RimWorld.Planet.WorldObject {
        public Material[] materials;
        public List<string> texture_paths = new List<string>();
        public List<VisualObject> visual_objects = new List<VisualObject>();
        public int population_size = 1000;
        public int prev_tick = 0;
        public Vector3 prev_cam_pos = new Vector3();

        public override void PostAdd() {
            base.PostAdd();
            texture_paths = new List<string> {
                "Satellites/Asteroids/RimNauts2_Asteroid_1",
                "Satellites/Asteroids/RimNauts2_Asteroid_2",
                "Satellites/Asteroids/RimNauts2_Asteroid_3",
                "Satellites/Asteroids/RimNauts2_Asteroid_4",
                "Satellites/Asteroids/RimNauts2_Asteroid_5",
                "Satellites/Asteroids/RimNauts2_Asteroid_6",
                "Satellites/Asteroids/RimNauts2_Asteroid_7",
                "Satellites/Asteroids/RimNauts2_Asteroid_8",
                "Satellites/Asteroids/RimNauts2_Asteroid_9",
            };
            int material_size = texture_paths.Count;
            // init materials
            materials = new Material[material_size];
            for (int i = 0; i < material_size; i++) materials[i] = null;
            // populate visual_objects
            float avg_tile_size = Find.WorldGrid.averageTileSize;
            for (int i = 0; i < population_size; i++) {
                VisualObject visual_object = new VisualObject(
                    material_size,
                    orbit_spread: new Vector3 { x = 0.3f, y = 0.1f, z = 0.3f },
                    orbit_position_default: new Vector3 { x = 250.0f, y = 5.0f, z = 250.0f },
                    orbit_speed_between: new Vector2 { x = 2.0f, y = 4.0f },
                    size_between: new Vector2 { x = 1.0f, y = 1.5f },
                    avg_tile_size,
                    random_direction: false
                );
                visual_objects.Add(visual_object);
            }
            // increase far clipping plane to see asteroids further back when zoomed out
            Find.WorldCamera.farClipPlane = 1400.0f;
        }

        public override void Draw() {
            int tick = Find.TickManager.TicksGame;
            Vector3 cam_pos = Find.WorldCamera.transform.position;
            bool unpaused = tick != prev_tick;
            bool camera_moved = cam_pos != prev_cam_pos;
            if (unpaused || camera_moved) {
                Vector3 center = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
                Parallel.ForEach(visual_objects, visual_object => {
                    if (unpaused) visual_object.update_position(tick);
                    visual_object.update_transformation_matrix(center);
                });
                prev_tick = tick;
                prev_cam_pos = cam_pos;
            }
            foreach (VisualObject visual_object in visual_objects) {
                Graphics.DrawMesh(
                    MeshPool.plane10,
                    visual_object.transformation_matrix,
                    get_material(visual_object.material_index),
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

        public Material get_material(int index) {
            Material material = materials[index];
            if (material != null) return material;
            material = MaterialPool.MatFrom(
                texture_paths[index],
                ShaderDatabase.WorldOverlayTransparentLit,
                RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
            );
            materials[index] = material;
            return material;
        }
    }
}
