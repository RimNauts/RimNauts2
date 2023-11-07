using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Building {
    public class SatelliteOperationsCenter : Verse.Building {
        private Universum.Defs.CelestialObject _relaySatelliteDef;
        private Command _relaySatelliteCmd;
        private Texture2D _relaySatelliteIcon;
        private int _relayTotal = 0;
        private int _relayTotalNeeded = 0;
        private string descConnection = "Connecting...";

        public SatelliteOperationsCenter() {
            _relaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"];
            _relaySatelliteIcon = Universum.Assets.GetTexture(_relaySatelliteDef.icon.texturePath);

            _UpdateRelaySatelliteCmd();
        }

        public override string GetInspectString() {
            string desc = base.GetInspectString();
            if (!desc.NullOrEmpty()) descConnection += "\n\n";

            return descConnection + base.GetInspectString();
        }

        public override void TickRare() {
            base.TickRare();
            // update properties
            _relayTotal = Universum.Game.MainLoop.instance.GetTotal(_relaySatelliteDef);
            _relayTotalNeeded = 0;
            // update commands
            _UpdateRelaySatelliteCmd();
            // update description
            descConnection = "Connected";
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            foreach (Gizmo gizmo in base.GetGizmos()) {
                yield return gizmo;
            }

            yield return _relaySatelliteCmd;
        }

        private void _void() { }

        private void _UpdateRelaySatelliteCmd() {
            _relaySatelliteCmd = new Command_Action {
                defaultLabel = "Relay Satellite",
                defaultDesc = $"Total: \t{_relayTotal}\nNeeded: \t{_relayTotalNeeded}",
                icon = _relaySatelliteIcon,
                action = new Action(_void)
            };
        }
    }
}
