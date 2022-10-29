using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class WorldObjectCompProperties_HarvestAsteroid : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public WorldObjectCompProperties_HarvestAsteroid() => compClass = typeof(HarvestAsteroid);
    }

    public class HarvestAsteroid : RimWorld.Planet.WorldObjectComp {
        public WorldObjectCompProperties_HarvestAsteroid Props => (WorldObjectCompProperties_HarvestAsteroid) props;

        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (!parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = Props.label,
                    defaultDesc = Props.desc,
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true),
                    action = () => Ore.generate_map(parent),
                };
            }
        }
    }

    public class WorldObjectCompProperties_SpawnOre : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public WorldObjectCompProperties_SpawnOre() => compClass = typeof(SpawnOre);
    }

    public class SpawnOre : RimWorld.Planet.WorldObjectComp {
        public WorldObjectCompProperties_SpawnOre Props => (WorldObjectCompProperties_SpawnOre) props;
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (Prefs.DevMode && parent.type == Satellite_Type.Asteroid) {
                yield return new Command_Action {
                    defaultLabel = Props.label + " (Dev)",
                    defaultDesc = Props.desc,
                    action = () => Ore.generate_ore(parent),
                };
            }
        }
    }

    public static class Ore {
        public static Satellite generate_ore(Satellite satellite) {
            Satellite new_satellite = Generate_Satellites.copy_satellite(satellite, Satellite_Type_Methods.WorldObjects(Satellite_Type.Asteroid_Ore).RandomElement(), Satellite_Type.Asteroid_Ore);
            satellite.type = Satellite_Type.Buffer;
            satellite.Destroy();
            return new_satellite;
        }

        public static void generate_map(Satellite satellite) {
            MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(satellite.type), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }
    }
}
