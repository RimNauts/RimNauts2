﻿using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class CompProperties_SatelliteDish : CompProperties {
        public string worldObject;
        public string texPath;
        public CompProperties_SatelliteDish() => compClass = typeof(SatelliteDish);
    }
    class SatelliteDish : ThingComp {
        public CompProperties_SatelliteDish Props => (CompProperties_SatelliteDish) props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            string label = "Look for a " + Props.worldObject.Substring(14).ToLower() + " moon";
            if (Prefs.DevMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = "Your pawn will look for a " + Props.worldObject.Substring(14).ToLower() + " moon orbiting the planet.",
                icon = ContentFinder<Texture2D>.Get(Props.texPath, true),
                action = new Action(action)
            };
            if (!Prefs.DevMode) {
                if (SatelliteContainer.size(Satellite_Type.Artifical_Satellite) < (SatelliteContainer.size(Satellite_Type.Moon) + 1) * 2) {
                    int diff = (SatelliteContainer.size(Satellite_Type.Moon) + 1) * 2 - SatelliteContainer.size(Satellite_Type.Artifical_Satellite);
                    cmd.Disable("Requires " + diff.ToString() + " more satellite" + (diff > 1 ? "s" : "") + " orbiting the planet.");
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
                Messages.Message("Successfully found a " + Props.worldObject.Substring(14).ToLower() + " moon orbiting the planet!", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            } else {
                Log.Error("RimNauts2: Couldn't find a free tile to spawn a moon on. Either the map size is too small to spawn all the satellites or increase the total satellite objects in settings (requires a new save).");
            }
        }
    }
}
