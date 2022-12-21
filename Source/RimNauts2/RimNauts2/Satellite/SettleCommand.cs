using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.SettleInEmptyTileUtility), nameof(RimWorld.Planet.SettleInEmptyTileUtility.SettleCommand))]
    public static class SettleCommand {
        public static bool Prefix(RimWorld.Planet.Caravan caravan, ref Command __result) {
            bool not_satellite = !SatelliteContainer.exists(caravan.Tile);
            if (not_satellite) return true;
            Satellite satellite = SatelliteContainer.get(caravan.Tile);
            if (satellite.HasMap) return true;
            if (satellite.type == Satellite_Type.Moon) {
                Command_Settle commandSettle = new Command_Settle {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => settle(caravan)
                };
                __result = commandSettle;
                return false;
            } else if (satellite.type == Satellite_Type.Asteroid_Ore) {
                Command_Settle commandSettle = new Command_Settle {
                    defaultLabel = "DesignatorMine".Translate(),
                    defaultDesc = "",
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine"),
                    action = () => settle(caravan, change_world_object: false)
                };
                __result = commandSettle;
                return false;
            } else return true;
        }

        public static void settle(RimWorld.Planet.Caravan caravan, bool change_world_object = true) {
            RimWorld.Faction faction = caravan.Faction;
            if (faction != RimWorld.Faction.OfPlayer) {
                Log.Error("Cannot settle with non-player faction.");
            } else {
                if (Find.AnyPlayerHomeMap == null) {
                    foreach (Pawn podsAliveColonist in RimWorld.PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists) {
                        RimWorld.MemoryThoughtHandler memories = podsAliveColonist.needs?.mood?.thoughts?.memories;
                        if (memories != null) {
                            memories.RemoveMemoriesOfDef(RimWorld.ThoughtDefOf.NewColonyOptimism);
                            memories.RemoveMemoriesOfDef(RimWorld.ThoughtDefOf.NewColonyHope);
                            if (podsAliveColonist.IsFreeNonSlaveColonist)
                                memories.TryGainMemory(RimWorld.ThoughtDefOf.NewColonyOptimism);
                        }
                    }
                }
                Satellite satellite = SatelliteContainer.get(caravan.Tile);
                if (change_world_object) {
                    SatelliteSettings satellite_settings = Generate_Satellites.copy_satellite(satellite, satellite.def_name + "_Base", Satellite_Type.Moon);
                    satellite.type = Satellite_Type.Buffer;
                    satellite.Destroy();
                    Satellite new_satellite = Generate_Satellites.paste_satellite(satellite_settings);
                    satellite = new_satellite;
                }
                // generate map
                Map map = MapGenerator.GenerateMap(Find.World.info.initialMapSize, satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
                satellite.SetFaction(RimWorld.Faction.OfPlayer);
                Find.World.WorldUpdate();

                LongEventHandler.QueueLongEvent(() => {
                    Pawn pawn = caravan.PawnsListForReading[0];
                    RimWorld.Planet.CaravanEnterMapUtility.Enter(caravan, map, RimWorld.Planet.CaravanEnterMode.Center, RimWorld.Planet.CaravanDropInventoryMode.DropInstantly, extraCellValidator: x => x.GetRoom(map).CellCount >= 600);
                    CameraJumper.TryJump(pawn);
                }, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
            }
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.CaravanTweenerUtility), nameof(RimWorld.Planet.CaravanTweenerUtility.PatherTweenedPosRoot))]
    public static class CaravanTweenerUtility_PatherTweenedPosRoot {
        public static bool Prefix(RimWorld.Planet.Caravan caravan, ref Vector3 __result) {
            if (SatelliteContainer.exists(caravan.Tile)) {
                __result = SatelliteContainer.get(caravan.Tile).get_parametric_ellipse();
                return false;
            }
            return true;
        }
    }
}
