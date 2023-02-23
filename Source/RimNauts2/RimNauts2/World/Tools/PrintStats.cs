using System;
using Verse;

namespace RimNauts2.World.Tools {
    public static class PrintStats {
        [DebugAction(
            category: "RimNauts 2",
            name: "Print stats",
            displayPriority: 0,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void print_stats() {
            Log.Message("\n" + Info.name + " Stats:");
            foreach (int type in Enum.GetValues(typeof(Type))) {
                if (type == 0) continue;
                Log.Message(Style.tab + ((Type) type) + ": " + Caching_Handler.render_manager.get_total((Type) type));
            }
            Log.Message("");
        }
    }
}
