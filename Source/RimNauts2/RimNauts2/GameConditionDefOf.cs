using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimNauts2 {
    [RimWorld.DefOf]
    public static class GameConditionDefOf {
        public static GameConditionDef CompletelyIrradiated;

        static GameConditionDefOf() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
    }
}
