using System.Reflection;

namespace RimNauts2 {
    [Verse.StaticConstructorOnStartup]
    public static class RimNauts2 {
        static RimNauts2() {
            new HarmonyLib.Harmony("sindre0830.RimNauts2").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
