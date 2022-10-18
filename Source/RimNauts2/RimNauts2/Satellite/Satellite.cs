using System;
using System.Reflection;
using UnityEngine;
using Verse;
using System.Linq;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public class Satellite : RimWorld.Planet.MapParent {
        public string def_name;
        public Satellite_Type type;
        public Vector3 max_orbits;
        public Vector3 shift_orbits;
        public Vector3 spread;
        public float period;
        public int time_offset;
        public float speed;
        public bool has_map = false;

        public override Vector3 DrawPos => get_parametric_ellipse();

        public Vector3 get_parametric_ellipse() {
            int time = Find.TickManager.TicksGame;
            Vector3 vec3 = new Vector3 {
                x = max_orbits.x * (float) Math.Cos(6.28f / period * ((speed * time) + time_offset)) + shift_orbits.x,
                z = max_orbits.z * (float) Math.Sin(6.28f / period * ((speed * time) + time_offset)) + shift_orbits.z,
                y = max_orbits.y * (float) Math.Cos(6.28f / period * ((speed * time) + time_offset)) + shift_orbits.y,
            };
            return vec3;
        }

        public void set_default_values(Satellite_Type new_type) {
            type = new_type;
            switch (type) {
                case Satellite_Type.Asteroid:
                    max_orbits = new Vector3(250.0f, 0.0f, 250.0f);
                    shift_orbits = new Vector3(0.0f, 0.0f, 0.0f);
                    spread = new Vector3(0.25f, 0.25f, 0.25f);
                    speed = Rand.Range(1.0f, 2.0f);
                    break;
                case Satellite_Type.Moon:
                    max_orbits = new Vector3(400.0f, 0.0f, 400.0f);
                    shift_orbits = new Vector3(0.0f, 0.0f, 0.0f);
                    spread = new Vector3(0.25f, 0.25f, 0.25f);
                    speed = 1.0f;
                    break;
                default:
                    Log.Error("RimNauts2: Failed to set default values in satellite.");
                    return;
            }
            max_orbits = randomize_vector(max_orbits);
            shift_orbits = randomize_vector(shift_orbits);
            period = random_orbit(36000.0f, 6000.0f);
            time_offset = Rand.Range(0, (int) period);
        }

        public int random_orbit(float min, float range) => (int) (min + (range * (Rand.Value - 0.5f)));

        public Vector3 randomize_vector(Vector3 vec, bool rand_direction = false) {
            return new Vector3 {
                x = (Rand.Bool && rand_direction ? 1 : -1) * vec.x + (float) ((Rand.Value - 0.5f) * (vec.x * spread.x)),
                y = (Rand.Bool && rand_direction ? 1 : -1) * vec.y + (float) ((Rand.Value - 0.5f) * (vec.y * spread.y)),
                z = (Rand.Bool && rand_direction ? 1 : -1) * vec.z + (float) ((Rand.Value - 0.5f) * (vec.z * spread.z)),
            };
        }

        public override void Print(LayerSubMesh subMesh) {
            RimWorld.Planet.WorldRendererUtility.PrintQuadTangentialToPlanet(
                DrawPos,
                10f * Find.WorldGrid.averageTileSize,
                0.008f,
                subMesh,
                false,
                false,
                true
            );
        }

        public override void Draw() {
            RimWorld.Planet.WorldRendererUtility.DrawQuadTangentialToPlanet(
                DrawPos,
                10f * Find.WorldGrid.averageTileSize,
                0.008f,
                Material,
                false,
                false,
                null
            );
        }

        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject) {
            alsoRemoveWorldObject = true;
            if ((from ob in Find.World.worldObjects.AllWorldObjects
                 where ob is RimWorld.Planet.TravelingTransportPods pods && ((int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ob) == Tile || pods.destinationTile == Tile)
                 select ob).Count() > 0) {
                return false;
            }
            return base.ShouldRemoveMapNow(out alsoRemoveWorldObject);
        }

        public override void PostRemove() {
            int buffer_tile_id = Tile;
            string buffer_def_name = def_name;
            Satellite_Type buffer_type = type;
            Vector3 buffer_max_orbits = max_orbits;
            Vector3 buffer_shift_orbits = shift_orbits;
            Vector3 buffer_spread = spread;
            float buffer_period = period;
            int buffer_time_offset = time_offset;
            float buffer_speed = speed;
            if (type == Satellite_Type.Moon) Find.World.grid.tiles.ElementAt(Tile).biome = BiomeDefOf.SatelliteBiome;
            SatelliteContainer.remove(this);
            base.PostRemove();
            if (type == Satellite_Type.Moon) _ = Generate_Satellites.copy_satellite(
                buffer_tile_id,
                buffer_def_name.Substring(0, buffer_def_name.Length - 5),
                buffer_type,
                buffer_max_orbits,
                buffer_shift_orbits,
                buffer_spread,
                buffer_period,
                buffer_time_offset,
                buffer_speed
            );
        }
    }

    public enum Satellite_Type {
        None = 0,
        Asteroid = 1,
        Moon = 2,
    }
}
