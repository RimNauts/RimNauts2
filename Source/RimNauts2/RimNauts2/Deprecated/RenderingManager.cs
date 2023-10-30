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

    public class Caching_Handler : GameComponent {
        public Caching_Handler(Game game) : base() { }
    }
    
    class RenderingManager : GameComponent {
        public RenderingManager(Game game) : base() { }
    }

    [StaticConstructorOnStartup]
    public class ObjectHolder : RimWorld.Planet.MapParent {
        public MapGeneratorDef map_generator;
        public Type type;

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Defs.Look(ref map_generator, "map_generator");
            Scribe_Values.Look(ref type, "type");
        }

        public override void Tick() {
            if (destroyed) return;

            Universum.World.Generator.UpdateTile(Tile, RimWorld.BiomeDefOf.Ocean);

            if (HasMap || type == Type.Satellite) {
                Universum.Defs.CelestialObject celestialObjectDef = null;

                switch (type) {
                    case Type.Moon:
                        celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Moon_Barren"];
                        if (map_generator.defName == "RimNauts2_MoonWater_MapGen") celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Moon_Ocean"];
                        break;
                    case Type.AsteroidOre:
                        celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_AsteroidOre_Steel"];
                        if (map_generator.defName == "RimNauts2_OreGold_MapGen") celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_AsteroidOre_Gold"];
                        if (map_generator.defName == "RimNauts2_OrePlasteel_MapGen") celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_AsteroidOre_Plasteel"];
                        if (map_generator.defName == "RimNauts2_OreUranium_MapGen") celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_AsteroidOre_Uranium"];
                        break;
                    case Type.Satellite:
                        celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite"];
                        break;
                    case Type.SpaceStation:
                        celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Station"];
                        break;
                    default:
                        celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_AsteroidOre_Steel"];
                        break;
                }

                if (celestialObjectDef.objectHolder != null) {
                    Universum.World.ObjectHolder objectHolder = Universum.World.Generator.CreateObjectHolder(celestialObjectDef.defName, tile: Tile);

                    Map.info.parent = objectHolder;
                    objectHolder.SetFaction(RimWorld.Faction.OfPlayer);
                    Find.World.WorldUpdate();
                } else Universum.World.Generator.Create(celestialObjectDef.defName);
            }
            Destroy();
        }

        public override void Draw() { }

        public override void Print(LayerSubMesh subMesh) { }

        public override Vector3 DrawPos => Vector3.zero;
    }
}
