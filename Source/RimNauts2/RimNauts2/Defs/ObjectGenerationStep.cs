using UnityEngine;
using Verse;

namespace RimNauts2.Defs {
    public class ObjectGenerationStep : Def {
        public int amount;
        public int type;
        public string texture_path;
        public Vector3? orbit_position;
        public float? orbit_speed;
        public Vector3? draw_size;
        public int? period;
        public int? time_offset;
        public int? orbit_direction;
        public float? color;
        public float? rotation_angle;
        public Vector3? current_position;
        public string object_holder_def;
    }
}
