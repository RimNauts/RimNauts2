using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Defs {
    public class ObjectMetadata : Def {
        public List<string> texture_paths = new List<string>();
        public Vector3 orbit_position = Vector3.one;
        public Vector3 orbit_spread = Vector3.one;
        public Vector2 orbit_speed_between = Vector2.one;
        public Vector2 size_between = Vector2.one;
        public Vector2 color_between = Vector2.one;
        public bool random_rotation;
        public bool random_direction;
    }
}
