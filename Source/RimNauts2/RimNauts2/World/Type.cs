using System;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    public enum Type {
        None = 0,
        Asteroid = 1,
        AsteroidOre = 2,
        AsteroidCrashing = 3,
        Moon = 4,
        Satellite = 5,
        SpaceStation = 6,
    }

    public static class TypeExtension {
        public static Defs.ObjectMetadata asteroid_defs = (Defs.ObjectMetadata) GenDefDatabase.GetDef(
            typeof(Defs.ObjectMetadata),
            "RimNauts2_Object_Metadata_Asteroid"
        );
        public static Defs.ObjectMetadata moon_defs = (Defs.ObjectMetadata) GenDefDatabase.GetDef(
            typeof(Defs.ObjectMetadata),
            "RimNauts2_Object_Metadata_Moon"
        );
        public static Defs.ObjectMetadata satellite_defs = (Defs.ObjectMetadata) GenDefDatabase.GetDef(
            typeof(Defs.ObjectMetadata),
            "RimNauts2_Object_Metadata_Satellite"
        );
        public static Defs.ObjectMetadata space_station_defs = (Defs.ObjectMetadata) GenDefDatabase.GetDef(
            typeof(Defs.ObjectMetadata),
            "RimNauts2_Object_Metadata_SpaceStation"
        );

        public static Objects.NEO neo(this Type type) {
            switch (type) {
                case Type.Asteroid:
                    return new Objects.Asteroid();
                case Type.AsteroidCrashing:
                    return new Objects.AsteroidCrashing();
                case Type.Moon:
                    return new Objects.Moon();
                case Type.Satellite:
                    return new Objects.Satellite();
                case Type.SpaceStation:
                    return new Objects.SpaceStation();
                default:
                    return null;
            }
        }

        public static Objects.NEO neo(
            this Type type,
            string texture_path,
            Vector3 orbit_position,
            float orbit_speed,
            Vector3 draw_size,
            int period,
            int time_offset,
            int orbit_direction,
            float color,
            float rotation_angle,
            Vector3 current_position
        ) {
            switch (type) {
                case Type.Asteroid:
                    return new Objects.Asteroid(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        current_position
                    );
                case Type.AsteroidCrashing:
                    return new Objects.AsteroidCrashing(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        current_position
                    );
                case Type.Moon:
                    return new Objects.Moon(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        current_position
                    );
                case Type.Satellite:
                    return new Objects.Satellite(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        current_position
                    );
                case Type.SpaceStation:
                    return new Objects.SpaceStation(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        current_position
                    );
                default:
                    return null;
            }
        }

        public static Defs.ObjectMetadata defs(this Type type) {
            switch (type) {
                case Type.Asteroid:
                    return asteroid_defs;
                case Type.AsteroidOre:
                    return asteroid_defs;
                case Type.AsteroidCrashing:
                    return asteroid_defs;
                case Type.Moon:
                    return moon_defs;
                case Type.Satellite:
                    return satellite_defs;
                case Type.SpaceStation:
                    return space_station_defs;
                default:
                    return asteroid_defs;
            }
        }

        public static string texture_path(this Type type) {
            return type.defs().texture_paths.RandomElement();
        }

        public static Vector3 orbit_position(this Type type) {
            Vector3 orbit_position = type.defs().orbit_position;
            Vector3 orbit_spread = type.defs().orbit_spread;
            return new Vector3 {
                x = orbit_position.x + (float) ((Rand.Value - 0.5f) * (orbit_position.x * orbit_spread.x)),
                y = Rand.Range(Math.Abs(orbit_position.y) * -1, Math.Abs(orbit_position.y)),
                z = orbit_position.z + (float) ((Rand.Value - 0.5f) * (orbit_position.z * orbit_spread.z)),
            };
        }

        public static float orbit_speed(this Type type) {
            Vector2 speed_between = type.defs().orbit_speed_between;
            return Rand.Range(speed_between.x, speed_between.y);
        }

        public static float size(this Type type) {
            Vector2 size_between = type.defs().size_between;
            return Rand.Range(size_between.x, size_between.y) * 20 * Find.WorldGrid.averageTileSize;
        }

        public static float color(this Type type) {
            Vector2 color_between = type.defs().color_between;
            return Rand.Range(color_between.x, color_between.y);
        }

        public static float rotation_angle(this Type type) {
            return type.defs().random_rotation ? UnityEngine.Random.value * 360.0f : 270.0f;
        }

        public static int orbit_direction(this Type type) {
            return type.defs().random_direction && Rand.Bool ? 1 : -1;
        }
    }
}
