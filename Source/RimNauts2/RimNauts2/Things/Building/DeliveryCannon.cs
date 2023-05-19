using System.Linq;
using Verse;

namespace RimNauts2.Things.Building {
    [StaticConstructorOnStartup]
    public class DeliveryCannon : RimWorld.Building_Storage {
        Effecter effect = null;
        int effect_start = 0;
        readonly int effect_length_ticks = 40;
        Comps.Targeter targeter = null;
        Comps.Charger charger = null;
        RimWorld.CompPowerTrader power = null;

        public Comps.Targeter Targeter {
            get {
                if (targeter != null) return targeter;
                targeter = GetComp<Comps.Targeter>();
                return targeter;
            }
        }

        public Comps.Charger Charger {
            get {
                if (charger != null) return charger;
                charger = GetComp<Comps.Charger>();
                return charger;
            }
        }

        public RimWorld.CompPowerTrader Power {
            get {
                if (power != null) return power;
                power = GetComp<RimWorld.CompPowerTrader>();
                return power;
            }
        }

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
            if (!Power.PowerOn) return;
            if (Charger.charging()) return;
            if (slotGroup.HeldThings.Count() <= 0) return;
            if (!Targeter.valid_target()) return;
            launch();
            Charger.reset();
        }

        private void launch() {
            ThingOwner<Thing> things = new ThingOwner<Thing>();
            float capacity = 0;
            foreach (Thing thing in slotGroup.HeldThings) {
                if (capacity >= Charger.Props.max_capacity) break;
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
            flyShipLeaving.destinationTile = Targeter.target_tile;
            flyShipLeaving.arrivalAction = new TransportPodArrivalAction(Targeter.get_map(Targeter.target_tile).Parent, Targeter.target_cell);
            flyShipLeaving.worldObjectDef = Defs.Loader.world_object_travelling_delivery_cannon_shell;

            effect = Defs.Loader.effecter_delivery_cannon_shot.Spawn();
            effect_start = 0;

            GenSpawn.Spawn(flyShipLeaving, Position + new IntVec3(0, 0, 1), Map);
        }
    }
}
