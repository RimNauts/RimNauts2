using RimWorld;

namespace RimNauts2 {
    [DefOf]
    public static class MoonDefOf {
        public static BiomeDef RockMoonBiome;

        static MoonDefOf() {
            DefOfHelper.EnsureInitializedInCtor(typeof(MoonDefOf));
        }
    }
}
