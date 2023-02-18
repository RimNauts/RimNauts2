using System;
using UnityEngine;
using Verse;

namespace RimNauts2.WorldObject {
    public class VisualObject {
        public int material_index;
        public Vector3 orbit_position;
        public float orbit_speed;
        public Vector3 draw_size;
        public int period;
        public int time_offset;
        public int orbit_direction;
        public Vector3 current_position;
        public Matrix4x4 transformation_matrix;
        public float distance_from_camera;

        public VisualObject(
            int total_materials,
            Vector3 orbit_position_default,
            Vector3 orbit_spread,
            Vector2 orbit_speed_between,
            Vector2 size_between,
            float avg_tile_size,
            bool random_direction
        ) {
            material_index = Rand.Range(0, total_materials);
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
            orbit_direction = random_direction && Rand.Bool ? -1 : 1;
            current_position = orbit_position;
            distance_from_camera = 0;
            update_position(tick: 0);
        }

        public void update_position(int tick) {
            float time = orbit_speed * orbit_direction * tick + time_offset;
            current_position.x = (orbit_position.x - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Cos(6.28f / period * time);
            current_position.z = (orbit_position.z - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Sin(6.28f / period * time);
        }

        public void update_transformation_matrix(Vector3 center) {
            transformation_matrix = Matrix4x4.identity;
            transformation_matrix.SetTRS(
                pos: current_position,
                q: Quaternion.LookRotation(Vector3.Cross(center, Vector3.right), center),
                s: draw_size
            );
        }

        public void update_distance_from_camera(Vector3 cam_pos, Vector3 cam_forward) {
            Vector3 heading = current_position - cam_pos;
            distance_from_camera = Vector3.Dot(heading, cam_forward);
        }
    }
}
