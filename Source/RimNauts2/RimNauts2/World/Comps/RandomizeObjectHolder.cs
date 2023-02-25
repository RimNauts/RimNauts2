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
            parent.visual_object.orbit_position = parent.visual_object.type.orbit_position();
            parent.visual_object.orbit_speed = parent.visual_object.type.orbit_speed();
            float size = parent.visual_object.type.size();
            parent.visual_object.draw_size = new Vector3(size, 1.0f, size);
            parent.visual_object.period = (int) (36000.0f + (6000.0f * (Rand.Value - 0.5f)));
            parent.visual_object.time_offset = Rand.Range(0, parent.visual_object.period);
            parent.visual_object.orbit_direction = parent.visual_object.type.orbit_direction();
            parent.visual_object.color = parent.visual_object.type.color();
            parent.visual_object.rotation_angle = parent.visual_object.type.rotation_angle();
            parent.visual_object.current_position = parent.visual_object.orbit_position;
            parent.visual_object.rotation = Quaternion.AngleAxis(parent.visual_object.rotation_angle, Vector3.up);
            parent.visual_object.material = null;
            parent.visual_object.get_material();
            parent.visual_object.update_when_unpaused();
            parent.visual_object.update();
            RenderingManager.force_update = true;
        }
    }
}
