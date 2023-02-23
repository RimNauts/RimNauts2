using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;

namespace RimNauts2 {
    /*public class CompProperties_Launch : CompProperties {
        public float fuelThreshold;
        public string label;
        public string desc;
        public string name;
        public string iconPath;
        public string failMessageFuel;
        public string failMessageLaunch;
        public string successMessage;
        public string createDefName;
        public string type;
        public bool createMap;
        public ThingDef skyfallerLeaving;

        public CompProperties_Launch() => compClass = typeof(Launch);
    }

    [StaticConstructorOnStartup]
    public class Launch : ThingComp {
        public Building FuelingPortSource => RimWorld.FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(parent.Position, parent.Map);
        public bool ConnectedToFuelingPort => FuelingPortSource != null;
        public float FuelingPortSourceFuel => !ConnectedToFuelingPort ? 0.0f : FuelingPortSource.GetComp<RimWorld.CompRefuelable>().Fuel;
        public CompProperties_Launch Props => (CompProperties_Launch) props;

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
                if (FuelingPortSourceFuel < Props.fuelThreshold)
                    cmd.Disable(Props.fuelThreshold + " " + Props.failMessageFuel + " " + FuelingPortSourceFuel + "/" + Props.fuelThreshold);
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
                Building fuelingPortSource = FuelingPortSource;
                if (fuelingPortSource != null)
                    fuelingPortSource.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(Props.fuelThreshold);
                RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
                RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving, activeDropPod);
                flyShipLeaving.groupID = 0;
                flyShipLeaving.destinationTile = map.Tile;
                flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
                parent.Destroy(DestroyMode.Vanish);
                GenSpawn.Spawn(flyShipLeaving, parent.Position, map);
                CameraJumper.TryHideWorld();

                int tile_id = -1;

                foreach (var old_satellite in RimNauts_GameComponent.satellites) {
                    if (old_satellite.Value.type != Satellite_Type.Asteroid || old_satellite.Value.can_out_of_bounds || old_satellite.Value.mineral_rich) continue;
                    tile_id = old_satellite.Key;
                    break;
                }

                if (tile_id == -1) {
                    Messages.Message(Props.failMessageLaunch, RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Logger.print(
                        Logger.Importance.Error,
                        key: "RimNauts.Error.no_free_tile_for_satellite",
                        prefix: Style.name_prefix
                    );
                    return;
                }
                Satellite_Type type = Satellite_Type_Methods.get_type_from_string(Props.type);

                Satellite satellite = Generate_Satellites.add_satellite(tile_id, type);

                if (Props.createMap) {
                    MapGenerator.GenerateMap(Find.World.info.initialMapSize, satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
                    satellite.SetFaction(RimWorld.Faction.OfPlayer);
                    Find.World.WorldUpdate();
                }
                

                Messages.Message(Props.successMessage, RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
        }
    }*/
}
