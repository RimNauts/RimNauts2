using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [StaticConstructorOnStartup]
    public static class Loader {
        private static int total_defs;
        public static Dictionary<World.Type, List<ObjectHolder>> object_holders = new Dictionary<World.Type, List<ObjectHolder>>();
        public static Dictionary<World.Type, ObjectMetadata> object_metadata = new Dictionary<World.Type, ObjectMetadata>();
        public static Dictionary<World.Type, ObjectGenerationStep> object_generation_steps = new Dictionary<World.Type, ObjectGenerationStep>();
        public static RoofDef roof_magnetic_field = DefDatabase<RoofDef>.GetNamed("RimNauts2_Roof_MagneticField");
        public static ThingDef thing_roof_magnetic_field = DefDatabase<ThingDef>.GetNamed("RimNauts2_Things_Roof_MagneticField");
        public static ThingDef thing_unroof_magnetic_field = DefDatabase<ThingDef>.GetNamed("RimNauts2_Things_UnRoof_MagneticField");

        public static void init() {
            foreach (ObjectHolder object_holder in DefDatabase<ObjectHolder>.AllDefs) {
                World.Type type = (World.Type) object_holder.type;
                if (!object_holders.ContainsKey(type)) object_holders.Add(type, new List<ObjectHolder>());
                object_holders[type].Add(object_holder);
                total_defs++;
            }
            foreach (ObjectMetadata metadata in DefDatabase<ObjectMetadata>.AllDefs) {
                World.Type type = (World.Type) metadata.type;
                if (object_metadata.ContainsKey(type)) continue;
                object_metadata.Add(type, metadata);
                total_defs++;
            }
            foreach (ObjectGenerationStep object_generation_step in DefDatabase<ObjectGenerationStep>.AllDefs) {
                World.Type type = (World.Type) object_generation_step.type;
                if (object_generation_steps.ContainsKey(type)) continue;
                object_generation_steps.Add(type, object_generation_step);
                total_defs++;
            }
            // print mod info
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.def_loader_done",
                prefix: Style.tab,
                args: new NamedArgument[] { total_defs }
            );
        }

        public static ObjectHolder get_object_holder(World.Type type, string def_name = null) {
            if (!object_holders.ContainsKey(type)) return null;
            if (def_name == null) return object_holders[type].RandomElement();
            return object_holders[type].Find(object_holder => object_holder.defName == def_name);
        }

        public static ObjectMetadata get_object_metadata(World.Type type) {
            if (!object_metadata.ContainsKey(type)) return null;
            return object_metadata[type];
        }
    }
}
