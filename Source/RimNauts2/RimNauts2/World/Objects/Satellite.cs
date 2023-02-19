using System;
using UnityEngine;

namespace RimNauts2.World.Objects {
    public class Satellite : NEO {
        public Satellite() : base(Type.Satellite) { }

        public Satellite(
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
        ) : base(
            Type.Satellite,
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

        public override void update_when_unpaused(int tick) {
            base.update_when_unpaused(tick);
        }

        public override void update_when_camera_moved() {
            base.update_when_camera_moved();
        }
    }
}
