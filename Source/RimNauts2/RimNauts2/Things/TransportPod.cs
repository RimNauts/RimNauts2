using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RimNauts2.Things {
    public class TransportPod_Properties : CompProperties {
        public float fuelThreshold;
        public string label;
        public string desc;
        public string name;
        public string iconPath;
        public string failMessageFuel;
        public string failMessageLaunch;
        public string successMessage;
        public string createDefName;
        public int type;
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

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            string label = Props.label;
            if (DebugSettings.godMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = Props.desc,
                icon = ContentFinder<Texture2D>.Get(Props.iconPath, true),
                action = new Action(launch)
            };
            if (!DebugSettings.godMode) {
                ThingDef module_thing = ThingDef.Named(Props.module_def);
                if (!Transporter.innerContainer.Contains(module_thing)) {
                    cmd.Disable("Requires 1 " + module_thing.label);
                } else if (FuelingPortSourceFuel < Props.fuelThreshold) {
                    cmd.Disable(Props.fuelThreshold + " " + Props.failMessageFuel + " " + FuelingPortSourceFuel + "/" + Props.fuelThreshold);
                } else if (Transporter.innerContainer.Count != 1) {
                    cmd.Disable("Can only send 1 module into orbit, please remove everything else");
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
            } else {
                Map map = parent.Map;
                Verse.Building fuelingPortSource = FuelingPortSource;
                fuelingPortSource?.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(Props.fuelThreshold);
                RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
                RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving, activeDropPod);
                flyShipLeaving.groupID = 0;
                flyShipLeaving.destinationTile = map.Tile;
                flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
                parent.Destroy(DestroyMode.Vanish);
                GenSpawn.Spawn(flyShipLeaving, parent.Position, map);
                CameraJumper.TryHideWorld();
                int tile_id = World.Generator.get_free_tile();
                if (tile_id == -1) {
                    Messages.Message(Props.failMessageLaunch, RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Logger.print(
                        Logger.Importance.Error,
                        key: "RimNauts.Error.no_free_tile_for_satellite",
                        prefix: Style.name_prefix
                    );
                    return;
                }
                World.ObjectHolder object_holder = World.Generator.add_object_holder((World.Type) Props.type);
                if (object_holder == null) return;
                if (Props.createMap) {
                    MapGenerator.GenerateMap(
                        Find.World.info.initialMapSize,
                        object_holder,
                        object_holder.MapGeneratorDef,
                        object_holder.ExtraGenStepDefs,
                        extraInitBeforeContentGen: null
                    );
                    object_holder.SetFaction(RimWorld.Faction.OfPlayer);
                    Find.World.WorldUpdate();
                }
                Messages.Message(Props.successMessage, RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
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
