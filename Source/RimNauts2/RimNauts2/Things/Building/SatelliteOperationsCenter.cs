using System.Collections.Generic;
using Verse;

namespace RimNauts2.Things.Building {
    public class SatelliteOperationsCenter : Verse.Building {
        private string descConnection = "Connecting...";
        private bool disconnected = true;

        public override string GetInspectString() {
            string desc = base.GetInspectString();
            if (!desc.NullOrEmpty()) descConnection += "\n\n";

            return descConnection + base.GetInspectString();
        }

        public override void TickRare() {
            base.TickRare();

            if (disconnected) {
                disconnected = false;
                descConnection = "Connected";
            }
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            foreach (Gizmo gizmo in base.GetGizmos()) {
                yield return gizmo;
            }

            if (!disconnected) {
                foreach (var satelliteCommand in Game.MainLoop.instance.satelliteCommands) yield return satelliteCommand.cmd;
            }
        }
    }
}
