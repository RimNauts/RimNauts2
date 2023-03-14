using UnityEngine;

namespace RimNauts2.World {
    public class FeatureMesh {
        private readonly TMPro.TextMeshPro text_mesh;
        private Quaternion rotation;
        public bool active;
        public bool block;

        public FeatureMesh(string text, Color color) {
            Vector3 axis = Vector3.up;
            Quaternion.AngleAxis_Injected(270.0f, ref axis, out rotation);
            GameObject new_game_object = Object.Instantiate(Assets.game_object_world_feature);
            Object.DontDestroyOnLoad(new_game_object);
            text_mesh = new_game_object.GetComponent<TMPro.TextMeshPro>();
            text_mesh.text = text;
            text_mesh.color = color * 1.2f;
            text_mesh.font = TMPro.TMP_FontAsset.CreateFontAsset((Font) Resources.Load("Fonts/Arial_small"));
            text_mesh.fontSize = 40.0f;
            text_mesh.outlineWidth = 0.2f;
            text_mesh.outlineColor = Color.black;
            text_mesh.overflowMode = TMPro.TextOverflowModes.Overflow;
            foreach (Material sharedMaterial in text_mesh.GetComponent<MeshRenderer>().sharedMaterials) {
                sharedMaterial.renderQueue = RimWorld.Planet.WorldMaterials.FeatureNameRenderQueue;
            }
            set_active(false);
            RenderingManager.dirty_features = true;
        }

        public void destroy() => Object.Destroy(text_mesh.gameObject);

        public void set_active(bool active) {
            if (this.active == active) return;
            text_mesh.gameObject.SetActive(active);
            this.active = active;
        }

        public void check_hide(Vector3 position) {
            if ((Vector3.Distance(text_mesh.transform.localPosition, RenderingManager.center) + 220.0f) > Vector3.Distance(RenderingManager.center, RenderingManager.camera_position)) {
                if (!block) block = true;
            } else if (block) block = false;
        }

        public void update_transformation(Vector3 position, float size, Quaternion camera_rotation) {
            if (block) set_active(false);
            position = Vector3.MoveTowards(position, RenderingManager.camera_position, 50.0f);
            text_mesh.transform.localPosition = new Vector3(
                position.x,
                position.y - (size / 2.0f) - (2.0f * (RenderingManager.altitude_percent + 1.0f)),
                position.z
            );
            text_mesh.transform.localRotation = camera_rotation * rotation * Quaternion.Euler(90.0f, 0f, 0f);
        }
    }
}
