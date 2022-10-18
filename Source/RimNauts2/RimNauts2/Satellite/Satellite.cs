using System;
using System.Reflection;
using UnityEngine;
using Verse;
using System.Linq;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public class Satellite : RimWorld.Planet.MapParent {
        readonly SatelliteDef SatelliteCore = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public string def_name;
        public Satellite_Type type;
        public Vector3 max_orbits;
        public Vector3 shift_orbits;
        public Vector3 spread;
        public Map map;
        public float period;
        public RimWorld.Planet.Tile real_tile;
        public int time_offset = 0;
        public float speed = Rand.Range(0.5f, 1.5f);
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

        public override void PostAdd() {
            if (spread == default) spread = SatelliteCore.getSpread;
            if (max_orbits == default) {
                if (type == Satellite_Type.Moon) {
                    max_orbits.x = 400;
                    max_orbits.y = 0;
                    max_orbits.z = 400;
                } else max_orbits = SatelliteCore.getMaxOrbits;
                max_orbits = randomize_vector(max_orbits, false);
            }
            if (shift_orbits == default) shift_orbits = randomize_vector(SatelliteCore.getShiftOrbits, false);
            if (period == default) period = (int) random_orbit(SatelliteCore.getOrbitPeriod, SatelliteCore.getOrbitPeriodVar);
            if (time_offset == default) time_offset = Rand.Range(0, (int) period);
            base.PostAdd();
        }

        public float random_orbit(float min, float range) => min + (range * (Rand.Value - 0.5f));

        public Vector3 randomize_vector(Vector3 vec, bool rand_direction) {
            return new Vector3 {
                x = (Rand.Bool && rand_direction ? 1 : -1) * vec.x + (float) ((Rand.Value - 0.5f) * (vec.x * spread.x)),
                y = (Rand.Bool && rand_direction ? 1 : -1) * vec.y + (float) ((Rand.Value - 0.5f) * (vec.y * spread.y)),
                z = (Rand.Bool && rand_direction ? 1 : -1) * vec.z + (float) ((Rand.Value - 0.5f) * (vec.z * spread.z)),
            };
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref period, "orbitPeriod", 0, false);
            Scribe_Values.Look(ref time_offset, "timeOffset", 0, false);
            Scribe_Values.Look(ref spread, "spread", default, false);
            Scribe_Values.Look(ref max_orbits, "maxOrbits", default, false);
            Scribe_Values.Look(ref shift_orbits, "shiftOrbits", default, false);
            get_instance_field(typeof(RimWorld.Planet.WorldObject), this, "BaseDrawSize");
        }

        internal static object get_instance_field(Type type, object instance, string fieldName) {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetField(fieldName, bindingAttr).GetValue(instance);
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
            if (type == Satellite_Type.Moon) {
                Satellites.has_moon_map = false;
                Satellites.rock_moon_tile = -1;
                Find.World.grid.tiles.ElementAt(Tile).biome = BiomeDefOf.SatelliteBiome;
                
            }
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
        Asteroid = 0,
        Moon = 1,
    }
}
