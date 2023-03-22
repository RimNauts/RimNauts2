using System.Collections.Generic;
using Verse;

namespace RimNauts2.Things.Building {
    [StaticConstructorOnStartup]
    public class DeliveryCannon : RimWorld.Building_Storage {
        int launch_interval = 400;
        float max_capacity = 1000;

        public override void Tick() {
            base.Tick();
            if (World.RenderingManager.tick % launch_interval != 0) return;
            launch();
        }

        private void launch() {
            List<Thing> things = new List<Thing>();
            float capacity = 0;
            
            foreach (Thing thing in slotGroup.HeldThings) {
                if (capacity >= max_capacity) return;
                Log.Message("BAM: " + thing.def.defName);
                capacity += thing.def.BaseMass;
                things.Add(thing);
            }
        }
    }
}
