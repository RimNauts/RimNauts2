using RimWorld.Planet;
using Verse;

namespace RimNauts2.Things.Building {
    [StaticConstructorOnStartup]
    public class DeliveryCannon : RimWorld.Building_Storage {
        readonly int launch_interval = 800;
        readonly float max_capacity = 1000;
        Effecter effect = null;
        int effect_start = 0;
        readonly int effect_length_ticks = 50;
        public Comps.Target Target => GetComp<Comps.Target>();

        public override void Tick() {
            base.Tick();
            if (effect != null) {
                effect.EffectTick((TargetInfo) this, (TargetInfo) this);
                effect_start++;
                if (effect_start >= effect_length_ticks) {
                    effect.Cleanup();
                    effect = null;
                }
            }
            if (World.RenderingManager.tick % launch_interval != 0) return;
            if (!Target.valid_target()) return;
            launch();
        }

        private void launch() {
            ThingOwner<Thing> things = new ThingOwner<Thing>();
            float capacity = 0;
            
            foreach (Thing thing in slotGroup.HeldThings) {
                if (capacity >= max_capacity) return;
                capacity += thing.def.BaseMass * thing.stackCount;
                thing.holdingOwner = null;
                things.TryAddOrTransfer(thing, thing.stackCount);
            }

            RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
            activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
            activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(things, destroyLeftover: true);

            RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(
                ThingDef.Named("RimNauts2_DropPodLeaving_Shell"),
                activeDropPod
            );
            flyShipLeaving.groupID = 0;
            flyShipLeaving.destinationTile = Target.target_tile;
            flyShipLeaving.arrivalAction = new TransportPodArrivalAction(Target.get_map(Target.target_tile).Parent, Target.target_cell);
            flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;

            effect = Defs.Loader.effecter_delivery_cannon_shot.Spawn();
            effect_start = 0;

            GenSpawn.Spawn(flyShipLeaving, Position + new IntVec3(0, 0, 1), Map);
        }
    }
}
