using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace RimNauts2 {
    public class CompProperties_SatelliteDish : CompProperties {
        public string worldObject;
        public CompProperties_SatelliteDish() => compClass = typeof(SatelliteDish);
    }
    class SatelliteDish : ThingComp {
        public CompProperties_SatelliteDish Props => (CompProperties_SatelliteDish) props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            yield return new Command_Action {
                defaultLabel = "Look for a " + Props.worldObject.Substring(14).ToLower() + " moon",
                defaultDesc = "Your pawn will look for a " + Props.worldObject.Substring(14).ToLower() + " moon orbiting the planet.",
                icon = ContentFinder<UnityEngine.Texture2D>.Get("Satellites/Moons/" + Props.worldObject, true),
                action = new Action(action)
            };
            yield break;
        }

        public void action() {
            int new_moon_tile_id = -1;

            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                if (Find.World.grid.tiles.ElementAt(i).biome.defName == "RimNauts2_Satellite_Biome") {
                    if (new_moon_tile_id == -1) {
                        new_moon_tile_id = i;
                        break;
                    }
                }
            }

            if (new_moon_tile_id != -1) {
                Generate_Satellites.add_satellite(new_moon_tile_id, Satellite_Type.Moon, def_name: Props.worldObject);
                Messages.Message("Succesfully found a " + Props.worldObject.Substring(14).ToLower() + " moon orbiting the planet!", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            } else {
                Log.Error("RimNauts2: Couldn't find a free tile to spawn a moon on. Either map size is too small to spawn all the satellites or increase total satellite objects in settings");
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldCameraDriver), nameof(RimWorld.Planet.WorldCameraDriver.JumpTo), new Type[] { typeof(int) })]
    public static class WorldCameraDriver {
        public static bool Prefix(int tile) {
            if (Find.WorldObjects.AnyWorldObjectAt<Satellite>(tile)) {
                Satellite satellite = Find.WorldObjects.WorldObjectAt<Satellite>(tile);
                RimWorld.Planet.WorldCameraDriver camera = new RimWorld.Planet.WorldCameraDriver();
                return false;
            }
            return true;
        }
    }
}
