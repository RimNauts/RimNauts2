using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Building {
    public class SatelliteOperationsCenter : Verse.Building {
        private string descConnection = "Connecting...";

        private Universum.Defs.CelestialObject _relaySatelliteDef;
        private Texture2D _relaySatelliteIcon;
        private Command _relaySatelliteCmd;
        private int _relayTotal = 0;
        private int _relayTotalNeeded = 0;

        private Universum.Defs.CelestialObject _energyRelaySatelliteDef;
        private Texture2D _energyRelaySatelliteIcon;
        private Command _energyRelaySatelliteCmd;
        private int _energyRelayTotal = 0;
        private int _energyRelayCapacity = 0;
        private int _powerGenerated = 0;
        private int _powerNeeded = 0;

        public SatelliteOperationsCenter() {
            _relaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"];
            _relaySatelliteIcon = Universum.Assets.GetTexture(_relaySatelliteDef.icon.texturePath);

            _energyRelaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Energy"];
            _energyRelaySatelliteIcon = Universum.Assets.GetTexture(_energyRelaySatelliteDef.icon.texturePath);

            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
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

            _energyRelayTotal = Universum.Game.MainLoop.instance.GetTotal(_energyRelaySatelliteDef);
            _energyRelayCapacity = _energyRelayTotal * 50;
            _powerGenerated = _energyRelayTotal * 20;
            _powerNeeded = 0;
            // update commands
            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
            // update description
            descConnection = "Connected";
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            foreach (Gizmo gizmo in base.GetGizmos()) {
                yield return gizmo;
            }

            yield return _relaySatelliteCmd;
            yield return _energyRelaySatelliteCmd;
        }

        private void _void() { }

        private void _UpdateRelaySatelliteCmd() {
            string desc = "";
            desc += $"Total: {_relayTotal}\n";
            desc += $"Needed: {_relayTotalNeeded}";

            _relaySatelliteCmd = new Command_Action {
                defaultLabel = "Relay Satellite",
                defaultDesc = desc,
                icon = _relaySatelliteIcon,
                action = new Action(_void)
            };
        }

        private void _UpdateEnergyRelaySatelliteCmd() {
            string desc = "";
            desc += $"Total: {_energyRelayTotal}\n";
            desc += $"Relay Capacity: {_energyRelayCapacity}\n";
            desc += $"Power Needed/Generated: {_powerNeeded}/{_powerGenerated}";

            _energyRelaySatelliteCmd = new Command_Action {
                defaultLabel = "Energy Relay Satellite",
                defaultDesc = desc,
                icon = _energyRelaySatelliteIcon,
                action = new Action(_void)
            };
        }
    }
}
