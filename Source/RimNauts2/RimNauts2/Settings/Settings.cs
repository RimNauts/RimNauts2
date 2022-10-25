using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class Settings : ModSettings {
        public static int TotalSatelliteObjects = 1000;
        public static bool CrashingAsteroidsToggle = true;
        public static bool MineralAsteroidsToggle = true;

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref TotalSatelliteObjects, "TotalSatelliteObjects", 1000);
            Scribe_Values.Look(ref CrashingAsteroidsToggle, "CrashingAsteroidsToggle", true);
            Scribe_Values.Look(ref MineralAsteroidsToggle, "MineralAsteroidsToggle", true);
        }
    }

    public class SettingsPage : Mod {
        public static Settings settings;
        private static string bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();

        public SettingsPage(ModContentPack content) : base(content) => settings = GetSettings<Settings>();

        public override string SettingsCategory() => "RimNauts2";

        public override void DoSettingsWindowContents(Rect inRect) {
            Rect rect1 = new Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f);

            Listing_Standard listingStandard1 = new Listing_Standard();

            listingStandard1.Begin(rect1);

            if (listingStandard1.ButtonText("Default")) {
                Settings.TotalSatelliteObjects = 1000;
                bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();
                Settings.CrashingAsteroidsToggle = true;
                Settings.MineralAsteroidsToggle = true;
            }
            listingStandard1.Gap(10f);

            listingStandard1.Label("Total satellite objects. Changes require a new save");
            listingStandard1.IntEntry(ref Settings.TotalSatelliteObjects, ref bufferTotalSatelliteObjects);
            if (Settings.TotalSatelliteObjects < 0) {
                Settings.TotalSatelliteObjects = 0;
                bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();
            }
            listingStandard1.Gap(10f);

            listingStandard1.CheckboxLabeled("Crashing asteroids", ref Settings.CrashingAsteroidsToggle);
            listingStandard1.Gap(10f);

            listingStandard1.CheckboxLabeled("Mineral-rich asteroids", ref Settings.MineralAsteroidsToggle);

            listingStandard1.End();
        }

        public override void WriteSettings() => base.WriteSettings();
    }
}
