using System.Collections.Generic;
using System.Linq;
using Verse;
using HarmonyLib;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.Planet.World), "NaturalRockTypesIn")]
    internal static class World_AddNaturalRockTypes {
        internal static void Postfix(int tile, ref IEnumerable<ThingDef> __result, ref RimWorld.Planet.World __instance) {
            Traverse traverse = Traverse.Create(__instance);
            RimWorld.Planet.WorldGrid value = traverse.Field("grid").GetValue<RimWorld.Planet.WorldGrid>();
            bool flag = value[tile].biome.defName.Equals("RockMoonBiome");
            if (flag) {
                List<ThingDef> list1 = new List<ThingDef> {
                    DefDatabase<ThingDef>.GetNamed("BiomesNEO_MariaRock"),
                    DefDatabase<ThingDef>.GetNamed("BiomesNEO_HighlandRock")
                };
                __result = list1; Find.World.NaturalRockTypesIn(1);
            } else {
                bool flag2 = __result.Contains(DefDatabase<ThingDef>.GetNamed("BiomesNEO_HighlandRock"));
                bool flag4 = __result.Contains(DefDatabase<ThingDef>.GetNamed("BiomesNEO_MariaRock"));
                if (flag2 || flag4) {
                    Rand.PushState();
                    Rand.Seed = tile;
                    List<ThingDef> rocks = __result.ToList();
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
                                            select d).ToList();

                    bool flag3 = !GenList.NullOrEmpty(list2);
                    if (flag3) {
                        rocks.Add(GenCollection.RandomElement(list2));
                    }
                    __result = rocks;
                    Rand.PopState();
                }
            }
        }
    }
}
