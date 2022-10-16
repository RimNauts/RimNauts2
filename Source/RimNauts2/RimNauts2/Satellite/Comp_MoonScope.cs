using System;
using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace RimNauts2 {
    public class Comp_MoonScope : RimWorld.Planet.WorldObjectComp {
        public void generate_moon_map() {
            Map map2;
            if (Satellites.rock_moon_tile != -1 && !Satellites.has_moon_map) {
                map2 = Current.Game.GetComponent<Satellites>().makeMoonMap();
                if (map2 == null) return;
                CameraJumper.TryJump(map2.Center, map2);
                Find.MapUI.Notify_SwitchedMap();
            }
            Current.Game.GetComponent<Satellites>().updateSatellites();
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            if (!Satellites.has_moon_map) {
                yield return new Command_Action {
                    defaultLabel = "Settle",
                    icon = ContentFinder<Texture2D>.Get("UI/teleIcon", true),
                    defaultDesc = "Settle the moon.",
                    action = new Action(generate_moon_map)
                };
            }
            yield break;
        }
    }
}
