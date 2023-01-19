using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class Settings : ModSettings {
        public static int TotalSatelliteObjects = 500;
        public static bool CrashingAsteroidsToggle = true;
        public static bool MineralAsteroidsToggle = true;
        public static bool MineralAsteroidsVerboseToggle = false;

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref TotalSatelliteObjects, "TotalSatelliteObjects", 500);
            Scribe_Values.Look(ref CrashingAsteroidsToggle, "CrashingAsteroidsToggle", true);
            Scribe_Values.Look(ref MineralAsteroidsToggle, "MineralAsteroidsToggle", true);
            Scribe_Values.Look(ref MineralAsteroidsVerboseToggle, "MineralAsteroidsVerboseToggle", false);
        }
    }

    public class SettingsPage : Mod {
        public static Settings settings;
        private static string bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();

        public SettingsPage(ModContentPack content) : base(content) => settings = GetSettings<Settings>();

        public override string SettingsCategory() => Info.name;

        public override void DoSettingsWindowContents(Rect inRect) {
            Rect rect1 = new Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f);

            Listing_Standard listingStandard1 = new Listing_Standard();

            listingStandard1.Begin(rect1);

            if (listingStandard1.ButtonText(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.default"))) {
                Settings.TotalSatelliteObjects = 500;
                bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();
                Settings.CrashingAsteroidsToggle = true;
                Settings.MineralAsteroidsToggle = true;
                Settings.MineralAsteroidsVerboseToggle = false;
            }
            listingStandard1.Label(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.total_satellites_to_spawn_option"));
            listingStandard1.IntEntry(ref Settings.TotalSatelliteObjects, ref bufferTotalSatelliteObjects);
            if (Settings.TotalSatelliteObjects < 0) {
                Settings.TotalSatelliteObjects = 0;
                bufferTotalSatelliteObjects = Settings.TotalSatelliteObjects.ToString();
            }
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.crashing_asteroids"), ref Settings.CrashingAsteroidsToggle);
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.mineral_rich_asteroids"), ref Settings.MineralAsteroidsToggle);
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.mineral_rich_asteroids_messages_option"), ref Settings.MineralAsteroidsVerboseToggle);

            listingStandard1.End();
        }

        public override void WriteSettings() => base.WriteSettings();
    }
}
