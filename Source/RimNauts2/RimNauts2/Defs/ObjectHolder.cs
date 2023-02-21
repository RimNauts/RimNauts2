using Verse;

namespace RimNauts2.Defs {
    public class ObjectHolder : Def {
        public int type;
        public MapGeneratorDef map_generator;
        [NoTranslate]
        public string texture_path;
        public string biome_def;
    }
}
