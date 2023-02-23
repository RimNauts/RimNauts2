using System;
using System.Reflection;
using UnityEngine;
using Verse;
using System.Linq;

namespace RimNauts2 {
    public class RimNauts_GameComponent : GameComponent {
        public RimNauts_GameComponent(Game game) : base() { }
    }

    public enum Satellite_Type {
        None = 0,
        Asteroid = 1,
        Moon = 2,
        Artifical_Satellite = 3,
        Asteroid_Ore = 4,
        Buffer = 5,
        Space_Station = 6,
    }

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
        }

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

        public override Vector3 DrawPos => Vector3.zero;

        public override void Print(LayerSubMesh subMesh) { }

        public override void Draw() { }

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
            Find.World.grid.tiles.ElementAt(Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("Ocean");
        }
    }
}
