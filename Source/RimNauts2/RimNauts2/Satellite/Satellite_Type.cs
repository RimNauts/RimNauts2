using System.Collections.Generic;

namespace RimNauts2 {
    public enum Satellite_Type {
        None = 0,
        Asteroid = 1,
        Moon = 2,
        Artifical_Satellite = 3,
        Asteroid_Ore = 4,
        Buffer = 5,
    }

    public static class Satellite_Type_Methods {
        public static List<string> WorldObjects(this Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return SatelliteDefOf.Satellite.AsteroidObjects;
                case Satellite_Type.Moon:
                    return SatelliteDefOf.Satellite.MoonObjects;
                case Satellite_Type.Artifical_Satellite:
                    return SatelliteDefOf.Satellite.ArtificalSatelliteObjects;
                case Satellite_Type.Asteroid_Ore:
                    return SatelliteDefOf.Satellite.AsteroidOreObjects;
                default:
                    return new List<string>();
            }
        }

        public static Satellite_Type get_type_from_biome(string biome_def) {
            switch (biome_def) {
                case "RimNauts2_Satellite_Biome":
                    return Satellite_Type.Asteroid;
                case "RimNauts2_Artifical_Satellite_Biome":
                    return Satellite_Type.Artifical_Satellite;
                case "RimNauts2_MoonBarren_Biome":
                    return Satellite_Type.Moon;
                case "RimNauts2_OreSteel_Biome":
                    return Satellite_Type.Asteroid_Ore;
                case "RimNauts2_OreGold_Biome":
                    return Satellite_Type.Asteroid_Ore;
                case "RimNauts2_OrePlasteel_Biome":
                    return Satellite_Type.Asteroid_Ore;
                default:
                    return Satellite_Type.None;
            }
        }
    }
}
