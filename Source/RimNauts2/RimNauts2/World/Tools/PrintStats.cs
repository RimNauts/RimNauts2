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
            string msg = Info.name + " Stats:";
            foreach (int type in Enum.GetValues(typeof(Type))) {
                if (type == 0) continue;
                msg += Style.tab + ((Type) type) + ": " + RenderingManager.get_total((Type) type) + "\n";
            }
            Log.Message(msg);
        }
    }
}
