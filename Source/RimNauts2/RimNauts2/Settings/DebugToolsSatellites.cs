using System.Collections.Generic;
using Verse;

namespace RimNauts2 {
    public static class DebugToolsSatellites {
        [DebugAction("Spawning", "RimNauts2: Regenerate objects", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
        public static void RegenerateSatellites() {
            Generate_Satellites.regenerate_satellites();
        }
        [DebugAction("Spawning", "RimNauts2: Stats", false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
        public static void PrintStats() {
            int asteroid_ore = 0;
            int crashing_asteroid = 0;
            foreach (KeyValuePair<int, Satellite> satellite in RimNauts_GameComponent.satellites) {
                if (satellite.Value.mineral_rich) asteroid_ore++;
                if (satellite.Value.can_out_of_bounds) crashing_asteroid++;
            }
            Verse.Log.Message("RimNauts.mineral_rich_asteroids".Translate() + ": " + asteroid_ore.ToString());
            Verse.Log.Message("RimNauts.crashing_asteroids".Translate() + ": " + crashing_asteroid.ToString());
        }
    }
}
