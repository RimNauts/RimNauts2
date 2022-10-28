using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Destroy : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Destroy() => compClass = typeof(DestroyObjectButton);
    }

    public class DestroyObjectButton : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (Prefs.DevMode &&!parent.HasMap) {
                yield return new Command_Action {
                    defaultLabel = "Destroy",
                    defaultDesc = "Blast the object out of orbit, never to be seen again.",
                    action = destroy_object,
                };
            }
        }

        public void destroy_object() {
            Satellite parent = this.parent as Satellite;
            parent.Destroy();
        }
    }
}
