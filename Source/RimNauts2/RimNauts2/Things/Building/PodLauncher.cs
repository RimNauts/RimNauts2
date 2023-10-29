using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimNauts2.Things.Building {
    public class PodLauncher : Verse.Building {
        public override IEnumerable<Gizmo> GetGizmos() {
            foreach (Gizmo gizmo in base.GetGizmos()) {
                yield return gizmo;
            }

            AcceptanceReport acceptanceReport = GenConstruct.CanPlaceBlueprintAt(Defs.Loader.thing_cargo_pod, FuelingPortUtility.GetFuelingPortCell(this), Defs.Loader.thing_cargo_pod.defaultPlacingRot, Map);
            Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(Defs.Loader.thing_cargo_pod);
            if (designator_Build != null) {
                Command_Action command_Action = new Command_Action {
                    defaultLabel = "BuildThing".Translate(Defs.Loader.thing_cargo_pod.label),
                    icon = designator_Build.icon,
                    defaultDesc = designator_Build.Desc,
                    action = delegate {
                        IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
                        GenConstruct.PlaceBlueprintForBuild(Defs.Loader.thing_cargo_pod, fuelingPortCell, Map, Defs.Loader.thing_cargo_pod.defaultPlacingRot, Faction.OfPlayer, null);
                    }
                };
                if (!acceptanceReport.Accepted) {
                    command_Action.Disable(acceptanceReport.Reason);
                }

                yield return command_Action;
            }
        }
    }
}
