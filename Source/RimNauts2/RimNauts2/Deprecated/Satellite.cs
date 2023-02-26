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

        public override void Tick() {
            World.ObjectHolder object_holder;
            string object_holder_def;
            Find.World.grid.tiles.ElementAt(Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("Ocean");
            switch (type) {
                case Satellite_Type.Moon:
                    object_holder_def = null;
                    if (def.mapGenerator.defName == "RimNauts2_MoonBarren_MapGen") object_holder_def = "RimNauts2_ObjectHolder_Moon_Barren";
                    if (def.mapGenerator.defName == "RimNauts2_MoonStripped_MapGen") object_holder_def = "RimNauts2_ObjectHolder_Moon_Stripped";
                    if (def.mapGenerator.defName == "RimNauts2_MoonWater_MapGen") object_holder_def = "RimNauts2_ObjectHolder_Moon_Water";
                    object_holder = World.Generator.add_object_holder(
                        type: World.Type.Moon,
                        object_holder_def: object_holder_def,
                        start_index: Tile
                    );
                    if (HasMap) {
                        Map map = Map;
                        map.info.parent = object_holder;
                        SetFaction(RimWorld.Faction.OfPlayerSilentFail);
                        object_holder.SetFaction(RimWorld.Faction.OfPlayer);
                        Find.World.WorldUpdate();
                    }
                    Destroy();
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.successfully_converted_old_object",
                        prefix: Style.name_prefix,
                        args: new NamedArgument[] { type.ToString(), Tile.ToString() }
                    );
                    break;
                case Satellite_Type.Artifical_Satellite:
                    World.Generator.add_object_holder(
                        type: World.Type.Satellite,
                        start_index: Tile
                    );
                    Destroy();
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.successfully_converted_old_object",
                        prefix: Style.name_prefix,
                        args: new NamedArgument[] { type.ToString(), Tile.ToString() }
                    );
                    break;
                case Satellite_Type.Asteroid_Ore:
                    object_holder_def = null;
                    if (def.mapGenerator.defName == "RimNauts2_OreSteel_MapGen") object_holder_def = "RimNauts2_ObjectHolder_AsteroidOre_Steel";
                    if (def.mapGenerator.defName == "RimNauts2_OreGold_MapGen") object_holder_def = "RimNauts2_ObjectHolder_AsteroidOre_Gold";
                    if (def.mapGenerator.defName == "RimNauts2_OrePlasteel_MapGen") object_holder_def = "RimNauts2_ObjectHolder_AsteroidOre_Plasteel";
                    if (def.mapGenerator.defName == "RimNauts2_OreUranium_MapGen") object_holder_def = "RimNauts2_ObjectHolder_AsteroidOre_Uranium";
                    object_holder = World.Generator.add_object_holder(
                        type: World.Type.AsteroidOre,
                        object_holder_def: object_holder_def,
                        start_index: Tile
                    );
                    if (HasMap) {
                        Map map = Map;
                        map.info.parent = object_holder;
                        SetFaction(RimWorld.Faction.OfPlayerSilentFail);
                        object_holder.SetFaction(RimWorld.Faction.OfPlayer);
                        Find.World.WorldUpdate();
                    }
                    Destroy();
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.successfully_converted_old_object",
                        prefix: Style.name_prefix,
                        args: new NamedArgument[] { type.ToString(), Tile.ToString() }
                    );
                    break;
                case Satellite_Type.Space_Station:
                    object_holder = World.Generator.add_object_holder(
                        type: World.Type.SpaceStation,
                        start_index: Tile
                    );
                    if (HasMap) {
                        Map map = Map;
                        map.info.parent = object_holder;
                        SetFaction(RimWorld.Faction.OfPlayerSilentFail);
                        object_holder.SetFaction(RimWorld.Faction.OfPlayer);
                        Find.World.WorldUpdate();
                    }
                    Destroy();
                    Logger.print(
                        Logger.Importance.Info,
                        key: "RimNauts.Info.successfully_converted_old_object",
                        prefix: Style.name_prefix,
                        args: new NamedArgument[] { type.ToString(), Tile.ToString() }
                    );
                    break;
                default:
                    Destroy();
                    break;
            }
            base.Tick();
        }

        public override Vector3 DrawPos => Vector3.zero;

        public override void Print(LayerSubMesh subMesh) { }

        public override void Draw() { }
    }
}
