using HarmonyLib;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.SectionLayer_BridgeProps), "ShouldDrawPropsBelow")]
    class SectionLayer_BridgeProps_ShouldDrawPropsBelow {
        public static bool Prefix(IntVec3 c, TerrainGrid terrGrid, ref bool __result) {
            if (terrGrid.TerrainAt(c).defName == "RimNauts2_Vacuum_Platform") {
                __result = false;
                return false;
            } else if (terrGrid.TerrainAt(c).defName == "RimNauts2_SteelBridge") {
                // should patch RimWorld.SectionLayer_BridgeProps.Regenerate to draw custom prop
                __result = false;
                return false;
            } else return true;
        }
    }
}
