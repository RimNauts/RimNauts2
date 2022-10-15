using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    class SatelliteDef : Def {
        public List<string> WorldObjectDefNames = new List<string>() { "RockMoon" };

        public List<string> getWorldObjectDefNames {
            get {
                return WorldObjectDefNames;
            }
        }

        public IntVec3 orbitVectorBase = new IntVec3(200, 40, 0);

        public Vector3 getOrbitVectorBase {
            get {
                return orbitVectorBase.ToVector3();
            }
        }
        public IntVec3 orbitVectorRange = new IntVec3(150, 10, 0);

        public Vector3 getOrbitVectorRange {
            get {
                return orbitVectorRange.ToVector3();
            }
        }
        public float orbitPeriod = 36000;

        public float getOrbitPeriod {
            get {
                return orbitPeriod;
            }
        }
        public float orbitPeriodVar = 6000;

        public float getOrbitPeriodVar {
            get {
                return orbitPeriodVar;
            }
        }

        public IntVec3 maxOrbits = new IntVec3(400, 50, 200);

        public Vector3 getMaxOrbits {
            get {
                return maxOrbits.ToVector3();
            }
        }
        public IntVec3 shiftOrbits = new IntVec3(20, 20, 20);

        public Vector3 getShiftOrbits {
            get {
                return shiftOrbits.ToVector3();
            }
        }
    }
}
