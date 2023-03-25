using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Defs {
    public class ObjectMetadata : Def {
        public int type;
        public List<string> texture_paths = new List<string>();
        public Vector3 orbit_position = Vector3.one;
        public Vector3 orbit_spread = Vector3.one;
        public Vector2 orbit_speed_between = Vector2.one;
        public Vector2 size_between = Vector2.one;
        public Vector2 color_between = Vector2.one;
        public bool random_rotation;
        public bool random_transformation_rotation;
        public bool random_direction;
        public bool object_holder;
        public bool trail;
        public float trail_width = 1.0f;
        public float trail_length = 1.0f;
        public Color? trail_color = null;
        public float trail_brightness = 1.0f;
        public float trail_transparency = 1.0f;
    }
}
