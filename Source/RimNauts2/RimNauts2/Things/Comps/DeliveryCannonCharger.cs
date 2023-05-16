using System;
using Verse;

namespace RimNauts2.Things.Comps {
    public class DeliveryCannonCharger_Properties : CompProperties {
        public int charge_interval = 1600;
        public int max_capacity = 150;

        public DeliveryCannonCharger_Properties() => compClass = typeof(Charger);
    }

    public class Charger : ThingComp {
        public DeliveryCannonCharger_Properties Props => (DeliveryCannonCharger_Properties) props;
        int charge_current = 0;

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref charge_current, "charge_current", 0);
        }

        public bool charging() {
            if (charge_current < Props.charge_interval) {
                charge_current++;
                return true;
            }
            return false;
        }

        public void reset() {
            charge_current = 0;
        }

        public override string CompInspectStringExtra() {
            return "Charge: " + (int) Math.Floor(((float) charge_current / (float) Props.charge_interval) * 100) + "%";
        }
    }
}
