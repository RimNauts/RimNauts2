using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;

namespace RimNauts2 {
    public class CompProperties_SpaceStation : CompProperties {
        public bool requireFuel = true;
        public ThingDef skyfallerLeaving;

        public CompProperties_SpaceStation() => compClass = typeof(SpaceStation);
    }

    [StaticConstructorOnStartup]
    public class SpaceStation : ThingComp {
        private RimWorld.CompTransporter cachedCompTransporter;
        public CompProperties_Launchable Props => (CompProperties_Launchable) props;
        public Building FuelingPortSource => RimWorld.FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(parent.Position, parent.Map);
        public bool ConnectedToFuelingPort => !Props.requireFuel || FuelingPortSource != null;
        public float FuelingPortSourceFuel => !ConnectedToFuelingPort ? 0.0f : FuelingPortSource.GetComp<RimWorld.CompRefuelable>().Fuel;

        public RimWorld.CompTransporter Transporter {
            get {
                if (cachedCompTransporter == null)
                    cachedCompTransporter = parent.GetComp<RimWorld.CompTransporter>();
                return cachedCompTransporter;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            SpaceStation compLaunchable = this;
            ThingOwner inventory = Transporter.innerContainer;
            string label = "Launch space station";
            if (Prefs.DevMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = "Launch space station into orbit.",
                icon = ContentFinder<Texture2D>.Get("Things/Item/Satellite/RimNauts2_SpaceStation", true),
                action = new Action(launch_space_station)
            };
            if (!Prefs.DevMode) {
                if (FuelingPortSourceFuel < 150.0f)
                    cmd.Disable("Requires 150 fuel, currently at " + FuelingPortSourceFuel + " fuel.");
                else if (inventory.Count != 1 || inventory.ContentsString != "space station")
                    cmd.Disable("Can only send up 1 space station.");
            }
            yield return cmd;
        }

        public void launch_space_station() {
            if (!parent.Spawned) {
                Log.Error("Tried to launch " + parent + ", but it's unspawned.");
            } else {
                Map map = parent.Map;
                Transporter.TryRemoveLord(map);
                RimWorld.CompTransporter compTransporter = Transporter;
                Building fuelingPortSource = compTransporter.Launchable.FuelingPortSource;
                if (fuelingPortSource != null)
                    fuelingPortSource.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(150.0f);
                ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
                directlyHeldThings.ClearAndDestroyContents();
                RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
                activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, destroyLeftover: true);
                RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(RimWorld.ThingDefOf.DropPodLeaving, activeDropPod);
                flyShipLeaving.groupID = 0;
                flyShipLeaving.destinationTile = map.Tile;
                flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
                compTransporter.CleanUpLoadingVars(map);
                compTransporter.parent.Destroy(DestroyMode.Vanish);
                GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map);
                CameraJumper.TryHideWorld();

                int tile_id = -1;

                for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                    if (Find.World.grid.tiles.ElementAt(i).biome.defName == "RimNauts2_Satellite_Biome") {
                        if (tile_id == -1) {
                            tile_id = i;
                            break;
                        }
                    }
                }

                if (tile_id == -1) {
                    Messages.Message("Failed to launch space station into orbit.", RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Log.Error("RimNauts2: Couldn't find a free tile to spawn an artifical satellite on. Either map size is too small to spawn all the satellites or increase total satellite objects in settings");
                    return;
                }

                Satellite satellite = Generate_Satellites.add_satellite(tile_id, Satellite_Type.Space_Station);
                Find.World.grid.tiles.ElementAt(tile_id).elevation = 100f;
                Find.World.grid.tiles.ElementAt(tile_id).hilliness = RimWorld.Planet.Hilliness.Flat;
                Find.World.grid.tiles.ElementAt(tile_id).rainfall = 0f;
                Find.World.grid.tiles.ElementAt(tile_id).swampiness = 0f;
                Find.World.grid.tiles.ElementAt(tile_id).temperature = -80f;

                Map new_map = MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(satellite.Biome.defName), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
                foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                    if (weather.defName.Equals("OuterSpaceWeather")) {
                        new_map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                        if (Prefs.DevMode) Log.Message("RimNauts2: Found SOS2 space weather.");
                        break;
                    }
                }

                satellite.has_map = true;
                satellite.SetFaction(RimWorld.Faction.OfPlayer);
                Find.World.WorldUpdate();

                Messages.Message("Succesfully launched a space station into orbit.", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
        }
    }

