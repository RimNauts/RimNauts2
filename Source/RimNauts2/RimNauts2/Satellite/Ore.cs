using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_SpawnOre : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public WorldObjectCompProperties_SpawnOre() => compClass = typeof(SpawnOre);
    }

    public class SpawnOre : RimWorld.Planet.WorldObjectComp {
        public WorldObjectCompProperties_SpawnOre Props => (WorldObjectCompProperties_SpawnOre) props;
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (DebugSettings.godMode && parent.type == Satellite_Type.Asteroid) {
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
            SatelliteSettings satellite_settings = Generate_Satellites.copy_satellite(satellite, Satellite_Type_Methods.WorldObjects(Satellite_Type.Asteroid_Ore).RandomElement(), Satellite_Type.Asteroid_Ore);
            satellite.type = Satellite_Type.Buffer;
            satellite.Destroy();
            Satellite new_satellite = Generate_Satellites.paste_satellite(satellite_settings);
            if (!SatelliteContainer.exists(new_satellite.Tile)) {
                SatelliteContainer.add(new_satellite);
            }
            return new_satellite;
        }
    }
}
