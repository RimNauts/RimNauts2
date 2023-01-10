using System.Reflection;

namespace Universum {
    [Verse.StaticConstructorOnStartup]
    public static class Universum {
        static Universum() {
            new HarmonyLib.Harmony("sindre0830.universum").PatchAll(Assembly.GetExecutingAssembly());
            // print mod info
            Logger.print(
                Importance.Info,
                key: "Info_ModLoaded",
                prefix: false,
                args: new Verse.NamedArgument[] { Info.name, Info.version }
            );
            // load configuarations
            Utilities.Biome.Handler.init();
        }
    }
}
