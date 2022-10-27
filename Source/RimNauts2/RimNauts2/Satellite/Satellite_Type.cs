using System.Collections.Generic;

namespace RimNauts2 {
    public enum Satellite_Type {
        None = 0,
        Asteroid = 1,
        Moon = 2,
        Artifical_Satellite = 3,
        Asteroid_Ore = 4,
        Buffer = 5,
        Space_Station = 6,
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
                case Satellite_Type.Space_Station:
                    return SatelliteDefOf.Satellite.SpaceStationObjects;
                default:
                    return new List<string>();
            }
        }

        public static Satellite_Type get_type_from_string(string type_string) {
            switch (type_string) {
                case "Asteroid":
                    return Satellite_Type.Asteroid;
                case "Moon":
                    return Satellite_Type.Moon;
                case "Artifical_Satellite":
                    return Satellite_Type.Artifical_Satellite;
                case "Asteroid_Ore":
                    return Satellite_Type.Asteroid_Ore;
                case "Buffer":
                    return Satellite_Type.Buffer;
                case "Space_Station":
                    return Satellite_Type.Space_Station;
                default:
                    return Satellite_Type.None;
            }
        }
    }
}
