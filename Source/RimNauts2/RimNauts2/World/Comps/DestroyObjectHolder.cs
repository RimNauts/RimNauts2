using System.Collections.Generic;
using Verse;

namespace RimNauts2.World.Comps {
    public class DestroyObjectHolder_Properties : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public DestroyObjectHolder_Properties() => compClass = typeof(DestroyObjectHolder);
    }

    public class DestroyObjectHolder : RimWorld.Planet.WorldObjectComp {
        public DestroyObjectHolder_Properties Props => (DestroyObjectHolder_Properties) props;

        public override IEnumerable<Gizmo> GetGizmos() {
            ObjectHolder parent = this.parent as ObjectHolder;
            if (DebugSettings.godMode && !parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = Props.label,
                    defaultDesc = Props.desc,
                    action = destroy_object,
                };
            }
        }

        public void destroy_object() {
            ObjectHolder parent = this.parent as ObjectHolder;
            parent.keep_after_abandon = false;
            parent.Destroy();
        }
    }
}
