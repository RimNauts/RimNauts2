using System.Reflection;

namespace RimNauts2 {
    [Verse.StaticConstructorOnStartup]
    public static class RimNauts2 {
        static RimNauts2() {
            new HarmonyLib.Harmony("sindre0830.RimNauts2").PatchAll(Assembly.GetExecutingAssembly());
            // print mod info
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.mod_loaded",
                args: new Verse.NamedArgument[] { Info.name, Info.version }
            );
        }
    }
}
