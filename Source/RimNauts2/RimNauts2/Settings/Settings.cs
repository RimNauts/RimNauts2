/*using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class SettingsMod : ModSettings {
        public static int TotalSatelliteObjects = 500;
        public static float MineralRichAsteroidsPercentage = 0.01f;
        public static bool CrashingAsteroidsToggle = true;
        public static bool MineralAsteroidsToggle = true;
        public static bool MineralAsteroidsVerboseToggle = false;

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref TotalSatelliteObjects, "TotalSatelliteObjects", 500);
            Scribe_Values.Look(ref MineralRichAsteroidsPercentage, "MineralRichAsteroidsPercentage", 0.01f);
            Scribe_Values.Look(ref CrashingAsteroidsToggle, "CrashingAsteroidsToggle", true);
            Scribe_Values.Look(ref MineralAsteroidsToggle, "MineralAsteroidsToggle", true);
            Scribe_Values.Look(ref MineralAsteroidsVerboseToggle, "MineralAsteroidsVerboseToggle", false);
        }
    }

    public class SettingsPage : Mod {
        public static SettingsMod settings;
        private static string bufferTotalSatelliteObjects = SettingsMod.TotalSatelliteObjects.ToString();

        public SettingsPage(ModContentPack content) : base(content) => settings = GetSettings<SettingsMod>();

        public override string SettingsCategory() => Info.name;

        public override void DoSettingsWindowContents(Rect inRect) {
            Rect rect1 = new Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f);

            Listing_Standard listingStandard1 = new Listing_Standard();

            listingStandard1.Begin(rect1);

            if (listingStandard1.ButtonText(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.default"))) {
                SettingsMod.TotalSatelliteObjects = 500;
                SettingsMod.MineralRichAsteroidsPercentage = 0.01f;
                bufferTotalSatelliteObjects = SettingsMod.TotalSatelliteObjects.ToString();
                SettingsMod.CrashingAsteroidsToggle = true;
                SettingsMod.MineralAsteroidsToggle = true;
                SettingsMod.MineralAsteroidsVerboseToggle = false;
            }
            listingStandard1.GapLine();
            listingStandard1.Label(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.total_satellites_to_spawn_option"));
            listingStandard1.IntEntry(ref SettingsMod.TotalSatelliteObjects, ref bufferTotalSatelliteObjects);
            if (SettingsMod.TotalSatelliteObjects < 0) {
                SettingsMod.TotalSatelliteObjects = 0;
                bufferTotalSatelliteObjects = SettingsMod.TotalSatelliteObjects.ToString();
            }
            listingStandard1.GapLine();
            listingStandard1.Label(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.percantage_of_mineral_rich_asteroids", new Verse.NamedArgument[] { (SettingsMod.MineralRichAsteroidsPercentage * 100).ToString("0.00") }));
            SettingsMod.MineralRichAsteroidsPercentage = listingStandard1.Slider(SettingsMod.MineralRichAsteroidsPercentage, 0.0f, 1.0f);
            listingStandard1.GapLine();
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.crashing_asteroids"), ref SettingsMod.CrashingAsteroidsToggle);
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.mineral_rich_asteroids"), ref SettingsMod.MineralAsteroidsToggle);
            listingStandard1.CheckboxLabeled(Verse.TranslatorFormattedStringExtensions.Translate("RimNauts.mineral_rich_asteroids_messages_option"), ref SettingsMod.MineralAsteroidsVerboseToggle);

            listingStandard1.End();
        }

        public override void WriteSettings() => base.WriteSettings();
    }
}*/
