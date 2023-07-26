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
                    defaultLabel = "RimNauts.Label.delivery_cannon_mode_auto".Translate(),
                    defaultDesc = "RimNauts.Description.delivery_cannon_mode_auto".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_ManualMode", true),
                    action = new Action(change_mode)
                };
                yield return cmd;
            } else {
                Command_Action cmd = new Command_Action {
                    defaultLabel = "RimNauts.Label.delivery_cannon_mode_manual".Translate(),
                    defaultDesc = "RimNauts.Description.delivery_cannon_mode_manual".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_AutomaticMode", true),
                    action = new Action(change_mode)
                };
                yield return cmd;
                if (fire_once) {
                    Command_Action cmd_fire = new Command_Action {
                        defaultLabel = "RimNauts.Label.delivery_cannon_stop_delivery".Translate(),
                        defaultDesc = "RimNauts.Description.delivery_cannon_stop_delivery".Translate(),
                        icon = ContentFinder<Texture2D>.Get("Icons/RimNauts2_DeliveryCannon_Cancel", true),
                        action = new Action(order_fire)
                    };
                    yield return cmd_fire;
                } else {
                    Command_Action cmd_fire = new Command_Action {
                        defaultLabel = "RimNauts.Label.delivery_cannon_send_delivery".Translate(),
                        defaultDesc = "RimNauts.Description.delivery_cannon_send_delivery".Translate(),
                        icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true),
                        action = new Action(order_fire)
                    };
                    yield return cmd_fire;
                }

            }
        }

        public override string CompInspectStringExtra() {
            return "RimNauts.delivery_cannon_automatic_firing_mode".Translate(automatic.ToString());
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
