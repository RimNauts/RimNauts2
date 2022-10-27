using System.Collections.Generic;
using Verse;
using UnityEngine;
using System;

namespace RimNauts2 {
    public class SatelliteDef : Def {
        public float CrashingAsteroidObjectPercentage;
        public List<string> AsteroidObjects;
        public List<string> AsteroidOreObjects;
        public List<string> WasteObjects;
        public List<string> MoonObjects;
        public List<string> ArtificalSatelliteObjects;
        public List<string> SpaceStationObjects;

        public float MineralRichAsteroidsPercentage;
        public Vector2 MineralRichAsteroidsRandomWaitTicks;
        public Vector2 MineralRichAsteroidsRandomInWorldTicks;

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

        public Vector3 SpaceStationObjectsOrbitPosition;
        public Vector3 SpaceStationObjectsOrbitSpread;
        public Vector2 SpaceStationObjectsOrbitSpeedBetween;
        public bool SpaceStationObjectsOrbitRandomDirection;

        public List<string> AllowedSatelliteIncidents;

        public int TotalCrashingAsteroidObjects {
            get {
                if (SatelliteDefOf.Satellite.CrashingAsteroidObjectPercentage <= 0) return 0;
                return (int) (Settings.TotalSatelliteObjects * SatelliteDefOf.Satellite.CrashingAsteroidObjectPercentage);
            }
        }

        public int TotalMineralAsteroidObjects {
            get {
                if (SatelliteDefOf.Satellite.MineralRichAsteroidsPercentage <= 0) return 0;
                return (int) Math.Max(1.0f, Settings.TotalSatelliteObjects * SatelliteDefOf.Satellite.MineralRichAsteroidsPercentage);
            }
        }

        public int MineralAppearWait {
            get {
                if (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) > 0) {
                    float diff = 1 - (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) / 100);
                    return (int) (diff * Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.y));
                } else {
                    return (int) Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomWaitTicks.y);
                }
            }
        }

        public int MineralAbondonWait {
            get {
                if (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) > 0) {
                    float diff = 1 + (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) / 100);
                    return (int) (diff * Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.y));
                } else {
                    return (int) Rand.Range(SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.x, SatelliteDefOf.Satellite.MineralRichAsteroidsRandomInWorldTicks.y);
                }
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
                case Satellite_Type.Space_Station:
                    return SpaceStationObjectsOrbitPosition;
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
                case Satellite_Type.Space_Station:
                    return SpaceStationObjectsOrbitSpread;
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
                case Satellite_Type.Space_Station:
                    return Rand.Range(SpaceStationObjectsOrbitSpeedBetween.x, SpaceStationObjectsOrbitSpeedBetween.y);
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
                case Satellite_Type.Space_Station:
                    return SpaceStationObjectsOrbitRandomDirection;
                default:
                    return false;
            }
        }

        public IntVec3 MapSize(Satellite_Type type) {
            switch (type) {
                case Satellite_Type.Asteroid_Ore:
                    return new IntVec3(100, 1, 100);
                default:
                    return new IntVec3(250, 1, 250);
            }
        }
    }
}
