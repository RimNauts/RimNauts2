using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimNauts2.Things.Building {
    public class SatelliteOperationsCenter : Verse.Building {
        private string descConnection = "Connecting...";

        private Universum.Defs.CelestialObject _relaySatelliteDef;
        private ThingDef _relaySatelliteThingDef;
        private Texture2D _relaySatelliteIcon;
        private Command _relaySatelliteCmd;
        private int _relayTotal = 0;
        private int _relayCapacity = 0;
        private int _relayTotalNeeded = 0;

        private Universum.Defs.CelestialObject _energyRelaySatelliteDef;
        private ThingDef _energyRelaySatelliteThingDef;
        private Texture2D _energyRelaySatelliteIcon;
        private Command _energyRelaySatelliteCmd;
        private int _energyRelayTotal = 0;
        private int _energyRelayCapacity = 0;
        private int _powerGenerated = 0;
        private int _powerNeeded = 0;

        private Universum.Defs.CelestialObject _energyBoosterSatelliteDef;
        private ThingDef _energyBoosterSatelliteThingDef;
        private Texture2D _energyBoosterSatelliteIcon;
        private Command _energyBoosterSatelliteCmd;
        private int _energyBoosterTotal = 0;
        private int _energyBoosterPowerGenerated = 0;

        private Universum.Defs.CelestialObject _cosmicSurveillanceSatelliteDef;
        private ThingDef _cosmicSurveillanceSatelliteThingDef;
        private Texture2D _cosmicSurveillanceSatelliteIcon;
        private Command _cosmicSurveillanceSatelliteCmd;
        private int _cosmicSurveillanceTotal = 0;
        private int _cosmicSurveillancePowerConsumption = 0;

        public SatelliteOperationsCenter() {
            _relaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"];
            _relaySatelliteThingDef = ThingDef.Named("RimNauts2_Module_Satellite");
            _relaySatelliteIcon = Universum.Assets.GetTexture(_relaySatelliteDef.icon.texturePath);

            _energyRelaySatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Energy"];
            _energyRelaySatelliteThingDef = ThingDef.Named("RimNauts2_Module_Satellite_Energy");
            _energyRelaySatelliteIcon = Universum.Assets.GetTexture(_energyRelaySatelliteDef.icon.texturePath);

            _energyBoosterSatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_EnergyBooster"];
            _energyBoosterSatelliteThingDef = ThingDef.Named("RimNauts2_Module_Satellite_EnergyBooster");
            _energyBoosterSatelliteIcon = Universum.Assets.GetTexture(_energyBoosterSatelliteDef.icon.texturePath);

            _cosmicSurveillanceSatelliteDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_CosmicSurveillance"];
            _cosmicSurveillanceSatelliteThingDef = ThingDef.Named("RimNauts2_Module_Satellite_CosmicSurveillance");
            _cosmicSurveillanceSatelliteIcon = Universum.Assets.GetTexture(_cosmicSurveillanceSatelliteDef.icon.texturePath);

            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
            _UpdateEnergyBoosterSatelliteCmd();
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
            _relayCapacity = _relayTotal * 4;

            _energyRelayTotal = Universum.Game.MainLoop.instance.GetTotal(_energyRelaySatelliteDef);
            _energyRelayCapacity = _energyRelayTotal * 50;
            _powerGenerated = _energyRelayTotal * 20;

            _energyBoosterTotal = Universum.Game.MainLoop.instance.GetTotal(_energyBoosterSatelliteDef);
            _energyBoosterPowerGenerated = _energyBoosterTotal * 360;

            _cosmicSurveillanceTotal = Universum.Game.MainLoop.instance.GetTotal(_cosmicSurveillanceSatelliteDef);
            _cosmicSurveillancePowerConsumption = _cosmicSurveillanceTotal * 50;
            _relayTotalNeeded += _cosmicSurveillanceTotal;
            _powerNeeded += _cosmicSurveillancePowerConsumption;
            _powerGenerated += _energyBoosterPowerGenerated;
            // update commands
            _UpdateRelaySatelliteCmd();
            _UpdateEnergyRelaySatelliteCmd();
            _UpdateEnergyBoosterSatelliteCmd();
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
            yield return _energyBoosterSatelliteCmd;
            yield return _cosmicSurveillanceSatelliteCmd;
        }

        private void _Void() { }

        private void _UpdateRelaySatelliteCmd() {
            bool needCapacity = _relayTotalNeeded > _relayCapacity;

            bool problemDetected = needCapacity;

            string desc = _BuildHeader("Relay Satellite");
            desc += _BuildOverview(_relaySatelliteThingDef.description);
            desc += _AddDetail($"Total Satellites: {_relayTotal}");
            desc += _AddDetail($"Relay Demand/Capacity: {_relayTotalNeeded}/{_relayCapacity} ({_GetSurplusString(surplus: _relayCapacity - _relayTotalNeeded)})");
            if (problemDetected) desc += _AddAlertHeader("ALERT");
            if (needCapacity) desc += _AddAlertDetail(reason: "Limited relay capacity.", solution: "Deploy additional Relay Satellites to increase capacity.");

            _relaySatelliteCmd = new Command_Action {
                defaultLabel = "Relay Satellite",
                defaultDesc = desc,
                icon = _relaySatelliteIcon,
                action = new Action(_Void)
            };

            if (problemDetected) _relaySatelliteCmd.defaultIconColor = Color.red;
        }

        private void _UpdateEnergyRelaySatelliteCmd() {
            bool needPower = _powerNeeded > _powerGenerated;
            bool needCapacity = _powerNeeded > _energyRelayCapacity;

            bool problemDetected = needPower || needCapacity;

            string desc = _BuildHeader("Energy Relay Satellite");
            desc += _BuildOverview(_energyRelaySatelliteThingDef.description);
            desc += _AddDetail($"Total Satellites: {_energyRelayTotal}");
            desc += _AddDetail($"Relay Demand/Capacity: {_powerNeeded}/{_energyRelayCapacity} ({_GetSurplusString(surplus: _energyRelayCapacity - _powerNeeded)})");
            desc += _AddDetail($"Power Demand/Supply: {_powerNeeded}/{_powerGenerated} ({_GetSurplusString(surplus: _powerGenerated - _powerNeeded)})");
            if (problemDetected) desc += _AddAlertHeader("ALERT");
            if (needPower) desc += _AddAlertDetail(reason: "Power deficit detected.", solution: "Increase power generation by deploying Energy Booster Satellites.");
            if (needCapacity) desc += _AddAlertDetail(reason: "Limited relay capacity.", solution: "Deploy additional Energy Relay Satellites to increase capacity.");

            _energyRelaySatelliteCmd = new Command_Action {
                defaultLabel = "Energy Relay Satellite",
                defaultDesc = desc,
                icon = _energyRelaySatelliteIcon,
                action = new Action(_Void)
            };

            if (problemDetected) _energyRelaySatelliteCmd.defaultIconColor = Color.red;
        }

        private void _UpdateEnergyBoosterSatelliteCmd() {
            string desc = _BuildHeader("Energy Booster Satellite");
            desc += _BuildOverview(_energyBoosterSatelliteThingDef.description);
            desc += _AddDetail($"Total Satellites: {_energyBoosterTotal}");
            desc += _AddDetail($"Total Power Generated: {_energyBoosterPowerGenerated}");

            _energyBoosterSatelliteCmd = new Command_Action {
                defaultLabel = "Energy Booster Satellite",
                defaultDesc = desc,
                icon = _energyBoosterSatelliteIcon,
                action = new Action(_Void)
            };
        }

        private void _UpdateCosmicSurveillanceSatelliteCmd() {
            string desc = _BuildHeader("Cosmic Surveillance Satellite");
            desc += _BuildOverview(_cosmicSurveillanceSatelliteThingDef.description);
            desc += _AddDetail($"Total Satellites: {_cosmicSurveillanceTotal}");
            desc += _AddDetail($"Total Power Consumption: {_cosmicSurveillancePowerConsumption}");

            _cosmicSurveillanceSatelliteCmd = new Command_Action {
                defaultLabel = "Cosmic Surveillance Satellite",
                defaultDesc = desc,
                icon = _cosmicSurveillanceSatelliteIcon,
                action = new Action(_Void)
            };
        }

        private string _BuildHeader(string body) => "<size=15><b>" + body + "</b></size>\n";

        private string _BuildOverview(string body) => "\n" + body + "\n";

        private string _AddDetail(string body) => "\n\n" + body;

        private string _AddAlertHeader(string body) => "\n\n\n" + body.Colorize(ColorLibrary.RedReadable);

        private string _AddAlertDetail(string reason, string solution) => "\n\n" + "<b>" + reason + "</b> <i>" + solution + "</i>";

        private string _GetSurplusString(int surplus) {
            if (surplus < 0) {
                return surplus.ToString().Colorize(ColorLibrary.RedReadable);
            } else return surplus.ToString().Colorize(ColorLibrary.Green);
        }
    }
}
