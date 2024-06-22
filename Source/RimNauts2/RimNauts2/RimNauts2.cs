using System.Reflection;
using Verse;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    public static class RimNauts2 {
        static RimNauts2() {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("sindre0830.rimnauts2");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            // print mod info
            Logger.print(
                Logger.Importance.Info,
                key: "RimNauts.Info.mod_loaded",
                args: new NamedArgument[] { Info.name, RimNauts2_ModContent.instance.Content.ModMetaData.ModVersion }
            );
            Defs.Loader.init();
        }
    }

    public class RimNauts2_ModContent : Mod {
        public static RimNauts2_ModContent instance { get; private set; }

        public RimNauts2_ModContent(ModContentPack content) : base(content) {
            instance = this;
        }
    }
}
