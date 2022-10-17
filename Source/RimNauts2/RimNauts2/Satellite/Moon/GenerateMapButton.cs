using System;
using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Settle : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Settle() => compClass = typeof(GenerateMapButton);
    }

    public class GenerateMapButton : RimWorld.Planet.WorldObjectComp {
        public static void generate_moon_map(Satellite parent) {
            Log.Message("OIOIOI: " + parent.type);
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;
            if (!parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => generate_moon_map(parent),
                };
            }
        }
    }
}
