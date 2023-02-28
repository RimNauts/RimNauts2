using System;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Objects {
    public abstract class NEO {
        public int index;
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
        public ObjectHolder object_holder;

        public NEO(
            Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            Vector3? current_position = null
        ) {
            this.type = type;
            this.texture_path = texture_path ?? type.texture_path();
            this.orbit_position = orbit_position ?? type.orbit_position();
            this.orbit_speed = orbit_speed ?? type.orbit_speed();
            if (draw_size == null) {
                float size = type.size();
                this.draw_size = new Vector3(size, 1.0f, size);
            } else this.draw_size = (Vector3) draw_size;
            this.period = period ?? (int) (36000.0f + (6000.0f * (Rand.Value - 0.5f)));
            this.time_offset = time_offset ?? Rand.Range(0, this.period);
            this.orbit_direction = orbit_direction ?? type.orbit_direction();
            this.color = color ?? type.color();
            this.rotation_angle = rotation_angle ?? type.rotation_angle();
            this.current_position = current_position ?? this.orbit_position;
            Vector3 axis = Vector3.up;
            Quaternion.AngleAxis_Injected(this.rotation_angle, ref axis, out rotation);
            if (current_position == null) update_position(tick: 0);
        }

        public virtual void randomize() {
            orbit_position = type.orbit_position();
            orbit_speed = type.orbit_speed();
            float size = type.size();
            draw_size = new Vector3(size, 1.0f, size);
            period = (int) (36000.0f + (6000.0f * (Rand.Value - 0.5f)));
            time_offset = Rand.Range(0, period);
            orbit_direction = type.orbit_direction();
            color = type.color();
            rotation_angle = type.rotation_angle();
            current_position = orbit_position;
            Vector3 axis = Vector3.up;
            Quaternion.AngleAxis_Injected(rotation_angle, ref axis, out rotation);
            material = null;
            get_material();
            update_when_unpaused();
            update();
        }

        public virtual void update() {
            if (object_holder == null) return;
            object_holder.hide_now = Patch.HideIcons.check_object_holder(current_position);
        }

        public virtual void update_when_unpaused() {
            update_position(RenderingManager.tick);
            if (object_holder != null) object_holder.position = current_position;
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
            Vector3 towards_camera = Vector3.Cross(center, Vector3.up);
            Quaternion.LookRotation_Injected(ref towards_camera, ref center, out Quaternion transformation_rotation);
            transformation_rotation *= rotation;
            Matrix4x4.TRS_Injected(ref current_position, ref transformation_rotation, ref draw_size, out Matrix4x4 transformation_matrix);
            return transformation_matrix;
        }

        public virtual Material get_material() {
            if (material != null) return material;
            material = Assets.materials[texture_path];
            material.color = new Color(color, color, color);
            return material;
        }
    }
}
