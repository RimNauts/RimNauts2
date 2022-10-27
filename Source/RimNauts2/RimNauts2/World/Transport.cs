using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;

namespace RimNauts2 {
    public class CompProperties_Launch : CompProperties {
        public float fuelThreshold;
        public string label;
        public string desc;
        public string name;
        public string iconPath;
        public string createDefName;
        public string type;
        public bool createMap;

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
            if (Prefs.DevMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = Props.desc,
                icon = ContentFinder<Texture2D>.Get(Props.iconPath, true),
                action = new Action(launch)
            };
            if (!Prefs.DevMode) {
                if (FuelingPortSourceFuel < Props.fuelThreshold)
                    cmd.Disable("Requires " + Props.fuelThreshold + " fuel, currently at " + FuelingPortSourceFuel + " fuel.");
            }
            yield return cmd;
        }

        public void launch() {
            if (!parent.Spawned) {
                Log.Error("Tried to launch " + parent + ", but it's unspawned.");
            } else {
                Map map = parent.Map;
                Building fuelingPortSource = FuelingPortSource;
                if (fuelingPortSource != null)
                    fuelingPortSource.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(Props.fuelThreshold);
                RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
                RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(DefDatabase<ThingDef>.GetNamed("RimNauts2_DropPodLeaving", true), activeDropPod);
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
                    Messages.Message("Failed to " + Props.label.ToLower() + " into orbit.", RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Log.Error("RimNauts2: Couldn't find a free tile to spawn a " + Props.name + " on. Either the map size is too small to spawn all the satellites or increase the total satellite objects in settings (requires a new save)");
                    return;
                }
                Satellite_Type type = Satellite_Type_Methods.get_type_from_string(Props.type);

                Satellite satellite = Generate_Satellites.add_satellite(tile_id, type);

                if (Props.createMap) {
                    MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(satellite.type), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
                    satellite.applySatelliteSurface();

                    satellite.has_map = true;
                    satellite.SetFaction(RimWorld.Faction.OfPlayer);
                    Find.World.WorldUpdate();
                }
                

                Messages.Message("Successfully launched a " + Props.name + " into orbit.", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class TransportpodSatelliteIgnoreMaxRange {
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool to_moon = Find.World.grid.tiles.ElementAt(end).biome.defName == "RimNauts2_Satellite_Biome";
            bool from_moon = Find.World.grid.tiles.ElementAt(start).biome.defName == "RimNauts2_Satellite_Biome";
            if (to_moon || from_moon) {
                if (!from_moon) {
                    if (Find.WorldObjects.WorldObjectAt<Satellite>(end).def.defName.Contains("Ore")) {
                        __result = 9999;
                        return;
                    }
                }
                __result = 1;
                return;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods), "Start", MethodType.Getter)]
    public static class TransportpodFromSatelliteAnimation {
        public static void Postfix(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int num = (int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == num) __result = worldObject.DrawPos;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods), "End", MethodType.Getter)]
    public static class TransportpodToSatelliteAnimation {
        public static void Postfix(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int destinationTile = __instance.destinationTile;
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == destinationTile) __result = worldObject.DrawPos;
            }
        }
    }
}
