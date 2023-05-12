using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2.Things.Building {
    [StaticConstructorOnStartup]
    public class DeliveryCannon : RimWorld.Building_Storage {
        int launch_interval = 800;
        float max_capacity = 1000;
        Effecter effect = null;
        int effect_start = 0;
        int effect_length_ticks = 50;
        Thing shell = null;

        public override void Tick() {
            base.Tick();
            if (effect != null) {

                effect_start++;

                effect.EffectTick((TargetInfo) this, (TargetInfo) this);

                if (effect_start >= effect_length_ticks) {
                    effect.Cleanup();
                    effect = null;
                }
            }
            if (World.RenderingManager.tick % launch_interval != 0) return;
            launch();
        }

        private void launch() {
            ThingOwner<Thing> things = new ThingOwner<Thing>();
            float capacity = 0;
            
            foreach (Thing thing in slotGroup.HeldThings) {
                if (capacity >= max_capacity) return;
                Log.Message("BAM: " + thing.def.defName);
                capacity += thing.def.BaseMass * thing.stackCount;
                thing.holdingOwner = null;
                things.TryAddOrTransfer(thing, thing.stackCount);
            }

            shell = GenSpawn.Spawn(ThingDef.Named("RimNauts2_TransportPod_Shell"), Position + new IntVec3(0, 0, 1), Map);
            RimWorld.CompTransporter comp_transporter = shell.TryGetComp<RimWorld.CompTransporter>();
            shell.TryGetInnerInteractableThingOwner().TryAddRangeOrTransfer(things, destroyLeftover: true);
            Log.Message(comp_transporter.innerContainer.ContentsString);

            RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
            activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
            activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(comp_transporter.innerContainer, destroyLeftover: true);

            RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(ThingDef.Named("RimNauts2_DropPodLeaving_Shell"), activeDropPod);
            flyShipLeaving.groupID = 0;
            flyShipLeaving.destinationTile = Map.Tile;
            flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;

            effect = Defs.Loader.effecter_delivery_cannon_shot.Spawn();
            effect_start = 0;

            shell.Destroy(DestroyMode.Vanish);
            GenSpawn.Spawn(flyShipLeaving, shell.Position, Map);
            CameraJumper.TryHideWorld();
        }
    }
}
