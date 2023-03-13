using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.PlaceWorker {
    public class PlaceWorker_RemoveRoof : Verse.PlaceWorker {
        public override AcceptanceReport AllowsPlacing(
            BuildableDef checkingDef,
            IntVec3 loc,
            Rot4 rot,
            Map map,
            Thing thingToIgnore = null,
            Thing thing = null
        ) {
            if (!loc.InBounds(map) || loc.Fogged(map)) return (AcceptanceReport) false;
            RoofDef roof_def = map.roofGrid.RoofAt(loc);
            if (roof_def == null || (roof_def != null && roof_def != Defs.Loader.roof_magnetic_field)) return (AcceptanceReport) false;
            return similar_roof_exists(map.thingGrid.ThingsListAt(loc)) ? (AcceptanceReport) false : (AcceptanceReport) true;
        }

        public override void DrawGhost(
            ThingDef def,
            IntVec3 center,
            Rot4 rot,
            Color ghostCol,
            Thing thing = null
        ) {
            Map currentMap = Find.CurrentMap;
            GenUI.RenderMouseoverBracket();
            currentMap.areaManager.BuildRoof.MarkForDraw();
            currentMap.areaManager.NoRoof.MarkForDraw();
            currentMap.roofGrid.Drawer.MarkForDraw();
        }

        public static bool similar_roof_exists(List<Thing> things) {
            if (!things.NullOrEmpty()) {
                foreach (Thing thing in things) {
                    if (thing.def == Defs.Loader.thing_roof_magnetic_field || thing.def == Defs.Loader.thing_unroof_magnetic_field) return true;
                }
            }
            return false;
        }
    }
}
