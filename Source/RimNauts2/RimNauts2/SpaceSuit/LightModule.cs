using UnityEngine;
using Verse;

namespace RimNauts2.SpaceSuit {
    [StaticConstructorOnStartup]
    class LightModule : RimWorld.Apparel {
        private readonly int wait_time_max = 100;
        private readonly int wait_time_min = -50;
        private bool lights_turned_off;
        private int wait_time;
        private FleckDef light;

        public LightModule() {
            light = DefDatabase<FleckDef>.GetNamed("RimNauts2_Fleck_Flashlight");
            wait_time = wait_time_min;
            lights_turned_off = true;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref lights_turned_off, "lights_turned_off", true);
            Scribe_Values.Look(ref wait_time, "wait_time", wait_time_min);
            light = DefDatabase<FleckDef>.GetNamed("RimNauts2_Fleck_Flashlight");
        }

        public override void Tick() {
            base.Tick();
            if (!Settings.HelmetFlashlightToggle) return;
            if (Wearer == null || !is_dark() || Wearer.InContainerEnclosed || Wearer.CurJob.def == RimWorld.JobDefOf.LayDown || Wearer.CurJob.def == RimWorld.JobDefOf.LayDownAwake || Wearer.CurJob.def == RimWorld.JobDefOf.LayDownResting) {
                if (lights_turned_off) return;
                if (Wearer == null || Wearer.InContainerEnclosed || Wearer.CurJob.def == RimWorld.JobDefOf.LayDown || Wearer.CurJob.def == RimWorld.JobDefOf.LayDownAwake || Wearer.CurJob.def == RimWorld.JobDefOf.LayDownResting) {
                    lights_turned_off = true;
                    wait_time = wait_time_min;
                    return;
                } else if (wait_time > 0) {
                    wait_time--;
                    RimWorld.FleckMaker.AttachedOverlay(Wearer, light, new Vector3(0.0f, 0.0f, 0.0f), scale: 4.0f);
                    return;
                }
                lights_turned_off = true;
                wait_time = wait_time_min;
            } else if (lights_turned_off) {
                if (wait_time < 0) {
                    wait_time++;
                    return;
                }
                RimWorld.FleckMaker.AttachedOverlay(Wearer, light, new Vector3(0.0f, 0.0f, 0.0f), scale: 4.0f);
                lights_turned_off = false;
                wait_time = wait_time_max;
            } else {
                if (wait_time < wait_time_max) wait_time = wait_time_max;
                lights_turned_off = false;
                RimWorld.FleckMaker.AttachedOverlay(Wearer, light, new Vector3(0.0f, 0.0f, 0.0f), scale: 4.0f);
            }
        }

        private bool is_dark() => light_value(new IntVec3(0, 0, 0)) < 0.349999994039536;

        private float light_value(IntVec3 offset) => Wearer.Map.glowGrid.GameGlowAt(offset + Wearer.Position, ignoreCavePlants: true);
    }
}
