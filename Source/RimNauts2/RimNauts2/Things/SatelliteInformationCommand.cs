using System;
using UnityEngine;
using Verse;

namespace RimNauts2.Things {
    public abstract class SatelliteInformationCommand {
        public static Relay relay;
        public static EnergyRelay energyRelay;
        public static EnergyBooster energyBooster;
        public static CosmicSurveillance cosmicSurveillance;

        protected Universum.Defs.CelestialObject _celestialObjectDef;
        protected ThingDef _thingDef;
        protected Texture2D _icon;
        public Command cmd;

        protected bool _problemDetected;
        protected string _label;
        protected string _description;
        protected string _info;
        public int total;

        public SatelliteInformationCommand() {
            _Init();

            UpdateText();

            UpdateCmd();
        }

        protected virtual void _Init() {
            _icon = Universum.Assets.GetTexture(_celestialObjectDef.icon.texturePath);

            _description = _thingDef.description;

            cmd = new Command_Action {
                defaultLabel = _label,
                defaultDesc = _description,
                icon = _icon,
                action = new Action(_Void)
            };
        }

        public virtual void UpdateData() {
            total = Universum.Game.MainLoop.instance.GetTotal(_celestialObjectDef);
        }

        public virtual void UpdateText() {
            _info = _BuildHeader(_label);
            _info += _BuildOverview(_description);
            _info += _BuildDetails();
        }

        protected virtual string _BuildDetails() {
            string details = _AddDetail($"Total Satellites: {total}");

            return details;
        }

        public void UpdateCmd() {
            cmd.defaultDesc = _info;

            if (_problemDetected) {
                cmd.defaultIconColor = Color.red;
            } else cmd.defaultIconColor = Color.white;
        }

        protected void _Void() { }

        protected string _BuildHeader(string body) => "<size=15><b>" + body + "</b></size>\n";

        protected string _BuildOverview(string body) => "\n" + body + "\n";

        protected string _AddDetail(string body) => "\n\n" + body;

        protected string _AddAlertHeader(string body) => "\n\n\n" + body.Colorize(ColorLibrary.RedReadable);

        protected string _AddAlertDetail(string reason, string solution) => "\n\n" + "<b>" + reason + "</b> <i>" + solution + "</i>";

        protected string _GetSurplusString(int surplus) {
            if (surplus < 0) {
                return surplus.ToString().Colorize(ColorLibrary.RedReadable);
            } else return surplus.ToString().Colorize(ColorLibrary.Green);
        }
    }

    public class Relay : SatelliteInformationCommand {
        public int capacity;
        public int demand;

        public Relay() : base() {
            relay = this;
        }

        protected override void _Init() {
            _celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"];
            _thingDef = ThingDef.Named("RimNauts2_Module_Satellite");

            _label = "Relay Satellite";

            base._Init();
        }

        public override void UpdateData() {
            base.UpdateData();

            capacity = total * 4;
            demand = 0;
        }

        protected override string _BuildDetails() {
            string details = base._BuildDetails();

            bool needCapacity = demand > capacity;
            _problemDetected = needCapacity;

            details += _AddDetail($"Relay Demand/Capacity: {demand}/{capacity} ({_GetSurplusString(surplus: capacity - demand)})");

            if (_problemDetected) details += _AddAlertHeader("ALERT");
            if (needCapacity) details += _AddAlertDetail(reason: "Limited relay capacity.", solution: "Deploy additional Relay Satellites to increase capacity.");

            return details;
        }
    }

    public class EnergyRelay : SatelliteInformationCommand {
        public int capacity;
        public int demand;
        public int powerGenerated;

        public EnergyRelay() : base() {
            energyRelay = this;
        }

        protected override void _Init() {
            _celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Energy"];
            _thingDef = ThingDef.Named("RimNauts2_Module_Satellite_Energy");

            _label = "Energy Relay Satellite";

            base._Init();
        }

        public override void UpdateData() {
            base.UpdateData();

            capacity = total * 50;
            demand = 0;
            powerGenerated = total * 20;
        }

        protected override string _BuildDetails() {
            string details = base._BuildDetails();

            bool needPower = demand > powerGenerated;
            bool needCapacity = demand > capacity;
            _problemDetected = needPower || needCapacity;

            details += _AddDetail($"Relay Demand/Capacity: {demand}/{capacity} ({_GetSurplusString(surplus: capacity - demand)})");
            details += _AddDetail($"Power Demand/Supply: {demand}/{powerGenerated} ({_GetSurplusString(surplus: powerGenerated - demand)})");

            if (_problemDetected) details += _AddAlertHeader("ALERT");
            if (needPower) details += _AddAlertDetail(reason: "Power deficit detected.", solution: "Increase power generation by deploying Energy Booster Satellites.");
            if (needCapacity) details += _AddAlertDetail(reason: "Limited relay capacity.", solution: "Deploy additional Energy Relay Satellites to increase capacity.");

            return details;
        }
    }

    public class EnergyBooster : SatelliteInformationCommand {
        public int powerGenerated;

        public EnergyBooster() : base() {
            energyBooster = this;
        }

        protected override void _Init() {
            _celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_EnergyBooster"];
            _thingDef = ThingDef.Named("RimNauts2_Module_Satellite_EnergyBooster");

            _label = "Energy Booster Satellite";

            base._Init();
        }

        public override void UpdateData() {
            base.UpdateData();

            powerGenerated = total * 360;

            relay.demand += total;
            energyRelay.powerGenerated += powerGenerated;
        }

        protected override string _BuildDetails() {
            string details = base._BuildDetails();

            details += _AddDetail($"Total Power Generated: {powerGenerated}");

            return details;
        }
    }

    public class CosmicSurveillance : SatelliteInformationCommand {
        public CosmicSurveillance() : base() {
            cosmicSurveillance = this;
        }

        protected override void _Init() {
            _celestialObjectDef = Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_CosmicSurveillance"];
            _thingDef = ThingDef.Named("RimNauts2_Module_Satellite_CosmicSurveillance");

            _label = "Cosmic Surveillance Satellite";

            base._Init();
        }

        public override void UpdateData() {
            base.UpdateData();

            relay.demand += total;
            energyRelay.demand += total * 50;
        }
    }
}
