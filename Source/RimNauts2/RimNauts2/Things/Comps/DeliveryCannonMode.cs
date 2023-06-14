using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Comps {
    public class DeliveryCannonMode_Properties : CompProperties {
        public DeliveryCannonMode_Properties() => compClass = typeof(Mode);
    }

    public class Mode : ThingComp {
        public DeliveryCannonMode_Properties Props => (DeliveryCannonMode_Properties) props;
        public bool automatic = true;
        public bool fire_once = false;

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref automatic, "automatic", true);
            Scribe_Values.Look(ref fire_once, "fire_once", false);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            if (automatic) {
                Command_Action cmd = new Command_Action {
                    defaultLabel = "Change firing mode to manual",
                    defaultDesc = "Change to manual firing",
                    icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_ManualMode", true),
                    action = new Action(change_mode)
                };
                yield return cmd;
            } else {
                Command_Action cmd = new Command_Action {
                    defaultLabel = "Change firing mode to automatic",
                    defaultDesc = "Change to automatic firing",
                    icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_AutomaticMode", true),
                    action = new Action(change_mode)
                };
                yield return cmd;
                if (fire_once) {
                    Command_Action cmd_fire = new Command_Action {
                        defaultLabel = "Cancel delivery",
                        defaultDesc = "Stop the the delivery cannon from firing",
                        icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_Cancel", true),
                        action = new Action(order_fire)
                    };
                    yield return cmd_fire;
                } else {
                    Command_Action cmd_fire = new Command_Action {
                        defaultLabel = "Send delivery",
                        defaultDesc = "Order the delivery cannon to fire once ready",
                        icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true),
                        action = new Action(order_fire)
                    };
                    yield return cmd_fire;
                }

            }
        }

        public override string CompInspectStringExtra() {
            return "Automatic firing mode: " + automatic;
        }

        public void change_mode() {
            automatic = !automatic;
            fire_once = false;
        }

        public void order_fire() {
            fire_once = !fire_once;
        }

        public bool can_fire() {
            if (automatic) return true;
            if (!fire_once) return false;
            fire_once = false;
            return true;
        }
    }
}
