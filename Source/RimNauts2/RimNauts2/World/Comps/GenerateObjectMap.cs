using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2.World.Comps {
    public class GenerateObjectMap_Properties : RimWorld.WorldObjectCompProperties {
        public string label;
        public string desc;

        public GenerateObjectMap_Properties() => compClass = typeof(GenerateObjectMap);
    }

    public class GenerateObjectMap : RimWorld.Planet.WorldObjectComp {
        public GenerateObjectMap_Properties Props => (GenerateObjectMap_Properties) props;

        public override IEnumerable<Gizmo> GetGizmos() {
            ObjectHolder parent = this.parent as ObjectHolder;
            if (DebugSettings.godMode && !parent.HasMap && parent.map_generator != null) {
                yield return new Command_Action {
                    defaultLabel = Props.label,
                    defaultDesc = Props.desc,
                    action = generate_map,
                };
            }
        }

        public void generate_map() {
            ObjectHolder object_holder = parent as ObjectHolder;
            // generate map
            MapGenerator.GenerateMap(
                Find.World.info.initialMapSize,
                object_holder,
                object_holder.MapGeneratorDef,
                object_holder.ExtraGenStepDefs,
                extraInitBeforeContentGen: null
            );
            object_holder.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }
    }
}
