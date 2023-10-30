using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2.Things {
    public class SatelliteDish_Properties : CompProperties {
        public string celestialObjectDefName;
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
                int total_moons = Universum.Game.MainLoop.instance.GetTotal(Universum.Defs.Loader.celestialObjects[Props.celestialObjectDefName]);
                int total_satellites = Universum.Game.MainLoop.instance.GetTotal(Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite"]);
                if (total_satellites < (total_moons + 1) * 2) {
                    int diff = (total_moons + 1) * 2 - total_satellites;
                    cmd.Disable(diff.ToString() + " " + Props.failMessage);
                }
            }
            yield return cmd;
        }

        public void generate_moon() {
            int new_moon_tile_id = Universum.World.Generator.GetFreeTile();

            if (new_moon_tile_id != -1) {
                Universum.World.Generator.CreateObjectHolder(Props.celestialObjectDefName, tile: new_moon_tile_id);
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
