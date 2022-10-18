using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Settle : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Settle() => compClass = typeof(GenerateMapButton);
    }

    public class GenerateMapButton : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;
            if (!parent.has_map) {
                yield return new Command_Action {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => Moon.generate_moon(parent),
                };
            }
        }
    }
}
