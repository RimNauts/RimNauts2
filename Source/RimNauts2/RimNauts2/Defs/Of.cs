using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [RimWorld.DefOf]
    public static class Of {
        public static General general;

        static Of() => RimWorld.DefOfHelper.EnsureInitializedInCtor(typeof(Of));
    }

    public class General : Def {
        public float max_altitude = 1600.0f;
        public float min_altitude = 80.0f;
        public float field_of_view = 40.0f;
        public float drag_sensitivity_multiplier = 0.50f;
        public float drag_velocity_multiplier = 0.50f;
        public float zoom_sensitivity_multiplier = 0.75f;
        public float altitude_hide_labels_multiplier = 0.50f;
        public List<string> allowed_incidents;
        public string space_station_wall;
        public string space_station_floor;
    }
}
