namespace RimNauts2 {
    [RimWorld.DefOf]
    public static class SatelliteDefOf {
        public static SatelliteDef Satellite;

        static SatelliteDefOf() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(SatelliteDefOf));
    }
}
