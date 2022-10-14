using System;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using System.Linq;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public class WorldObjectChild_Satellite : MapParent {
        public override Vector3 DrawPos {
            get {
                return calcParametricEllipse(this.maxOrbits, this.shiftOrbits, this.period, this.timeOffset);
            }
        }

        public override void PostAdd() {
            this.maxOrbits = randomizeVect(this.satDef.getMaxOrbits, true);
            this.shiftOrbits = randomizeVect(this.satDef.getShiftOrbits, true);
            this.period = (int) randomOrbit(satDef.getOrbitPeriod, satDef.getOrbitPeriodVar); //+ (float)((Rand.Value - 0.5f) * (this.satDef.getOrbitPeriod * 0.25)); 
            this.timeOffset = Rand.Range(0, (int) (this.period / 2));


            base.PostAdd();
        }

        public float randomOrbit(float min, float range) {
            float norm = min * (Rand.Bool ? 1 : -1);
            float result = min + (range * (Rand.Value - 0.5f));
            return result;
        }
        public Vector3 randomizeVect(Vector3 oldVector, bool dirRand) {
            Vector3 vec = new Vector3();
            vec.x = (Rand.Bool && dirRand ? 1 : -1) * oldVector.x + (float) ((Rand.Value - 0.5f) * (oldVector.x * 0.25));
            vec.y = (Rand.Bool && dirRand ? 1 : -1) * oldVector.y + (float) ((Rand.Value - 0.5f) * (oldVector.y * 0.25));
            vec.z = (Rand.Bool && dirRand ? 1 : -1) * oldVector.z + (float) ((Rand.Value - 0.5f) * (oldVector.z * 0.25));

            return vec;

        }
        public Vector3 calcParametricEllipse(Vector3 max, Vector3 shift, float Period, int timeOffset) {
            Vector3 vec3 = new Vector3();
            int time = Find.TickManager.TicksGame;
            vec3.x = max.x * (float) Math.Cos((6.28f) / Period * (time + timeOffset)) + shift.x;
            vec3.z = max.z * (float) Math.Sin((6.28f) / Period * (time + timeOffset)) + shift.z;
            vec3.y = max.y * (float) Math.Cos((6.28f) / Period * (time + timeOffset)) + shift.y;
            return vec3;
        }


        internal static object GetInstanceField(Type type, object instance, string fieldName) {
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetField(fieldName, bindingAttr).GetValue(instance);
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.period, "period", 0, false);
            Scribe_Values.Look<int>(ref this.timeOffset, "timeOffset", 0, false);
            //Scribe_Values.Look<float>(ref this.traveledPct, "traveledPct", 0f, false);
            Scribe_Values.Look<Vector3>(ref this.maxOrbits, "maxOrbits", default(Vector3), false);
            Scribe_Values.Look<Vector3>(ref this.shiftOrbits, "shiftOrbits", default(Vector3), false);
            WorldObjectChild_Satellite.GetInstanceField(typeof(WorldObject), this, "BaseDrawSize");
        }

        public override void Tick() {
            base.Tick();
        }

        public override void Print(LayerSubMesh subMesh) {
            float averageTileSize = Find.WorldGrid.averageTileSize;
            WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 10.7f * averageTileSize, 0.008f, subMesh, false, false, true);
        }

        public override void Draw() {
            float averageTileSize = Find.WorldGrid.averageTileSize;
            float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
            if (this.def.expandingIcon && transitionPct > 0f) {
                //Color color = this.Material.color;
                MaterialPropertyBlock materialPropertyBlock = WorldObjectChild_Satellite.propertyBlock;
                WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 10f * averageTileSize, 0.008f, this.Material, false, false, null);
                return;
            }
            WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 10f * averageTileSize, 0.008f, this.Material, false, false, null);
        }
        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject) {
            alsoRemoveWorldObject = true;
            if ((from ob in Find.World.worldObjects.AllWorldObjects
                 where ob is TravelingTransportPods && ((int) typeof(TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ob) == base.Tile || ((TravelingTransportPods) ob).destinationTile == base.Tile)
                 select ob).Count<WorldObject>() > 0) {
                return false;
            }
            return base.ShouldRemoveMapNow(out alsoRemoveWorldObject);
        }

        public override void PostRemove() {
            Current.Game.GetComponent<Satellite>().removeSatellite(this);
            base.PostRemove();
        }

        public float calculateDistanceAcceleration(float distance, float angleSpeed, float GravConst, float mass) {
            return distance * angleSpeed * angleSpeed - (GravConst * mass) / distance * distance;
        }

        public float calculateAngleAcceleration(float distanceSpeed, float angleSpeed, float distance) {
            return -2.0f * distanceSpeed * angleSpeed / distance;
        }

        public float newValue(float currentValue, float deltaT, float derivative) {
            return currentValue + deltaT * derivative;
        }
        public void convertCart(float radius, float theta, Vector3 cart) {
            cart.x = radius * (float) Math.Cos((double) theta);
            cart.y = radius * (float) Math.Sin((double) theta);
        }

        public void orbital() {
            float distance = 1.496e11f;
            float angleSpeed = 1.9909867e-7f;
            float GravConst = 6.674e-11f;
            float mass = 1000;
            float distanceSpeed = 0;
            float deltaT = 0.015f;
            double angle = Math.PI / 6.00;
            float distanceAcceleration = calculateDistanceAcceleration(distance, angleSpeed, GravConst, mass);
            distanceSpeed = newValue(distanceSpeed,
              deltaT, distanceAcceleration);
            distance = newValue(distance,
              deltaT, distanceSpeed);
            float angleAcceleration = calculateAngleAcceleration(distanceSpeed, angleSpeed, distance);
            angleSpeed = newValue(angleSpeed,
              deltaT, angleAcceleration);
            angle = newValue((float) angle,
              deltaT, angleSpeed);
        }

        SatelliteDef satDef = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public Vector3 maxOrbits;
        public Vector3 shiftOrbits;
        public bool hasMap = false;
        public Map map;
        public float period;
        public Tile realTile;
        private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        public int timeOffset = 0;
    }
}
