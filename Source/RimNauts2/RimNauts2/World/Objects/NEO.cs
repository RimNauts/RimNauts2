using System;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Objects {
    public abstract class NEO {
        public Type type;
        public string texture_path;
        public Vector3 orbit_position;
        public float orbit_speed;
        public Vector3 draw_size;
        public int period;
        public int time_offset;
        public int orbit_direction;
        public float color;
        public float rotation_angle;
        public Vector3 current_position;
        public Material material;
        public Quaternion rotation;
        public bool object_holder;

        public NEO(Type type) {
            this.type = type;
            texture_path = type.texture_path();
            orbit_position = type.orbit_position();
            orbit_speed = type.orbit_speed();
            float size = type.size();
            draw_size = new Vector3(size, 1.0f, size);
            orbit_direction = type.orbit_direction();
            color = type.color();
            rotation_angle = type.rotation_angle();
            period = (int) (36000.0f + (6000.0f * (Rand.Value - 0.5f)));
            time_offset = Rand.Range(0, period);
            current_position = orbit_position;
            rotation = Quaternion.AngleAxis(rotation_angle, Vector3.up);
            update_position(tick: 0);
        }

        public NEO(
            Type type,
            string texture_path,
            Vector3 orbit_position,
            float orbit_speed,
            Vector3 draw_size,
            int period,
            int time_offset,
            int orbit_direction,
            float color,
            float rotation_angle,
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
            this.rotation_angle = rotation_angle;
            this.current_position = current_position;
            rotation = Quaternion.AngleAxis(rotation_angle, Vector3.up);
        }

        public virtual void update() { }

        public virtual void update_when_unpaused(int tick) {
            update_position(tick);
        }

        public virtual void update_when_camera_moved() { }

        public virtual void update_position(int tick) {
            float time = orbit_speed * orbit_direction * tick + time_offset;
            float num1 = 6.28f / period * time;
            float num2 = Math.Abs(orbit_position.y) / 2;
            current_position.x = (orbit_position.x - num2) * (float) Math.Cos(num1);
            current_position.z = (orbit_position.z - num2) * (float) Math.Sin(num1);
        }

        public virtual Matrix4x4 get_transformation_matrix(Vector3 center) {
            Matrix4x4 transformation_matrix = Matrix4x4.identity;
            transformation_matrix.SetTRS(
                pos: current_position,
                q: Quaternion.LookRotation(Vector3.Cross(center, Vector3.up), center) * rotation,
                s: draw_size
            );
            return transformation_matrix;
        }

        public virtual Material get_material() {
            if (material != null) return material;
            material = MaterialPool.MatFrom(
                texture_path,
                Assets.neo_shader,
                RimWorld.Planet.WorldMaterials.WorldObjectRenderQueue
            );
            material.color = new Color(color, color, color);
            return material;
        }
    }
}
