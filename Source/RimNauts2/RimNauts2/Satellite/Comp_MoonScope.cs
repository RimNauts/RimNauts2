using System;
using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace RimNauts2 {
    public class Comp_MoonScope : ThingComp {
        public CompProperties_MoonScope Props {
            get {
                return props as CompProperties_MoonScope;
            }
        }

        public override void Initialize(CompProperties props) {
            base.Initialize(props);
            numberOfMoons = Current.Game.GetComponent<Satellites>().numberOfSatellites;
        }

        public void lookAtMoon() {
            Map map2;
            if (Current.Game.GetComponent<Satellites>().numberOfSatellites == 0) {
                Current.Game.GetComponent<Satellites>().tryGenSatellite();
                map2 = Current.Game.GetComponent<Satellites>().makeMoonMap();
                CameraJumper.TryJump(map2.Center, map2);
                Find.MapUI.Notify_SwitchedMap();
            }
            Current.Game.GetComponent<Satellites>().updateSatellites();
            Find.LetterStack.ReceiveLetter("Look at that moon!", "You can clearly see the surface of the moon with the telescope. Imagine visiting such a place!", LetterDefOf.NeutralEvent, null);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            if (Current.Game.GetComponent<Satellites>().numberOfSatellites == 0) {
                yield return new Command_Action {
                    defaultLabel = "Look at the Moon!",
                    icon = ContentFinder<Texture2D>.Get("UI/teleIcon", true),
                    defaultDesc = "Look at the moon's surface through the refracting telescope.",
                    action = new Action(lookAtMoon)
                };
            }
            yield break;
        }

        public bool triggerFlag = false;
        public int numberOfMoons = 1;
        public int numberOfMaps = 0;
    }
}
