using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.World.Comps {
    public class RandomizeObjectHolder_Properties : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public RandomizeObjectHolder_Properties() => compClass = typeof(RandomizeObjectHolder);
    }

    public class RandomizeObjectHolder : RimWorld.Planet.WorldObjectComp {
        public RandomizeObjectHolder_Properties Props => (RandomizeObjectHolder_Properties) props;

        public override IEnumerable<Gizmo> GetGizmos() {
            ObjectHolder parent = this.parent as ObjectHolder;
            if (DebugSettings.godMode) {
                yield return new Command_Action {
                    defaultLabel = Props.label,
                    defaultDesc = Props.desc,
                    action = randomize_object,
                };
            }
        }

        public void randomize_object() {
            ObjectHolder parent = this.parent as ObjectHolder;
            parent.visual_object.randomize();
            RenderingManager.force_update = true;
        }
    }
}
