using System;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    public class Trail {
        private readonly TrailRenderer trail_renderer;
        private TimeSpeed prev_speed;
        public bool active;
        float trail_length;
        public bool first_render = true;

        public Trail(
            float trail_width,
            float trail_length,
            Color? trail_color,
            float trail_brightness,
            float trail_transparency,
            int neo_render_queue,
            float neo_size,
            string neo_texture_path
        ) {
            prev_speed = TimeSpeed.Paused;
            this.trail_length = trail_length;
            GameObject game_object = UnityEngine.Object.Instantiate(Assets.game_object_world_feature);
            UnityEngine.Object.DontDestroyOnLoad(game_object);
            game_object.GetComponent<TMPro.TextMeshPro>().enabled = false;
            trail_renderer = game_object.AddComponent<TrailRenderer>();
            trail_renderer.startWidth = neo_size * trail_width;
            trail_renderer.endWidth = 0.0f;
            trail_renderer.time = 0.0f;
            trail_renderer.material = new Material(Shader.Find("Sprites/Default")) {
                mainTexture = Assets.get_content<Texture2D>("RimNauts2_Trail")
            };
            foreach (Material sharedMaterial in trail_renderer.sharedMaterials) {
                sharedMaterial.renderQueue = neo_render_queue;
            }
            Color color = trail_color ?? Assets.get_average_color_from_texture(neo_texture_path);
            color = color.RGBMultiplied(trail_brightness);
            trail_renderer.startColor = new Color(color.r, color.g, color.b, trail_transparency);
            trail_renderer.endColor = new Color(color.r, color.g, color.b, 0.0f);
            set_active(false);
            RenderingManager.dirty_features = true;
        }

        public void destroy() => UnityEngine.Object.Destroy(trail_renderer.gameObject);

        public void set_active(bool active) {
            if (this.active == active) return;
            trail_renderer.gameObject.SetActive(active);
            this.active = active;
            trail_renderer.enabled = active;
            clear_trail();
        }

        public void update_transformation(Vector3 position) {
            if (!active) return;
            trail_renderer.transform.set_position_Injected(ref position);
            TimeSpeed speed = RenderingManager.tick_manager.CurTimeSpeed;
            if (speed != prev_speed) {
                prev_speed = speed;
                float speed_value = (float) Math.Pow(3.0, (double) speed - 1.0);
                if (speed_value <= 0) {
                    trail_renderer.time = 0.0f;
                } else trail_renderer.time = trail_length / speed_value;
            }
            if (first_render) {
                clear_trail();
                first_render = false;
            }
        }

        public void clear_trail() => trail_renderer.Clear();
    }
}
