using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.SectionLayer_BridgeProps), "ShouldDrawPropsBelow")]
    class BridgeProps {
        public static bool Prefix(IntVec3 c, TerrainGrid terrGrid, ref bool __result) {
            if (terrGrid.TerrainAt(c).defName == "RimNauts2_Vacuum_Platform") {
                __result = false;
                return false;
            } else return true;
        }
    }
}
