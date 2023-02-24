using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2.Things {
    public class SatelliteDish_Properties : CompProperties {
        public string object_holder;
        public string label;
        public string desc;
        public string failMessage;
        public string successMessage;
        public string texPath;
        public SatelliteDish_Properties() => compClass = typeof(SatelliteDish);
    }
    class SatelliteDish : ThingComp {
        public SatelliteDish_Properties Props => (SatelliteDish_Properties) props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            string label = Props.label;
            if (DebugSettings.godMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = Props.desc,
                icon = ContentFinder<Texture2D>.Get(Props.texPath, true),
                action = new Action(generate_moon)
            };
            if (!DebugSettings.godMode) {
                int total_moons = World.Cache.get_total(World.Type.Moon);
                if (total_moons < (total_moons + 1) * 2) {
                    int diff = (total_moons + 1) * 2 - total_moons;
                    cmd.Disable(diff.ToString() + " " + Props.failMessage);
                }
            }
            yield return cmd;
        }

        public void generate_moon() {
            int new_moon_tile_id = World.Generator.get_free_tile();

            if (new_moon_tile_id != -1) {
                World.Generator.add_object_holder(World.Type.Moon, object_holder_def: Props.object_holder);
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
