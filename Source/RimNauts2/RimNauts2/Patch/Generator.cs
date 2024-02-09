using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace RimNauts2.Patch {
    public class Generator {
        public static void Init(Harmony harmony) {
            _ = new PatchClassProcessor(harmony, typeof(Generator_SendLetter)).Patch();
        }
    }

    [HarmonyPatch]
    static class Generator_SendLetter {
        public static bool Prepare() => TargetMethod() != null;
        public static MethodBase TargetMethod() => AccessTools.Method("Universum.World.Generator:SendLetter");

        public static void Postfix(Universum.Defs.ObjectGeneration objectGenerationStep, List<string> celestialDefNames, List<Universum.World.ObjectHolder> objectHolders) {
            if (objectGenerationStep.defName == "RimNauts2_ObjectGeneration_AsteroidOre" && Settings.Container.get_asteroid_ore_verbose && Universum.Game.MainLoop.instance.GetTotal(Universum.Defs.Loader.celestialObjects["RimNauts2_CelestialObject_Satellite_Relay"]) > 0) {
                if (objectHolders.NullOrEmpty()) return;

                string asteroid_ore_names = null;
                foreach (var objectHolder in objectHolders) {
                    if (asteroid_ore_names.NullOrEmpty()) {
                        asteroid_ore_names = objectHolder.celestialObjectDef.objectHolder.description;
                    } else asteroid_ore_names += ", " + objectHolder.celestialObjectDef.objectHolder.description;
                }
                
                Find.LetterStack.ReceiveLetter(
                    "RimNauts.Label.asteroid_spawned".Translate(asteroid_ore_names),
                    "RimNauts.Description.asteroid_spawned".Translate(),
                    RimWorld.LetterDefOf.NeutralEvent,
                    (LookTargets) objectHolders[0]
                );
            }
        }
    }
}
