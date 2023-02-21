using UnityEngine;

namespace RimNauts2.World.Objects {
    public static class AsteroidConfig {
        public static string[] texture_paths = new string[] {
            "Satellites/Asteroids/RimNauts2_Full_Asteroid_1",
            "Satellites/Asteroids/RimNauts2_Full_Asteroid_2",
            "Satellites/Asteroids/RimNauts2_Full_Asteroid_3",
        };
        public static Vector3 orbit_position = new Vector3(250.0f, 5.0f, 250.0f);
        public static Vector3 orbit_spread = new Vector3(0.3f, 0.1f, 0.3f);
        public static Vector2 orbit_speed_between = new Vector2(2.0f, 4.0f);
        public static Vector2 size_between = new Vector2(0.2f, 0.3f);
        public static Vector2 color_between = new Vector2(0.6f, 0.8f);
        public static bool random_rotation = true;
        public static bool random_direction = false;
    }

    public static class MoonConfig {
        public static string[] texture_paths = new string[] {
            "Satellites/Moons/RimNauts2_MoonBarren",
            "Satellites/Moons/RimNauts2_MoonStripped",
            "Satellites/Moons/RimNauts2_MoonWater",
        };
        public static Vector3 orbit_position = new Vector3(350.0f, 200.0f, 350.0f);
        public static Vector3 orbit_spread = new Vector3(0.25f, 0.0f, 0.25f);
        public static Vector2 orbit_speed_between = new Vector2(1.0f, 1.0f);
        public static Vector2 size_between = new Vector2(1.0f, 1.5f);
        public static Vector2 color_between = new Vector2(1.0f, 1.0f);
        public static bool random_rotation = false;
        public static bool random_direction = true;
    }

    public static class SatelliteConfig {
        public static string[] texture_paths = new string[] {
            "Satellites/ArtificalSatellites/RimNauts2_ArtificialSatellite",
        };
        public static Vector3 orbit_position = new Vector3(140.0f, 100.0f, 140.0f);
        public static Vector3 orbit_spread = new Vector3(0.1f, 0.0f, 0.1f);
        public static Vector2 orbit_speed_between = new Vector2(8.0f, 10.0f);
        public static Vector2 size_between = new Vector2(0.5f, 0.5f);
        public static Vector2 color_between = new Vector2(1.0f, 1.0f);
        public static bool random_rotation = true;
        public static bool random_direction = true;
    }

    public static class SpaceStationConfig {
        public static string[] texture_paths = new string[] {
            "Satellites/ArtificalSatellites/RimNauts2_SpaceStation",
        };
        public static Vector3 orbit_position = new Vector3(160.0f, 100.0f, 160.0f);
        public static Vector3 orbit_spread = new Vector3(0.1f, 0.0f, 0.1f);
        public static Vector2 orbit_speed_between = new Vector2(6.0f, 8.0f);
        public static Vector2 size_between = new Vector2(0.8f, 0.8f);
        public static Vector2 color_between = new Vector2(1.0f, 1.0f);
        public static bool random_rotation = true;
        public static bool random_direction = false;
    }
}
