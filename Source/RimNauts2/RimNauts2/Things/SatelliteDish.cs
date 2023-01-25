using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class CompProperties_SatelliteDish : CompProperties {
        public string worldObject;
        public string label;
        public string desc;
        public string failMessage;
        public string successMessage;
        public string texPath;
        public CompProperties_SatelliteDish() => compClass = typeof(SatelliteDish);
    }
    class SatelliteDish : ThingComp {
        public CompProperties_SatelliteDish Props => (CompProperties_SatelliteDish) props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            string label = Props.label;
            if (DebugSettings.godMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = Props.desc,
                icon = ContentFinder<Texture2D>.Get(Props.texPath, true),
                action = new Action(action)
            };
            if (!DebugSettings.godMode) {
                if (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) < (SatelliteContainer.size(Satellite_Type.Moon) + 1) * 2) {
                    int diff = (SatelliteContainer.size(Satellite_Type.Moon) + 1) * 2 - SatelliteContainer.size(Satellite_Type.Artifical_Satellite);
                    cmd.Disable(diff.ToString() + " " + Props.failMessage);
                }
            }
            yield return cmd;
        }

        public void action() {
            int new_moon_tile_id = -1;

            foreach (var satellite in RimNauts_GameComponent.satellites) {
                if (satellite.Value.type != Satellite_Type.Asteroid || satellite.Value.can_out_of_bounds || satellite.Value.mineral_rich) continue;
                new_moon_tile_id = satellite.Key;
                break;
            }

            if (new_moon_tile_id != -1) {
                Generate_Satellites.add_satellite(new_moon_tile_id, Satellite_Type.Moon, def_name: Props.worldObject);
                Messages.Message(Props.successMessage, RimWorld.MessageTypeDefOf.PositiveEvent, true);
            } else {
                Logger.print(
                    Logger.Importance.Error,
                    key: "RimNauts.Error.no_free_tile_for_satellite",
                    prefix: Style.name_prefix
                );
            }
        }
    }
}
