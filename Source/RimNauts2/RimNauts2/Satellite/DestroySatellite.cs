using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Destroy : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public WorldObjectCompProperties_Destroy() => compClass = typeof(DestroyObjectButton);
    }

    public class DestroyObjectButton : RimWorld.Planet.WorldObjectComp {
        public WorldObjectCompProperties_Destroy Props => (WorldObjectCompProperties_Destroy) props;

        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;
            if (DebugSettings.godMode && !parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = Props.label,
                    defaultDesc = Props.desc,
                    action = destroy_object,
                };
            }
        }

        public void destroy_object() {
            Satellite parent = this.parent as Satellite;
            if (parent.type == Satellite_Type.Asteroid) parent.type = Satellite_Type.None;
            parent.Destroy();
        }
    }
}
