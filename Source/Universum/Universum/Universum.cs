using System.Reflection;

namespace Universum {
    [Verse.StaticConstructorOnStartup]
    public static class Universum {
        static Universum() {
            new HarmonyLib.Harmony("sindre0830.universum").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
