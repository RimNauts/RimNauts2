namespace RimNauts2 {
    [RimWorld.DefOf]
    public static class MoonDefOf {
        public static RimWorld.BiomeDef RockMoonBiome;

        static MoonDefOf() {
            RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(MoonDefOf));
        }
    }
}
