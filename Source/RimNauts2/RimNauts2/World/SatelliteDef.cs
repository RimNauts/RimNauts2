using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    class SatelliteDef : Def {
        public static int total_satellite_amount = 200;
        public static float asteroid_percent = 0.90f;
        public static float ore_percent = 0.05f;
        public static float junk_percent = 0.05f;

        public static List<string> asteroid_defs = new List<string>() {
            "asteroid_1",
            "asteroid_2",
            "asteroid_3",
        };

        public static List<string> asteroid_ore_defs = new List<string>() {
            "ore_steel",
            "ore_gold",
            "ore_plasteel",
        };

        public static List<string> junk_defs = new List<string>() {
            "junk_1",
            "junk_2",
            "junk_3",
            "junk_4",
        };

        public List<string> WorldObjectDefNames = new List<string>() { "RockMoon" };
        public List<string> getWorldObjectDefNames {
            get {
                return (List<string>) this.WorldObjectDefNames;
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
