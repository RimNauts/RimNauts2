using System.Reflection;
using Verse;

namespace RimNauts2 {
    [Verse.StaticConstructorOnStartup]
    public static class RimNauts2 {
        static RimNauts2() {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("sindre0830.rimnauts2");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            if (ModsConfig.IsActive("torann.rimwar")) harmony.Unpatch(typeof(RimWorld.PawnGroupMakerUtility).GetMethod("GeneratePawns"), typeof(PawnGroupMakerUtility_GeneratePawns).GetMethod("Postfix"));
            // print mod info
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.mod_loaded",
                args: new NamedArgument[] { Info.name, Info.version }
            );
        }
    }
}
