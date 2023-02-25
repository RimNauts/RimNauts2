using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.SettleInEmptyTileUtility), nameof(RimWorld.Planet.SettleInEmptyTileUtility.SettleCommand))]
    public static class SettleInEmptyTileUtility_SettleCommand {
        public static bool Prefix(RimWorld.Planet.Caravan caravan, ref Command __result) {
            bool not_object_holder = !Cache.exists(caravan.Tile);
            if (not_object_holder) return true;
            ObjectHolder object_holder = Cache.get(caravan.Tile);
            if (object_holder.HasMap) return true;
            if (object_holder.type == Type.Moon) {
                Command_Settle commandSettle = new Command_Settle {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => settle(caravan)
                };
                __result = commandSettle;
                return false;
            } else if (object_holder.type == Type.AsteroidOre) {
                Command_Settle commandSettle = new Command_Settle {
                    defaultLabel = "DesignatorMine".Translate(),
                    defaultDesc = "",
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine"),
                    action = () => settle(caravan)
                };
                __result = commandSettle;
                return false;
            } else return true;
        }

        public static void settle(RimWorld.Planet.Caravan caravan) {
            RimWorld.Faction faction = caravan.Faction;
            if (faction != RimWorld.Faction.OfPlayer) {
                Logger.print(
                    Logger.Importance.Error,
                    key: "RimNauts.Error.cannot_settle_nonplayer",
                    prefix: Style.name_prefix
                );
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
                ObjectHolder object_holder = Cache.get(caravan.Tile);
                string object_holder_def = null;
                Defs.ObjectHolder defs = Defs.Loader.get_object_holder(object_holder.type, object_holder_def);
                if (defs == null) return;
                Find.World.grid.tiles.ElementAt(object_holder.Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(defs.biome_def);
                // generate map
                Map map = MapGenerator.GenerateMap(
                    Find.World.info.initialMapSize,
                    object_holder,
                    object_holder.MapGeneratorDef,
                    object_holder.ExtraGenStepDefs,
                    extraInitBeforeContentGen: null
                );
                object_holder.SetFaction(RimWorld.Faction.OfPlayer);
                Find.World.WorldUpdate();
                // spawn colonist
                LongEventHandler.QueueLongEvent(() => {
                    Pawn pawn = caravan.PawnsListForReading[0];
                    RimWorld.Planet.CaravanEnterMapUtility.Enter(
                        caravan,
                        map,
                        RimWorld.Planet.CaravanEnterMode.Center,
                        RimWorld.Planet.CaravanDropInventoryMode.DropInstantly,
                        extraCellValidator: x => x.GetRoom(map).CellCount >= 600
                    );
                    CameraJumper.TryJump(pawn);
                }, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
            }
        }
    }
}
