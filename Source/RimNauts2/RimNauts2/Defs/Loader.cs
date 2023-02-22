using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [StaticConstructorOnStartup]
    public static class Loader {
        private static int total_defs;
        public static Dictionary<World.Type, List<ObjectHolder>> object_holders = new Dictionary<World.Type, List<ObjectHolder>>();

        public static void init() {
            foreach (ObjectHolder object_holder in DefDatabase<ObjectHolder>.AllDefs) {
                World.Type type = (World.Type) object_holder.type;
                if (!object_holders.ContainsKey(type)) object_holders.Add(type, new List<ObjectHolder>());
                object_holders[type].Add(object_holder);
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
    }
}
