using System;
using UnityEngine;
using Verse;

namespace RimNauts2.WorldObject {
    public class VisualObject {
        public int type;
        public string texture_path;
        public Vector3 orbit_position;
        public float orbit_speed;
        public Vector3 draw_size;
        public int period;
        public int time_offset;
        public int orbit_direction;
        public float color;
        public float angle;
        public Vector3 current_position;
        public Material material;
        public Quaternion rotation;

        public VisualObject(
            int type,
            string texture_path,
            Vector3 orbit_position,
            float orbit_speed,
            Vector3 draw_size,
            int period,
            int time_offset,
            int orbit_direction,
            float color,
            float angle,
            Vector3 current_position
        ) {
            this.type = type;
            this.texture_path = texture_path;
            this.orbit_position = orbit_position;
            this.orbit_speed = orbit_speed;
            this.draw_size = draw_size;
            this.period = period;
            this.time_offset = time_offset;
            this.orbit_direction = orbit_direction;
            this.color = color;
            this.angle = angle;
            this.current_position = current_position;
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        public VisualObject(
            int type,
            string texture_path,
            Vector3 orbit_position_default,
            Vector3 orbit_spread,
            Vector2 orbit_speed_between,
            Vector2 size_between,
            Vector2 color_between,
            bool random_angle,
            float avg_tile_size,
            bool random_direction
        ) {
            this.type = type;
            color = Rand.Range(color_between.x, color_between.y);
            angle = random_angle ? UnityEngine.Random.value * 360.0f : 270.0f;
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
            this.texture_path = texture_path;
            orbit_position = new Vector3 {
                x = orbit_position_default.x + (float) ((Rand.Value - 0.5f) * (orbit_position_default.x * orbit_spread.x)),
                y = Rand.Range(Math.Abs(orbit_position_default.y) * -1, Math.Abs(orbit_position_default.y)),
                z = orbit_position_default.z + (float) ((Rand.Value - 0.5f) * (orbit_position_default.z * orbit_spread.z)),
            };
            orbit_speed = Rand.Range(orbit_speed_between.x, orbit_speed_between.y);
            float size = Rand.Range(size_between.x, size_between.y) * 20 * avg_tile_size;
            draw_size = new Vector3(size, 1.0f, size);
            period = (int) (36000.0f + (6000.0f * (Rand.Value - 0.5f)));
            time_offset = Rand.Range(0, period);
            orbit_direction = random_direction && Rand.Bool ? 1 : -1;
            current_position = orbit_position;
            update_position(tick: 0);
        }

        public void update_position(int tick) {
            float time = orbit_speed * orbit_direction * tick + time_offset;
            current_position.x = (orbit_position.x - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Cos(6.28f / period * time);
            current_position.z = (orbit_position.z - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Sin(6.28f / period * time);
        }

        public Matrix4x4 get_transformation_matrix(Vector3 center) {
            Matrix4x4 transformation_matrix = Matrix4x4.identity;
            transformation_matrix.SetTRS(
                pos: current_position,
                q: Quaternion.LookRotation(Vector3.Cross(center, Vector3.up), center) * rotation,
                s: draw_size
            );
            return transformation_matrix;
        }

        public Material get_material() {
            if (material != null) return material;
            material = MaterialPool.MatFrom(
                texture_path,
                ShaderDatabase.WorldOverlayCutout,
                RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
            );
            material.color = new Color(color, color, color);
            return material;
        }
    }
}
