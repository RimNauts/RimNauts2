using Verse;

namespace RimNauts2.Things.PlaceWorker {
    public class IsBarrenMoon : Verse.PlaceWorker {
        public override AcceptanceReport AllowsPlacing(
            BuildableDef checkingDef,
            IntVec3 loc,
            Rot4 rot,
            Map map,
            Thing thingToIgnore = null,
            Thing thing = null
        ) {
            if (map.Biome == Defs.Loader.biome_barren_moon) return (AcceptanceReport) true;
            return new AcceptanceReport("RimNauts.can_only_be_placed".Translate("RimNauts.barren_moon".Translate()));
        }
    }
}
