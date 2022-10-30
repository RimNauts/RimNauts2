using Verse;

namespace RimNauts2 {
    public static class DebugToolsSatellites {
        [DebugAction("Spawning", "RimNauts2: Regenerate objects", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
        public static void RegenerateSatellites() {
            Generate_Satellites.regenerate_satellites();
        }
    }
}
