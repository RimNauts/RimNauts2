using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld.Planet;
using RimWorld;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(World), "NaturalRockTypesIn")]
    internal static class World_AddNaturalRockTypes {

        internal static void Postfix(int tile, ref IEnumerable<ThingDef> __result, ref World __instance) {
            Traverse traverse = Traverse.Create(__instance);
            WorldGrid value = traverse.Field("grid").GetValue<WorldGrid>();
            bool flag = value[tile].biome.defName.Equals("RockMoonBiome");
            if (flag) {
                List<ThingDef> list1 = new List<ThingDef>();

                list1.Add(DefDatabase<ThingDef>.GetNamed("BiomesNEO_MariaRock"));
                list1.Add(DefDatabase<ThingDef>.GetNamed("BiomesNEO_HighlandRock"));
                __result = list1; Find.World.NaturalRockTypesIn(1);
            } else {
                bool flag2 = __result.Contains(DefDatabase<ThingDef>.GetNamed("BiomesNEO_HighlandRock"));
                bool flag4 = __result.Contains(DefDatabase<ThingDef>.GetNamed("BiomesNEO_MariaRock"));
                if (flag2 || flag4) {
                    Rand.PushState();
                    Rand.Seed = tile;
                    List<ThingDef> rocks = __result.ToList<ThingDef>();
                    if (flag2) rocks.Remove(DefDatabase<ThingDef>.GetNamed("BiomesNEO_HighlandRock"));
                    if (flag4) rocks.Remove(DefDatabase<ThingDef>.GetNamed("BiomesNEO_MariaRock"));
                    List<ThingDef> list2 = (from d in DefDatabase<ThingDef>.AllDefs
                                            where d.category.Equals(ThingCategory.Building) &&
                                            d.building.isNaturalRock &&
                                            !d.building.isResourceRock &&
                                            !d.IsSmoothed &&
                                            !rocks.Contains(d) &&
                                            d.defName != "BiomesNEO_HighlandRock" &&
                                            d.defName != "BiomesNEO_MariaRock"
                                            select d).ToList<ThingDef>();

                    bool flag3 = !GenList.NullOrEmpty<ThingDef>(list2);
                    if (flag3) {
                        rocks.Add(GenCollection.RandomElement<ThingDef>(list2));
                    }
                    __result = rocks;
                    Rand.PopState();
                }
            }
        }
    }
}
