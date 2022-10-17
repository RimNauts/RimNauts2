namespace RimNauts2 {
    [RimWorld.DefOf]
    public static class BiomeDefOf {
        public static RimWorld.BiomeDef RockMoonBiome;
        public static RimWorld.BiomeDef SatelliteBiome;

        static BiomeDefOf() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
    }
}
