using Verse;
using System.Collections.Generic;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Settle : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Settle() => compClass = typeof(GenerateMapButton);
    }

    public class GenerateMapButton : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (!parent.has_map) {
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
            // generate new satellite with the values saved from before (this process is done to get the new texture)
            Satellite new_satellite = Generate_Satellites.copy_satellite(satellite, get_moon_base(satellite.def_name), Satellite_Type.Moon);
            satellite.type = Satellite_Type.Buffer;
            satellite.Destroy();
            // generate map
            generate_moon_map(new_satellite);
            new_satellite.has_map = true;
            new_satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }

        private static void generate_moon_map(Satellite satellite) {
            Map map = MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(satellite.Biome.defName), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            Satellite.applySatelliteSurface(satellite.Tile, satellite.Biome.defName, map);
        }

        private static string get_moon_base(string moon) => moon + "_Base";
        public static string get_moon_biome(string moon) => moon + "_Biome";
    }
}
