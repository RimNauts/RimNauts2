using HarmonyLib;
using Verse;

namespace RimNauts2.Things.Patch {
    [HarmonyPatch(typeof(RimWorld.SectionLayer_BridgeProps), "ShouldDrawPropsBelow")]
    class SectionLayer_BridgeProps_ShouldDrawPropsBelow {
        public static bool Prefix(IntVec3 c, TerrainGrid terrGrid, ref bool __result) {
            if (terrGrid.TerrainAt(c).defName == "RimNauts2_Vacuum_Platform" || terrGrid.TerrainAt(c).defName == "RimNauts2_SteelBridge") {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
