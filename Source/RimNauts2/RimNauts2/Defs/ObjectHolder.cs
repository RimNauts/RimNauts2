using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Defs {
    public class ObjectHolder : Def {
        public int type;
        public MapGeneratorDef map_generator;
        public List<string> texture_paths;
        public string biome_def;
        public bool keep_after_abandon;
        public Vector2? limited_days_between;
    }
}
