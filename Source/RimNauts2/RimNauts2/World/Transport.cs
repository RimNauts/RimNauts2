using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;

namespace RimNauts2 {
    public class CompProperties_Launchable : CompProperties {
        public bool requireFuel = true;
        public int fixedLaunchDistanceMax = -1;
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
                int groupId = Transporter.groupID;
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
                flyShipLeaving.groupID = groupId;
                flyShipLeaving.destinationTile = map.Tile;
                flyShipLeaving.worldObjectDef = RimWorld.WorldObjectDefOf.TravelingTransportPods;
                compTransporter.CleanUpLoadingVars(map);
                compTransporter.parent.Destroy(DestroyMode.Vanish);
                GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map);
                CameraJumper.TryHideWorld();
                Messages.Message("Succesfully launched a satellite into orbit.", RimWorld.MessageTypeDefOf.PositiveEvent, true);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            CompLaunchable compLaunchable = this;
            ThingOwner inventory = Transporter.innerContainer;
            Command_Action cmd = new Command_Action();
            cmd.defaultLabel = "Launch satellite";
            cmd.defaultDesc = "Launch satellite into orbit.";
            cmd.icon = RimWorld.CompLaunchable.LaunchCommandTex;
            cmd.action = new Action(launch_satellite);
            if (FuelingPortSourceFuel < 150.0f)
                cmd.Disable("Requires 150 fuel, currently at " + FuelingPortSourceFuel + " fuel.");
            else if (inventory.Count != 1 || inventory.ContentsString != "satellite")
                cmd.Disable("Can only send up 1 satellite.");
            yield return cmd;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldGrid), nameof(RimWorld.Planet.WorldGrid.TraversalDistanceBetween))]
    public static class TransportpodSatelliteIgnoreMaxRange {
        [HarmonyPostfix]
        public static void Postfix(int start, int end, bool passImpassable, int maxDist, ref int __result) {
            bool to_moon = Find.World.grid.tiles.ElementAt(start).biome == BiomeDefOf.RockMoonBiome;
            if (to_moon) {
                __result = 1;
                return;
            }
            bool from_moon = Find.World.grid.tiles.ElementAt(end).biome == BiomeDefOf.RockMoonBiome;
            if (from_moon) {
                __result = 1;
                return;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("Start", MethodType.Getter)]
    public static class TransportpodFromSatelliteAnimation {
        [HarmonyPostfix]
        public static void StartAtShip(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {
            int num = (int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == num) __result = worldObject.DrawPos;
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.TravelingTransportPods))]
    [HarmonyPatch("End", MethodType.Getter)]
    public static class TransportpodToSatelliteAnimation {
        [HarmonyPostfix]
        public static void EndAtShip(RimWorld.Planet.TravelingTransportPods __instance, ref Vector3 __result) {

            int destinationTile = __instance.destinationTile;
            foreach (RimWorld.Planet.WorldObject worldObject in from o in Find.World.worldObjects.AllWorldObjects
                                                                where o is Satellite
                                                                select o) {
                if (worldObject.Tile == destinationTile) __result = worldObject.DrawPos;
            }
        }
    }
}
