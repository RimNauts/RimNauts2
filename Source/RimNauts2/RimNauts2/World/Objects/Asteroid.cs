using UnityEngine;

namespace RimNauts2.World.Objects {
    public class Asteroid : NEO {
        public Asteroid(
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
        ) : base(
            Type.Asteroid,
            texture_path,
            orbit_position,
            orbit_speed,
            draw_size,
            period,
            time_offset,
            orbit_direction,
            color,
            rotation_angle,
            current_position
        ) { }

        public override void update() {
            base.update();
        }

        public override void update_when_unpaused() {
            base.update_when_unpaused();
        }

        public override void update_when_camera_moved() {
            base.update_when_camera_moved();
        }
    }
}
