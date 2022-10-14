using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimNauts2 {
    public class Comp_MoonScope : ThingComp {
        public CompProperties_MoonScope Props {
            get {
                return this.props as CompProperties_MoonScope;
            }
        }

        public override void CompTick() {
            base.CompTick();

        }
        public override void Initialize(CompProperties props) {
            base.Initialize(props);
            this.numberOfMoons = Current.Game.GetComponent<Satellite>().numberOfSatellites;



        }

        public void lookAtMoon() {
            Map map2 = null;
            if (Current.Game.GetComponent<Satellite>().numberOfSatellites == 0) {
                Current.Game.GetComponent<Satellite>().tryGenSatellite();

                map2 = Current.Game.GetComponent<Satellite>().makeMoonMap();

                CameraJumper.TryJump(map2.Center, map2);
                Find.MapUI.Notify_SwitchedMap();


            }

            Current.Game.GetComponent<Satellite>().updateSatellites();
            Find.LetterStack.ReceiveLetter("Look at that moon!", "You can clearly see the surface of the moon with the telescope. Imagine visiting such a place!", LetterDefOf.NeutralEvent, null);


        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {

            if (Current.Game.GetComponent<Satellite>().numberOfSatellites == 0) {
                yield return new Command_Action {
                    defaultLabel = "Look at the Moon!",
                    icon = ContentFinder<Texture2D>.Get("UI/teleIcon", true),
                    defaultDesc = "Look at the moon's surface through the refracting telescope.",
                    action = new Action(this.lookAtMoon)
                };
            }
            yield break;
        }

        public bool triggerFlag = false;
        public int numberOfMoons = 1;
        public int numberOfMaps = 0;
    }
}
