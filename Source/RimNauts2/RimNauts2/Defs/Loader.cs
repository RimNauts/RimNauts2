using System.Collections.Generic;
using Verse;

namespace RimNauts2.Defs {
    [StaticConstructorOnStartup]
    public static class Loader {
        private static int total_defs;
        public static Dictionary<World.Type, ObjectGenerationStep> object_generation_steps = new Dictionary<World.Type, ObjectGenerationStep>();
        public static RoofDef roof_magnetic_field = DefDatabase<RoofDef>.GetNamed("RimNauts2_Roof_MagneticField");
        public static ThingDef thing_roof_magnetic_field = DefDatabase<ThingDef>.GetNamed("RimNauts2_Things_Roof_MagneticField");
        public static ThingDef thing_unroof_magnetic_field = DefDatabase<ThingDef>.GetNamed("RimNauts2_Things_UnRoof_MagneticField");
        public static RimWorld.BiomeDef biome_satellite = DefDatabase<RimWorld.BiomeDef>.GetNamed("RimNauts2_Satellite_Biome");
        public static ThingDef thing_cargo_pod = DefDatabase<ThingDef>.GetNamed("RimNauts2_TransportPod_Cargo");
        public static EffecterDef effecter_delivery_cannon_shot = DefDatabase<EffecterDef>.GetNamed("RimNauts2_DeliveryCannon_Shot");
        public static RimWorld.WorldObjectDef world_object_travelling_delivery_cannon_shell = DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RimNauts2_TravellingDeliveryCannonShell");
        public static ThingDef thing_delivery_cannon_incoming = DefDatabase<ThingDef>.GetNamed("RimNauts2_DropPodIncoming_Shell");
        public static ThingDef thing_delivery_cannon_active = DefDatabase<ThingDef>.GetNamed("RimNauts2_ActiveDropPod_Shell");
        public static RimWorld.BiomeDef biome_barren_moon = DefDatabase<RimWorld.BiomeDef>.GetNamed("RimNauts2_MoonBarren_Biome");

        public static void init() {
            foreach (ObjectGenerationStep object_generation_step in DefDatabase<ObjectGenerationStep>.AllDefs) {
                World.Type type = (World.Type) object_generation_step.type;
                if (object_generation_steps.ContainsKey(type)) continue;
                object_generation_steps.Add(type, object_generation_step);
                total_defs++;
            }
        }
    }
}