    public class CompProperties_Launchable : CompProperties {
        public bool requireFuel = true;
        public ThingDef skyfallerLeaving;

        public CompProperties_Launchable() => compClass = typeof(CompLaunchable);
    }

    [StaticConstructorOnStartup]
    public class CompLaunchable : ThingComp {
        private RimWorld.CompTransporter cachedCompTransporter;
        public CompProperties_Launchable Props => (CompProperties_Launchable) props;
        public Building FuelingPortSource => RimWorld.FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(parent.Position, parent.Map);
        public bool ConnectedToFuelingPort => !Props.requireFuel || FuelingPortSource != null;
        public float FuelingPortSourceFuel => !ConnectedToFuelingPort ? 0.0f : FuelingPortSource.GetComp<RimWorld.CompRefuelable>().Fuel;

        public RimWorld.CompTransporter Transporter {
            get {
                if (cachedCompTransporter == null)
                    cachedCompTransporter = parent.GetComp<RimWorld.CompTransporter>();
                return cachedCompTransporter;
            }
        }

        public void launch_satellite() {
            if (!parent.Spawned) {
                Log.Error("Tried to launch " + parent + ", but it's unspawned.");
            } else {
                Map map = parent.Map;
                Transporter.TryRemoveLord(map);
                RimWorld.CompTransporter compTransporter = Transporter;
                Building fuelingPortSource = compTransporter.Launchable.FuelingPortSource;
                if (fuelingPortSource != null)
                    fuelingPortSource.TryGetComp<RimWorld.CompRefuelable>().ConsumeFuel(150.0f);
                ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
                directlyHeldThings.ClearAndDestroyContents();
                RimWorld.ActiveDropPod activeDropPod = (RimWorld.ActiveDropPod) ThingMaker.MakeThing(RimWorld.ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new RimWorld.ActiveDropPodInfo();
                activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, destroyLeftover: true);
                RimWorld.FlyShipLeaving flyShipLeaving = (RimWorld.FlyShipLeaving) RimWorld.SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving ?? RimWorld.ThingDefOf.DropPodLeaving, activeDropPod);
                flyShipLeaving.groupID = 0;
                flyShipLeaving.destinationTile = map.Tile;
                flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
                compTransporter.CleanUpLoadingVars(map);
                compTransporter.parent.Destroy(DestroyMode.Vanish);
                GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map);
                CameraJumper.TryHideWorld();

                int tile_id = -1;

                for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                    if (Find.World.grid.tiles.ElementAt(i).biome.defName == "RimNauts2_Satellite_Biome") {
                        if (tile_id == -1) {
                            tile_id = i;
                            break;
                        }
                    }
                }

                if (tile_id == -1) {
                    Messages.Message("Failed to launch satellite into orbit.", RimWorld.MessageTypeDefOf.NegativeEvent, true);
                    Log.Error("RimNauts2: Couldn't find a free tile to spawn an artifical satellite on. Either map size is too small to spawn all the satellites or increase total satellite objects in settings");
                    return;
                }

                Generate_Satellites.add_satellite(tile_id, Satellite_Type.Artifical_Satellite);

                Messages.Message("Succesfully launched a satellite into orbit.", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            CompLaunchable compLaunchable = this;
            ThingOwner inventory = Transporter.innerContainer;
            string label = "Launch satellite";
            if (Prefs.DevMode) label += " (Dev)";
            Command_Action cmd = new Command_Action {
                defaultLabel = label,
                defaultDesc = "Launch satellite into orbit.",
                icon = ContentFinder<Texture2D>.Get("Things/Item/Satellite/RimNauts2_Satellite", true),
                action = new Action(launch_satellite)
            };
            if (!Prefs.DevMode) {
                if (FuelingPortSourceFuel < 150.0f)
                    cmd.Disable("Requires 150 fuel, currently at " + FuelingPortSourceFuel + " fuel.");
                else if (inventory.Count != 1 || inventory.ContentsString != "satellite")
                    cmd.Disable("Can only send up 1 satellite.");
            }
            yield return cmd;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class TransportpodSatelliteIgnoreMaxRange {
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool to_moon = SatelliteDefOf.Satellite.Biomes.Contains(Find.World.grid.tiles.ElementAt(start).biome.defName);
            if (to_moon) {
                __result = 1;
                return;
            }
            bool from_moon = SatelliteDefOf.Satellite.Biomes.Contains(Find.World.grid.tiles.ElementAt(end).biome.defName);
            if (from_moon) {
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
