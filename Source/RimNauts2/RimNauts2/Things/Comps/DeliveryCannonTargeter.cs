using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Comps {
    public class DeliveryCannonTargeter_Properties : CompProperties {
        public DeliveryCannonTargeter_Properties() => compClass = typeof(Targeter);
    }

    public class Targeter : ThingComp {
        public DeliveryCannonTargeter_Properties Props => (DeliveryCannonTargeter_Properties) props;
        public int target_tile = -1;
        public IntVec3 target_cell = new IntVec3(-1, -1, -1);
        public bool valid = false;

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref target_tile, "target_tile", -1);
            Scribe_Values.Look(ref target_cell, "target_cell", new IntVec3(-1, -1, -1));
            Scribe_Values.Look(ref valid, "valid", false);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            Command_Action cmd = new Command_Action {
                defaultLabel = "Choose target",
                defaultDesc = "",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true),
                action = new Action(target)
            };
            yield return cmd;
        }

        public void target() {
            CameraJumper.TryJump(CameraJumper.GetWorldTarget((RimWorld.Planet.GlobalTargetInfo) parent));
            Find.WorldSelector.ClearSelection();
            Find.WorldTargeter.BeginTargeting(
                action: new Func<RimWorld.Planet.GlobalTargetInfo, bool>(get_tile_target),
                canTargetTiles: false
            );
        }

        public bool get_tile_target(RimWorld.Planet.GlobalTargetInfo target) {
            if (!target.IsValid) {
                Messages.Message((string) "MessageTransportPodsDestinationIsInvalid".Translate(), RimWorld.MessageTypeDefOf.RejectInput, false);
                return false;
            }
            Map target_map = get_map(target.Tile);
            if (target_map == null) return false;

            CameraJumper.TryJump(new RimWorld.Planet.GlobalTargetInfo(new IntVec3(target_map.Size.x / 2, 0, target_map.Size.z / 2), target_map));

            Find.Targeter.BeginTargeting(
                new RimWorld.TargetingParameters {
                    canTargetLocations = true,
                    canTargetSelf = false,
                    canTargetPawns = false,
                    canTargetFires = false,
                    canTargetBuildings = false,
                    canTargetItems = false,
                    validator = (TargetInfo x) => RimWorld.DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, allowFogged: false, canRoofPunch: true)
                },
                action: x => chose_cell_target(target.Tile, x),
                highlightAction: x => GenDraw.DrawTargetHighlight(x),
                targetValidator: x => RimWorld.DropCellFinder.IsGoodDropSpot(x.Cell, target_map, allowFogged: false, canRoofPunch: true)
            );
            return true;
        }

        public void chose_cell_target(int tile, LocalTargetInfo target) {
            target_tile = tile;
            target_cell = target.Cell;
            valid = true;
            CameraJumper.TryJump(new RimWorld.Planet.GlobalTargetInfo(parent.Position, parent.Map));
        }

        public bool valid_target() {
            if (!valid) return false;
            if (target_tile == -1) {
                target_cell = new IntVec3(-1, -1, -1);
                valid = false;
                return false;
            }
            if (target_cell.x == -1 || target_cell.y == -1 || target_cell.z == -1) {
                target_tile = -1;
                target_cell = new IntVec3(-1, -1, -1);
                valid = false;
                return false;
            }
            Map map = get_map(target_tile);
            if (map == null) {
                target_tile = -1;
                target_cell = new IntVec3(-1, -1, -1);
                valid = false;
                return false;
            }
            if (!RimWorld.DropCellFinder.IsGoodDropSpot(target_cell, map, allowFogged: false, canRoofPunch: true)) {
                target_tile = -1;
                target_cell = new IntVec3(-1, -1, -1);
                valid = false;
                return false;
            }
            return true;
        }

        public Map get_map(int tile) {
            foreach (Map map in Find.Maps) {
                if (map.Tile == tile) return map;
            }
            return null;
        }

        public override string CompInspectStringExtra() {
            return "Valid target: " + valid;
        }

        public override IEnumerable<RimWorld.StatDrawEntry> SpecialDisplayStats() {
            yield return new RimWorld.StatDrawEntry(
                RimWorld.StatCategoryDefOf.Building,
                "Target tile: ",
                target_tile.ToString(),
                "Tile on the world map to target.",
                3171
            );
            yield return new RimWorld.StatDrawEntry(
                RimWorld.StatCategoryDefOf.Building,
                "Target cell: ",
                target_cell.ToString(),
                "Position on map to target.",
                3171
            );
        }
    }
}
