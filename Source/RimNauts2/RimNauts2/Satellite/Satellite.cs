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
        public bool can_out_of_bounds = false;
        public float out_of_bounds_offset = 1.0f;
        public float current_out_of_bounds;
        public bool out_of_bounds_direction_towards_surface = true;
        public int orbit_random_direction;
        public bool mineral_rich = false;
        public int mineral_rich_transform_wait;
        public int mineral_rich_abondon;
        public bool currently_mineral_rich = false;

        public override void PostAdd() {
            base.PostAdd();
            foreach (var biome_def in DefDatabase<RimWorld.BiomeDef>.AllDefsListForReading) {
                if (biome_def.defName == def.defName + "_Biome") {
                    Find.World.grid.tiles.ElementAt(Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(def.defName + "_Biome");
                    break;
                }
            }
        }

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
            Scribe_Values.Look(ref can_out_of_bounds, "can_out_of_bounds");
            Scribe_Values.Look(ref out_of_bounds_offset, "out_of_bounds_offset");
            Scribe_Values.Look(ref current_out_of_bounds, "current_out_of_bounds");
            Scribe_Values.Look(ref out_of_bounds_direction_towards_surface, "out_of_bounds_direction_towards_surface");
            Scribe_Values.Look(ref orbit_random_direction, "orbit_random_direction");
            Scribe_Values.Look(ref mineral_rich, "mineral_rich");
            Scribe_Values.Look(ref mineral_rich_transform_wait, "mineral_rich_transform_wait");
            Scribe_Values.Look(ref mineral_rich_abondon, "mineral_rich_abondon");
            Scribe_Values.Look(ref currently_mineral_rich, "currently_mineral_rich");
        }

        public override Vector3 DrawPos => get_parametric_ellipse();

        public override void Tick() {
            base.Tick();
            if (type == Satellite_Type.Asteroid && can_out_of_bounds) {
                if (out_of_bounds_direction_towards_surface) {
                    current_out_of_bounds -= 0.00015f;
                    if (current_out_of_bounds <= 0.42f) current_out_of_bounds = out_of_bounds_offset;
                } else {
                    current_out_of_bounds += 0.0002f;
                    if (current_out_of_bounds >= 2.4f) current_out_of_bounds = out_of_bounds_offset;
                }
            }
            if (mineral_rich) {
                if (!currently_mineral_rich) {
                    if (Settings.MineralAsteroidsToggle) {
                        if (mineral_rich_transform_wait <= 0) {
                            currently_mineral_rich = true;
                            mineral_rich_transform_wait = SatelliteDefOf.Satellite.MineralAppearWait;
                            mineral_rich_abondon = SatelliteDefOf.Satellite.MineralAbondonWait;
                            Ore.generate_ore(this);
                            if (Settings.MineralAsteroidsVerboseToggle) Find.LetterStack.ReceiveLetter(SatelliteDefOf.Satellite.AsteroidOreAppearLabel, SatelliteDefOf.Satellite.AsteroidOreAppearMessage, RimWorld.LetterDefOf.NeutralEvent, null);
                        } else mineral_rich_transform_wait--;
                    }
                } else {
                    if (mineral_rich_abondon <= 0) {
                        if (!HasMap) {
                            currently_mineral_rich = false;
                            mineral_rich_transform_wait = SatelliteDefOf.Satellite.MineralAppearWait;
                            mineral_rich_abondon = SatelliteDefOf.Satellite.MineralAbondonWait;
                            Destroy();
                        }
                    } else mineral_rich_abondon--;
                }
            }
        }

        public Vector3 get_parametric_ellipse() {
            float crash_course = get_crash_course();
            float time = orbit_speed * orbit_random_direction * Find.TickManager.TicksGame + time_offset;
            //if (out_of_bounds_direction_towards_surface || crash_course <= 1.0f) time *= crash_course * -1 + 2;
            return new Vector3 {
                x = (orbit_position.x - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Cos(6.28f / period * time) * crash_course,
                y = orbit_position.y,
                z = (orbit_position.z - (Math.Abs(orbit_position.y) / 2)) * (float) Math.Sin(6.28f / period * time) * crash_course,
            };
        }

        public float get_crash_course() {
            if (Settings.CrashingAsteroidsToggle && can_out_of_bounds) {
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
                    } else if (Generate_Satellites.mineral_asteroids_in_world < SatelliteDefOf.Satellite.TotalMineralAsteroidObjects) {
                        mineral_rich = true;
                        mineral_rich_transform_wait = SatelliteDefOf.Satellite.MineralAppearWait;
                        mineral_rich_abondon = SatelliteDefOf.Satellite.MineralAbondonWait;
                        Generate_Satellites.mineral_asteroids_in_world++;
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
            if (type == Satellite_Type.Moon && HasMap) {
                SatelliteSettings satellite_settings = Generate_Satellites.copy_satellite(this, def_name.Substring(0, def_name.Length - "_Base".Length));
                Satellite new_satellite = Generate_Satellites.paste_satellite(satellite_settings);
                if (!SatelliteContainer.exists(new_satellite.Tile)) {
                    SatelliteContainer.add(new_satellite);
                }
            } else if (type == Satellite_Type.None) {
                Find.World.grid.tiles.ElementAt(Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("Ocean");
            } else if (type == Satellite_Type.Buffer) {
                // nothing
            } else if (type == Satellite_Type.Asteroid_Ore) {
                SatelliteSettings satellite_settings = Generate_Satellites.copy_satellite(this, Satellite_Type_Methods.WorldObjects(Satellite_Type.Asteroid).RandomElement(), Satellite_Type.Asteroid);
                satellite_settings.currently_mineral_rich = false;
                Satellite new_satellite = Generate_Satellites.paste_satellite(satellite_settings);
                if (!SatelliteContainer.exists(new_satellite.Tile)) {
                    SatelliteContainer.add(new_satellite);
                }
            } else Generate_Satellites.add_satellite(Tile, Satellite_Type.Asteroid);
        }
    }
}
