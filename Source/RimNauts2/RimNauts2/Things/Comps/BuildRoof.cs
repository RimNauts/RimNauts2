using Verse;

namespace RimNauts2.Things.Comps {
    public class CompProperties_BuildRoof : CompProperties {
        public RoofDef roof_def;

        public CompProperties_BuildRoof() => compClass = typeof(BuildRoof);
    }

    public class BuildRoof : ThingComp {
        public CompProperties_BuildRoof Props => (CompProperties_BuildRoof) props;

        public override void PostSpawnSetup(bool respawningAfterLoad) {
            base.PostSpawnSetup(respawningAfterLoad);
            if (respawningAfterLoad) return;
            RoofDef roof_def = parent.Map.roofGrid.RoofAt(parent.Position);
            if (roof_def == null || roof_def != null && roof_def.defName != Props.roof_def.defName) {
                parent.Map.roofGrid.SetRoof(parent.Position, Props.roof_def);
                RimWorld.MoteMaker.PlaceTempRoof(parent.Position, parent.Map);
            }
        }

        public override void CompTick() {
            base.CompTick();
            if (parent.Destroyed) return;
            parent.Destroy(DestroyMode.Vanish);
        }
    }
}
