using System;
using System.Reflection;
using UnityEngine;
using Verse;
using System.Linq;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public class Satellite : RimWorld.Planet.MapParent {
        readonly SatelliteDef SatelliteCore = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public Vector3 max_orbits;
        public Vector3 shift_orbits;
        public Map map;
        public float period;
        public RimWorld.Planet.Tile real_tile;
        public int time_offset = 0;
        public bool is_moon = false;

        public override Vector3 DrawPos {
            get {
                return get_parametric_ellipse();
            }
        }

        public Vector3 get_parametric_ellipse() {
            int time = Find.TickManager.TicksGame;
            Vector3 vec3 = new Vector3 {
                x = max_orbits.x * (float) Math.Cos(6.28f / period * (time + time_offset)) + shift_orbits.x,
                z = max_orbits.z * (float) Math.Sin(6.28f / period * (time + time_offset)) + shift_orbits.z,
                y = max_orbits.y * (float) Math.Cos(6.28f / period * (time + time_offset)) + shift_orbits.y
            };
            return vec3;
        }

        public override void PostAdd() {
            max_orbits = randomize_vector(SatelliteCore.getMaxOrbits, true);
            shift_orbits = randomize_vector(SatelliteCore.getShiftOrbits, true);
            period = (int) random_orbit(SatelliteCore.getOrbitPeriod, SatelliteCore.getOrbitPeriodVar);
            time_offset = Rand.Range(0, (int) (period / 2));
            base.PostAdd();
        }

        public float random_orbit(float min, float range) {
            return min + (range * (Rand.Value - 0.5f));
        }

        public Vector3 randomize_vector(Vector3 vec, bool rand_direction) {
            return new Vector3 {
                x = (Rand.Bool && rand_direction ? 1 : -1) * vec.x + (float) ((Rand.Value - 0.5f) * (vec.x * 0.25)),
                y = (Rand.Bool && rand_direction ? 1 : -1) * vec.y + (float) ((Rand.Value - 0.5f) * (vec.y * 0.25)),
                z = (Rand.Bool && rand_direction ? 1 : -1) * vec.z + (float) ((Rand.Value - 0.5f) * (vec.z * 0.25))
            };
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref period, "period", 0, false);
            Scribe_Values.Look(ref time_offset, "timeOffset", 0, false);
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
            SatelliteTiles_Utilities.remove_satellite(this);
            base.PostRemove();
        }
    }
}
