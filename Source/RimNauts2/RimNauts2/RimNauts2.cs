using System.Reflection;
using Verse;
using HarmonyLib;

namespace RimNauts2 {
    [StaticConstructorOnStartup]
    internal class RimNauts2 : Mod {
        public RimNauts2(ModContentPack content) : base(content) {
            new Harmony("sindre0830.RimNauts2").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
