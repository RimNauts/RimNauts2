using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    class SatelliteDef : Def {
        public IntVec3 orbitVectorBase = new IntVec3(200, 40, 0);
        public IntVec3 orbitVectorRange = new IntVec3(150, 10, 0);
        public float orbitPeriod = 36000f;
        public float orbitPeriodVar = 6000f;
        public IntVec3 maxOrbits = new IntVec3(400, 50, 200);
        public IntVec3 shiftOrbits = new IntVec3(20, 20, 20);
        public Vector3 spread = new Vector3(0.25f, 0.25f, 0.25f);

        public Vector3 getOrbitVectorBase => orbitVectorBase.ToVector3();

        public Vector3 getOrbitVectorRange => orbitVectorRange.ToVector3();

        public float getOrbitPeriod => orbitPeriod;

        public float getOrbitPeriodVar => orbitPeriodVar;

        public Vector3 getMaxOrbits => maxOrbits.ToVector3();

        public Vector3 getShiftOrbits => shiftOrbits.ToVector3();

        public Vector3 getSpread => spread;
    }
}
