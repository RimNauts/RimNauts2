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
        public static Objects.NEO neo(this Type type) {
            switch (type) {
                case Type.Asteroid:
                    return new Objects.Asteroid();
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

        public static string texture_path(this Type type) {
            switch (type) {
                case Type.Asteroid:
                    return Objects.AsteroidConfig.texture_paths.RandomElement();
                case Type.AsteroidOre:
                    return Objects.AsteroidConfig.texture_paths.RandomElement();
                case Type.AsteroidCrashing:
                    return Objects.AsteroidConfig.texture_paths.RandomElement();
                case Type.Moon:
                    return Objects.MoonConfig.texture_paths.RandomElement();
                case Type.Satellite:
                    return Objects.SatelliteConfig.texture_paths.RandomElement();
                case Type.SpaceStation:
                    return Objects.SpaceStationConfig.texture_paths.RandomElement();
                default:
                    return Objects.AsteroidConfig.texture_paths[0];
            }
        }

        public static Vector3 orbit_position(this Type type) {
            Vector3 orbit_position;
            Vector3 orbit_spread;
            switch (type) {
                case Type.Asteroid:
                    orbit_position = Objects.AsteroidConfig.orbit_position;
                    orbit_spread = Objects.AsteroidConfig.orbit_spread;
                    break;
                case Type.AsteroidOre:
                    orbit_position = Objects.AsteroidConfig.orbit_position;
                    orbit_spread = Objects.AsteroidConfig.orbit_spread;
                    break;
                case Type.AsteroidCrashing:
                    orbit_position = Objects.AsteroidConfig.orbit_position;
                    orbit_spread = Objects.AsteroidConfig.orbit_spread;
                    break;
                case Type.Moon:
                    orbit_position = Objects.MoonConfig.orbit_position;
                    orbit_spread = Objects.MoonConfig.orbit_spread;
                    break;
                case Type.Satellite:
                    orbit_position = Objects.SatelliteConfig.orbit_position;
                    orbit_spread = Objects.SatelliteConfig.orbit_spread;
                    break;
                case Type.SpaceStation:
                    orbit_position = Objects.SpaceStationConfig.orbit_position;
                    orbit_spread = Objects.SpaceStationConfig.orbit_spread;
                    break;
                default:
                    orbit_position = new Vector3(250.0f, 0.0f, 250.0f);
                    orbit_spread = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
            }
            return new Vector3 {
                x = orbit_position.x + (float) ((Rand.Value - 0.5f) * (orbit_position.x * orbit_spread.x)),
                y = Rand.Range(Math.Abs(orbit_position.y) * -1, Math.Abs(orbit_position.y)),
                z = orbit_position.z + (float) ((Rand.Value - 0.5f) * (orbit_position.z * orbit_spread.z)),
            };
        }

        public static Vector3 orbit_spread(this Type type) {
            switch (type) {
                case Type.Asteroid:
                    return Objects.AsteroidConfig.orbit_spread;
                case Type.AsteroidOre:
                    return Objects.AsteroidConfig.orbit_spread;
                case Type.AsteroidCrashing:
                    return Objects.AsteroidConfig.orbit_spread;
                case Type.Moon:
                    return Objects.MoonConfig.orbit_spread;
                case Type.Satellite:
                    return Objects.SatelliteConfig.orbit_spread;
                case Type.SpaceStation:
                    return Objects.SpaceStationConfig.orbit_spread;
                default:
                    return new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        public static float orbit_speed(this Type type) {
            Vector2 speed_between;
            switch (type) {
                case Type.Asteroid:
                    speed_between = Objects.AsteroidConfig.orbit_speed_between;
                    break;
                case Type.AsteroidOre:
                    speed_between = Objects.AsteroidConfig.orbit_speed_between;
                    break;
                case Type.AsteroidCrashing:
                    speed_between = Objects.AsteroidConfig.orbit_speed_between;
                    break;
                case Type.Moon:
                    speed_between = Objects.MoonConfig.orbit_speed_between;
                    break;
                case Type.Satellite:
                    speed_between = Objects.SatelliteConfig.orbit_speed_between;
                    break;
                case Type.SpaceStation:
                    speed_between = Objects.SpaceStationConfig.orbit_speed_between;
                    break;
                default:
                    speed_between = new Vector2(1.0f, 1.0f);
                    break;
            }
            return Rand.Range(speed_between.x, speed_between.y);
        }

        public static float size(this Type type) {
            Vector2 size_between;
            switch (type) {
                case Type.Asteroid:
                    size_between = Objects.AsteroidConfig.size_between;
                    break;
                case Type.AsteroidOre:
                    size_between = Objects.AsteroidConfig.size_between;
                    break;
                case Type.AsteroidCrashing:
                    size_between = Objects.AsteroidConfig.size_between;
                    break;
                case Type.Moon:
                    size_between = Objects.MoonConfig.size_between;
                    break;
                case Type.Satellite:
                    size_between = Objects.SatelliteConfig.size_between;
                    break;
                case Type.SpaceStation:
                    size_between = Objects.SpaceStationConfig.size_between;
                    break;
                default:
                    size_between = new Vector2(1.0f, 1.0f);
                    break;
            }
            return Rand.Range(size_between.x, size_between.y) * 20 * Find.WorldGrid.averageTileSize;
        }

        public static float color(this Type type) {
            Vector2 color_between;
            switch (type) {
                case Type.Asteroid:
                    color_between = Objects.AsteroidConfig.color_between;
                    break;
                case Type.AsteroidOre:
                    color_between = Objects.AsteroidConfig.color_between;
                    break;
                case Type.AsteroidCrashing:
                    color_between = Objects.AsteroidConfig.color_between;
                    break;
                case Type.Moon:
                    color_between = Objects.MoonConfig.color_between;
                    break;
                case Type.Satellite:
                    color_between = Objects.SatelliteConfig.color_between;
                    break;
                case Type.SpaceStation:
                    color_between = Objects.SpaceStationConfig.color_between;
                    break;
                default:
                    color_between = new Vector2(1.0f, 1.0f);
                    break;
            }
            return Rand.Range(color_between.x, color_between.y);
        }

        public static float rotation_angle(this Type type) {
            bool random_rotation;
            switch (type) {
                case Type.Asteroid:
                    random_rotation = Objects.AsteroidConfig.random_rotation;
                    break;
                case Type.AsteroidOre:
                    random_rotation = Objects.AsteroidConfig.random_rotation;
                    break;
                case Type.AsteroidCrashing:
                    random_rotation = Objects.AsteroidConfig.random_rotation;
                    break;
                case Type.Moon:
                    random_rotation = Objects.MoonConfig.random_rotation;
                    break;
                case Type.Satellite:
                    random_rotation = Objects.SatelliteConfig.random_rotation;
                    break;
                case Type.SpaceStation:
                    random_rotation = Objects.SpaceStationConfig.random_rotation;
                    break;
                default:
                    random_rotation = false;
                    break;
            }
            return random_rotation ? UnityEngine.Random.value * 360.0f : 270.0f;
        }

        public static int orbit_direction(this Type type) {
            bool random_direction;
            switch (type) {
                case Type.Asteroid:
                    random_direction = Objects.AsteroidConfig.random_direction;
                    break;
                case Type.AsteroidOre:
                    random_direction = Objects.AsteroidConfig.random_direction;
                    break;
                case Type.AsteroidCrashing:
                    random_direction = Objects.AsteroidConfig.random_direction;
                    break;
                case Type.Moon:
                    random_direction = Objects.MoonConfig.random_direction;
                    break;
                case Type.Satellite:
                    random_direction = Objects.SatelliteConfig.random_direction;
                    break;
                case Type.SpaceStation:
                    random_direction = Objects.SpaceStationConfig.random_direction;
                    break;
                default:
                    random_direction = false;
                    break;
            }
            return random_direction && Rand.Bool ? 1 : -1;
        }
    }
}
