using System.Reflection;

namespace Universum {
    [Verse.StaticConstructorOnStartup]
    public static class Universum {
        static Universum() {
            new HarmonyLib.Harmony("sindre0830.universum").PatchAll(Assembly.GetExecutingAssembly());

            Verse.Log.Message(Verse.TranslatorFormattedStringExtensions.Translate("Info_ModLoaded", new Verse.NamedArgument[] { Info.name, Info.version }));
        }
    }
}
