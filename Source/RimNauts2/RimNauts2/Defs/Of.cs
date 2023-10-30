using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [RimWorld.DefOf]
    public static class Of {
        public static General general;

        static Of() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(Of));
    }

    public class General : Def {
        public List<string> allowed_incidents;
        public string space_station_wall;
        public string space_station_floor;
    }
}
