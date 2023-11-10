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

        private Universum.Defs.CelestialObject _cosmicSurveillanceSatelliteDef;
        private Texture2D _cosmicSurveillanceSatelliteIcon;
        private Command _cosmicSurveillanceSatelliteCmd;
        private int _cosmicSurveillanceTotal = 0;
        private int _cosmicSurveillancePowerConsumption = 0;

        public SatelliteOperationsCenter() {
            _relaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"];
            _relaySatelliteIcon = Universum.Assets.GetTexture(_relaySatelliteDef.icon.texturePath);

            _energyRelaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Energy"];
            _energyRelaySatelliteIcon = Universum.Assets.GetTexture(_energyRelaySatelliteDef.icon.texturePath);

            _cosmicSurveillanceSatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_CosmicSurveillance"];
            _cosmicSurveillanceSatelliteIcon = Universum.Assets.GetTexture(_cosmicSurveillanceSatelliteDef.icon.texturePath);

            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
            _UpdateCosmicSurveillanceSatelliteCmd();
        }

        public override string GetInspectString() {
            string desc = base.GetInspectString();
            if (!desc.NullOrEmpty()) descConnection += "\n\n";

            return descConnection + base.GetInspectString();
        }

        public override void TickRare() {
            base.TickRare();
            // update properties
            _relayTotalNeeded = 0;
            _powerNeeded = 0;

            _relayTotal = Universum.Game.MainLoop.instance.GetTotal(_relaySatelliteDef);

            _energyRelayTotal = Universum.Game.MainLoop.instance.GetTotal(_energyRelaySatelliteDef);
            _energyRelayCapacity = _energyRelayTotal * 50;
            _powerGenerated = _energyRelayTotal * 20;

            _cosmicSurveillanceTotal = Universum.Game.MainLoop.instance.GetTotal(_cosmicSurveillanceSatelliteDef);
            _cosmicSurveillancePowerConsumption = _cosmicSurveillanceTotal * 50;
            _relayTotalNeeded += _cosmicSurveillanceTotal;
            _powerNeeded += _cosmicSurveillancePowerConsumption;
            // update commands
            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
            _UpdateCosmicSurveillanceSatelliteCmd();
            // update description
            descConnection = "Connected";
        }

        public override IEnumerable<Gizmo> GetGizmos() {
            foreach (Gizmo gizmo in base.GetGizmos()) {
                yield return gizmo;
            }

            yield return _relaySatelliteCmd;
            yield return _energyRelaySatelliteCmd;
            yield return _cosmicSurveillanceSatelliteCmd;
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

            if (_relayTotalNeeded > _relayTotal) _relaySatelliteCmd.defaultIconColor = Color.red;
        }

        private void _UpdateEnergyRelaySatelliteCmd() {
            string desc = "";
            desc += $"Total: {_energyRelayTotal}\n";
            desc += $"Energy Relay Capacity: {_energyRelayCapacity}\n";
            desc += $"Power Needed/Generated: {_powerNeeded}/{_powerGenerated}\n";
            desc += $"Excess Power: {_powerGenerated - _powerNeeded}";

            _energyRelaySatelliteCmd = new Command_Action {
                defaultLabel = "Energy Relay Satellite",
                defaultDesc = desc,
                icon = _energyRelaySatelliteIcon,
                action = new Action(_void)
            };

            if (_powerNeeded > _powerGenerated || _powerNeeded > _energyRelayCapacity) _energyRelaySatelliteCmd.defaultIconColor = Color.red;
        }

        private void _UpdateCosmicSurveillanceSatelliteCmd() {
            string desc = "";
            desc += $"Total: {_cosmicSurveillanceTotal}\n";
            desc += $"Total Power Consumption: {_cosmicSurveillancePowerConsumption}";

            _cosmicSurveillanceSatelliteCmd = new Command_Action {
                defaultLabel = "Cosmic Surveillanc Satellite",
                defaultDesc = desc,
                icon = _cosmicSurveillanceSatelliteIcon,
                action = new Action(_void)
            };
        }
    }
}
