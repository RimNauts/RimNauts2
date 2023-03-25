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
        public static Objects.NEO neo(
            this Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            float? transformation_rotation_angle = null,
            Vector3? current_position = null
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
                        transformation_rotation_angle,
                        current_position
                    );
                case Type.AsteroidOre:
                    return new Objects.AsteroidOre(
                        texture_path,
                        orbit_position,
                        orbit_speed,
                        draw_size,
                        period,
                        time_offset,
                        orbit_direction,
                        color,
                        rotation_angle,
                        transformation_rotation_angle,
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
                        transformation_rotation_angle,
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
                        transformation_rotation_angle,
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
                        transformation_rotation_angle,
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
                        transformation_rotation_angle,
                        current_position
                    );
                default:
                    return null;
            }
        }

        public static Defs.ObjectMetadata defs(this Type type) {
            switch (type) {
                case Type.AsteroidOre:
                    return Defs.Loader.get_object_metadata(Type.Asteroid);
                case Type.AsteroidCrashing:
                    return Defs.Loader.get_object_metadata(Type.Asteroid);
                default:
                    return Defs.Loader.get_object_metadata(type);
            }
        }

        public static bool object_holder(this Type type) {
            return type.defs().object_holder;
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
            return Rand.Range(size_between.x, size_between.y) * 10;
        }

        public static float color(this Type type) {
            Vector2 color_between = type.defs().color_between;
            return Rand.Range(color_between.x, color_between.y);
        }

        public static float rotation_angle(this Type type) {
            return type.defs().random_rotation ? UnityEngine.Random.value * 360.0f : 270.0f;
        }

        public static float transformation_rotation_angle(this Type type) {
            return type.defs().random_transformation_rotation ? (UnityEngine.Random.value * 180.0f) - 90.0f : 0.0f;
        }

        public static int orbit_direction(this Type type) {
            return type.defs().random_direction && Rand.Bool ? 1 : -1;
        }

        public static bool trail(this Type type) {
            return type.defs().trail;
        }

        public static float trail_width(this Type type) {
            return type.defs().trail_width;
        }

        public static float trail_length(this Type type) {
            return type.defs().trail_length;
        }

        public static Color? trail_color(this Type type) {
            return type.defs().trail_color;
        }

        public static float trail_brightness(this Type type) {
            return type.defs().trail_brightness;
        }

        public static float trail_transparency(this Type type) {
            return type.defs().trail_transparency;
        }
    }
}
