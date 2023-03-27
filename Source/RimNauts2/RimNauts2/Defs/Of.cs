using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [RimWorld.DefOf]
    public static class Of {
        public static General general;

        static Of() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(Of));
    }

    public class General : Def {
        public float max_altitude = 1100.0f;
        public List<string> allowed_incidents;
        public string space_station_wall;
        public string space_station_floor;
    }
}
