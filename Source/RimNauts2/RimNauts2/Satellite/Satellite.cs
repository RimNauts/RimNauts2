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
        public Vector3 orbit_position;
        public Vector3 orbit_spread;
        public float orbit_speed;
        public float period;
        public int time_offset;
        public bool has_map = false;
        public bool can_out_of_bounds = false;
        public float out_of_bounds_offset = 1.0f;
        public float current_out_of_bounds;
        public bool out_of_bounds_direction_towards_surface = true;
        public int orbit_random_direction;

        public override bool ExpandMore => def.expandMore;

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref def_name, "def_name");
            Scribe_Values.Look(ref type, "type");
            Scribe_Values.Look(ref orbit_position, "orbit_position");
            Scribe_Values.Look(ref orbit_spread, "orbit_spread");
            Scribe_Values.Look(ref orbit_speed, "orbit_speed");
            Scribe_Values.Look(ref period, "period");
            Scribe_Values.Look(ref time_offset, "time_offset");
            Scribe_Values.Look(ref has_map, "has_map");
            Scribe_Values.Look(ref can_out_of_bounds, "can_out_of_bounds");
            Scribe_Values.Look(ref out_of_bounds_offset, "out_of_bounds_offset");
            Scribe_Values.Look(ref current_out_of_bounds, "current_out_of_bounds");
            Scribe_Values.Look(ref out_of_bounds_direction_towards_surface, "out_of_bounds_direction_towards_surface");
            Scribe_Values.Look(ref orbit_random_direction, "orbit_random_direction");
        }

        public override Vector3 DrawPos => get_parametric_ellipse();

        public override void Tick() {
            base.Tick();
            if (can_out_of_bounds) {
                if (out_of_bounds_direction_towards_surface) {
                    current_out_of_bounds -= 0.00015f;
                    if (current_out_of_bounds <= 0.42f) current_out_of_bounds = out_of_bounds_offset;
                } else {
                    current_out_of_bounds += 0.0002f;
                    if (current_out_of_bounds >= 2.4f) current_out_of_bounds = out_of_bounds_offset;
                }
            }
        }

        public Vector3 get_parametric_ellipse() {
            float crash_course = get_crash_course();
            float time = orbit_speed * orbit_random_direction * Find.TickManager.TicksGame + time_offset;
            if (out_of_bounds_direction_towards_surface || crash_course <= 1.0f) time *= crash_course * -1 + 2;
            return new Vector3 {
                x = (orbit_position.x - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Cos(6.28f / period * time) * crash_course,
                y = orbit_position.y,
                z = (orbit_position.z - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Sin(6.28f / period * time) * crash_course,
            };
        }

        public float get_crash_course() {
            if (can_out_of_bounds) {
                if (out_of_bounds_direction_towards_surface) {
                    return Math.Min(1.0f, current_out_of_bounds);
                } else {
                    return Math.Max(1.0f, current_out_of_bounds);
                }
            }
            return 1.0f;
        }

        public void set_default_values(Satellite_Type new_type) {
            type = new_type;
            switch (type) {
                case Satellite_Type.Asteroid:
                    if (Generate_Satellites.crashing_asteroids_in_world < SatelliteDefOf.Satellite.TotalCrashingAsteroidObjects) {
                        can_out_of_bounds = true;
                        if (Generate_Satellites.crashing_asteroids_in_world <= SatelliteDefOf.Satellite.TotalCrashingAsteroidObjects * 0.5) {
                            out_of_bounds_direction_towards_surface = false;
                            out_of_bounds_offset = Rand.Range(-10.0f, 1.0f);
                        } else out_of_bounds_offset = Rand.Range(1.0f, 10.0f);
                        Generate_Satellites.crashing_asteroids_in_world++;
                    }
                    break;
                default:
                    break;
            }
            orbit_spread = SatelliteDefOf.Satellite.OrbitSpread(type);
            orbit_position = SatelliteDefOf.Satellite.OrbitPosition(type);
            orbit_position.y = Rand.Range(Math.Abs(orbit_position.y) * -1, Math.Abs(orbit_position.y));
            orbit_position = randomize_vector(orbit_position);
            orbit_speed = SatelliteDefOf.Satellite.OrbitSpeed(type);
            period = random_orbit(36000.0f, 6000.0f);
            time_offset = Rand.Range(0, (int) period);
            current_out_of_bounds = out_of_bounds_offset;
            orbit_random_direction = Rand.Bool && SatelliteDefOf.Satellite.OrbitRandomDirection(type) ? -1 : 1;
        }

        public int random_orbit(float min, float range) => (int) (min + (range * (Rand.Value - 0.5f)));

        public Vector3 randomize_vector(Vector3 vec) {
            return new Vector3 {
                x = vec.x + (float) ((Rand.Value - 0.5f) * (vec.x * orbit_spread.x)),
                y = vec.y,
                z = vec.z + (float) ((Rand.Value - 0.5f) * (vec.z * orbit_spread.z)),
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
            base.PostRemove();
            if (type == Satellite_Type.Moon && has_map) {
                _ = Generate_Satellites.copy_satellite(this, def_name.Substring(0, def_name.Length - "_Base".Length));
            }
        }
    }
}
