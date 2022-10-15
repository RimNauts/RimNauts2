using System;
using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace RimNauts2 {
    public class Comp_MoonScope : ThingComp {
        public void lookAtMoon() {
            Map map2;
            if (!Satellites.moon_exists) {
                map2 = Current.Game.GetComponent<Satellites>().makeMoonMap();
                if (map2 == null) return;
                CameraJumper.TryJump(map2.Center, map2);
                Find.MapUI.Notify_SwitchedMap();
            }
            Current.Game.GetComponent<Satellites>().updateSatellites();
            Find.LetterStack.ReceiveLetter("Look at that moon!", "You can clearly see the surface of the moon with the telescope. Imagine visiting such a place!", RimWorld.LetterDefOf.NeutralEvent, null);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            if (!Satellites.moon_exists) {
                yield return new Command_Action {
                    defaultLabel = "Look at the Moon!",
                    icon = ContentFinder<Texture2D>.Get("UI/teleIcon", true),
                    defaultDesc = "Look at the moon's surface through the refracting telescope.",
                    action = new Action(lookAtMoon)
                };
            }
            yield break;
        }
    }
}
