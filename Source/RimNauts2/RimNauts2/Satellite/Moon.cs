using Verse;
using System.Collections.Generic;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Settle : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Settle() => compClass = typeof(GenerateMapButton);
    }

    public class GenerateMapButton : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (!parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => Moon.generate_moon(parent),
                };
            }
        }
    }

    public static class Moon {
        public static void generate_moon(Satellite satellite) {
            Satellite new_satellite = Generate_Satellites.copy_satellite(satellite, satellite.def_name + "_Base", Satellite_Type.Moon);
            satellite.type = Satellite_Type.Buffer;
            satellite.Destroy();
            // generate map
            MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(new_satellite.type), new_satellite, new_satellite.MapGeneratorDef, new_satellite.ExtraGenStepDefs, null);
            new_satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
            new_satellite.has_moon_map = true;
        }
    }
}
