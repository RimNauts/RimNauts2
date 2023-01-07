using System.Reflection;

namespace Universum {
    [Verse.StaticConstructorOnStartup]
    internal class Universum : Verse.Mod {
        public Universum(Verse.ModContentPack content) : base(content) {
            new HarmonyLib.Harmony("sindre0830.universum").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
