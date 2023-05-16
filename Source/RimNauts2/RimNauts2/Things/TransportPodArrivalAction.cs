using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.Things {
    public class TransportPodArrivalAction : RimWorld.Planet.TransportPodsArrivalAction {
        public RimWorld.Planet.MapParent map_parent;
        public IntVec3 cell;

        public TransportPodArrivalAction(RimWorld.Planet.MapParent map_parent, IntVec3 cell) {
            this.map_parent = map_parent;
            this.cell = cell;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_References.Look(ref map_parent, "mapParent");
            Scribe_Values.Look(ref cell, "cell");
        }

        public override RimWorld.FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile) {
            RimWorld.FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
            if (!floatMenuAcceptanceReport) return floatMenuAcceptanceReport;
            if (map_parent != null && map_parent.Tile != destinationTile) return false;
            return CanLandInSpecificCell(pods, map_parent);
        }

        public override void Arrived(List<RimWorld.ActiveDropPodInfo> pods, int tile) {
            RimWorld.Planet.TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(pods);
            RimWorld.ActiveDropPodInfo pod = new RimWorld.ActiveDropPodInfo();
            for (int i = 0; i < pods.Count; i++) {
                pod.innerContainer.TryAddRangeOrTransfer(pods[i].innerContainer, destroyLeftover: true);
            }
            pod.openDelay = 0;
            Thing activeDropPod_thing = ThingMaker.MakeThing(Defs.Loader.thing_delivery_cannon_active);
            RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) activeDropPod_thing;
            activeDropPod.Contents = pod;
            RimWorld.SkyfallerMaker.SpawnSkyfaller(Defs.Loader.thing_delivery_cannon_incoming, activeDropPod, cell, map_parent.Map);
        }

        public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, RimWorld.Planet.MapParent mapParent) {
            if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap) return false;
            if (mapParent.EnterCooldownBlocksEntering()) {
                return RimWorld.FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod()));
            }
            return true;
        }
    }
}
