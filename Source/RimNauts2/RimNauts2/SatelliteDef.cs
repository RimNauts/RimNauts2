using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class SatelliteDef : Def {
        public List<string> Biomes;
        public int TotalSatelliteObjects;
        public float CrashingAsteroidObjectPercentage;
        public List<string> AsteroidObjects;
        public List<string> AsteroidOreObjects;
        public List<string> WasteObjects;
        public List<string> MoonObjects;
        public List<string> ArtificalSatelliteObjects;

        public Vector3 AsteroidObjectsOrbitPosition;
        public Vector3 AsteroidObjectsOrbitSpread;
        public Vector2 AsteroidObjectsOrbitSpeedBetween;
        public bool AsteroidObjectsOrbitRandomDirection;

        public Vector3 MoonObjectsOrbitPosition;
        public Vector3 MoonObjectsOrbitSpread;
        public Vector2 MoonObjectsOrbitSpeedBetween;
        public bool MoonObjectsOrbitRandomDirection;

        public Vector3 ArtificalSatelliteObjectsOrbitPosition;
        public Vector3 ArtificalSatelliteObjectsOrbitSpread;
        public Vector2 ArtificalSatelliteObjectsOrbitSpeedBetween;
        public bool ArtificalSatelliteObjectsOrbitRandomDirection;

        public List<string> AllowedSatelliteIncidents;

        public int TotalCrashingAsteroidObjects {
            get {
                return (int) (SatelliteDefOf.Satellite.TotalSatelliteObjects * SatelliteDefOf.Satellite.CrashingAsteroidObjectPercentage);
            }
        }

        public Vector3 OrbitPosition(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return AsteroidObjectsOrbitPosition;
                case Satellite_Type.Moon:
                    return MoonObjectsOrbitPosition;
                case Satellite_Type.Artifical_Satellite:
                    return ArtificalSatelliteObjectsOrbitPosition;
                default:
                    return new Vector3();
            }
        }

        public Vector3 OrbitSpread(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return AsteroidObjectsOrbitSpread;
                case Satellite_Type.Moon:
                    return MoonObjectsOrbitSpread;
                case Satellite_Type.Artifical_Satellite:
                    return ArtificalSatelliteObjectsOrbitSpread;
                default:
                    return new Vector3();
            }
        }

        public float OrbitSpeed(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return Rand.Range(AsteroidObjectsOrbitSpeedBetween.x, AsteroidObjectsOrbitSpeedBetween.y);
                case Satellite_Type.Moon:
                    return Rand.Range(MoonObjectsOrbitSpeedBetween.x, MoonObjectsOrbitSpeedBetween.y);
                case Satellite_Type.Artifical_Satellite:
                    return Rand.Range(ArtificalSatelliteObjectsOrbitSpeedBetween.x, ArtificalSatelliteObjectsOrbitSpeedBetween.y);
                default:
                    return 1.0f;
            }
        }

        public bool OrbitRandomDirection(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid:
                    return AsteroidObjectsOrbitRandomDirection;
                case Satellite_Type.Moon:
                    return MoonObjectsOrbitRandomDirection;
                case Satellite_Type.Artifical_Satellite:
                    return ArtificalSatelliteObjectsOrbitRandomDirection;
                default:
                    return false;
            }
        }
    }
}
