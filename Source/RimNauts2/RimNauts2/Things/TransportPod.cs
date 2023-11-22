using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;

namespace RimNauts2.Things {
    public class TransportPod_Properties : CompProperties {
        public float fuelThreshold;
        public string name;
        public string iconPath;
        public string celestialObjectDefName;
        public bool createMap;
        public ThingDef skyfallerLeaving;
        public string module_def;

        public TransportPod_Properties() => compClass = typeof(TransportPod);
    }

    [StaticConstructorOnStartup]
    public class TransportPod : ThingComp {
        private RimWorld.CompTransporter cached_comp_transporter;
        public Verse.Building FuelingPortSource => RimWorld.FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(parent.Position, parent.Map);
        public bool ConnectedToFuelingPort => FuelingPortSource != null;
        public float FuelingPortSourceFuel => !ConnectedToFuelingPort ? 0.0f : FuelingPortSource.GetComp<RimWorld.CompRefuelable>().Fuel;
        public TransportPod_Properties Props => (TransportPod_Properties) props;
        public float FuelThreshold => Universum.Utilities.Cache.allowed_utility(parent.Map, "universum.vacuum") ? Props.fuelThreshold * 0.2f : Props.fuelThreshold;

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            string label = "RimNauts.transportpod_label".Translate(Props.name);
            if (DebugSettings.godMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = "RimNauts.transportpod_desc".Translate(Props.name),
                icon = ContentFinder<Texture2D>.Get(Props.iconPath, true),
                action = new Action(launch)
            };
            if (!DebugSettings.godMode) {
                ThingDef module_thing = ThingDef.Named(Props.module_def);
                if (!Transporter.innerContainer.Contains(module_thing)) {
                    cmd.Disable(TranslatorFormattedStringExtensions.Translate(key: "RimNauts.module_required", module_thing.label));
                } else if (FuelingPortSourceFuel < FuelThreshold) {
                    "RimNauts.transportpod_fail_message_fuel".Translate(FuelThreshold, FuelingPortSourceFuel + "/" + FuelThreshold);
                    cmd.Disable("RimNauts.transportpod_fail_message_fuel".Translate(FuelThreshold, FuelingPortSourceFuel + "/" + FuelThreshold));
                } else if (Transporter.innerContainer.Count != 1) {
                    cmd.Disable(TranslatorFormattedStringExtensions.Translate(key: "RimNauts.only_module", module_thing.label));
                }
            }
            yield return cmd;
        }

        public void launch() {
            if (!parent.Spawned) {
                Logger.print(
                    Logger.Importance.Error,
                    key: "RimNauts.Error.tried_to_launch_thing",
                    prefix: Style.name_prefix
                );
                return;
            }
            Transporter.innerContainer.Clear();
            Map map = parent.Map;
            Verse.Building fuelingPortSource = FuelingPortSource;
            fuelingPortSource?.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(FuelThreshold);
            RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
            activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
            RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving, activeDropPod);
            flyShipLeaving.groupID = 0;
            flyShipLeaving.destinationTile = map.Tile;
            flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
            parent.Destroy(DestroyMode.Vanish);
            GenSpawn.Spawn(flyShipLeaving, parent.Position, map);
            CameraJumper.TryHideWorld();

            Universum.Defs.CelestialObject celestialObjectDef = Universum.Defs.Loader.celestialObjects[Props.celestialObjectDefName];

            if (celestialObjectDef.objectHolder != null) {
                int tile_id = Universum.World.Generator.GetFreeTile();
                if (tile_id == -1) {
                    Messages.Message("RimNauts.transportpod_fail_message_launch".Translate(), RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Logger.print(
                        Logger.Importance.Error,
                        key: "RimNauts.Error.no_free_tile_for_satellite",
                        prefix: Style.name_prefix
                    );
                    return;
                }
                Universum.World.ObjectHolder object_holder = Universum.World.Generator.CreateObjectHolder(Props.celestialObjectDefName, tile: tile_id);
                if (object_holder == null) return;
                if (Props.createMap) object_holder.CreateMap(RimWorld.Faction.OfPlayer);
            } else Universum.World.Generator.Create(Props.celestialObjectDefName);

            Messages.Message("RimNauts.transportpod_success_message_launch".Translate(), RimWorld.MessageTypeDefOf.PositiveEvent, true);
        }

        public RimWorld.CompTransporter Transporter {
            get {
                if (cached_comp_transporter == null)
                    cached_comp_transporter = parent.GetComp<RimWorld.CompTransporter>();
                return cached_comp_transporter;
            }
        }
    }
}
