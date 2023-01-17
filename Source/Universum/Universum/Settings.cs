using System.Collections.Generic;

namespace Universum {
    public class Settings : Verse.ModSettings {
        private static int total_configurations_found;
        public static Dictionary<string, ObjectsDef.Metadata> utilities = new Dictionary<string, ObjectsDef.Metadata>();
        public static Dictionary<string, bool> failed_attempts = new Dictionary<string, bool>();
        public static Dictionary<string, bool> saved_settings = new Dictionary<string, bool>();

        public static void init() {
            // check with defs to update list
            foreach (ObjectsDef.Metadata metadata in DefOf.Objects.Utilities) utilities.Add(metadata.id, metadata);

            foreach (KeyValuePair<string, bool> saved_utility in saved_settings) {
                if (utilities.ContainsKey(saved_utility.Key)) utilities[saved_utility.Key].toggle = saved_utility.Value;
            }
            total_configurations_found = utilities.Count;
            // print stats
            Logger.print(
                Logger.Importance.Info,
                key: "Universum.Info.settings_loader_done",
                prefix: Style.tab,
                args: new Verse.NamedArgument[] { total_configurations_found }
            );
        }

        public static bool utility_turned_on(string id) {
            if (utilities.TryGetValue(id, out ObjectsDef.Metadata utility)) {
                return utility.toggle;
            } else {
                if (failed_attempts.TryGetValue(id, out bool value)) return value;
                Logger.print(
                    Logger.Importance.Error,
                    key: "Universum.Error.failed_to_find_utility",
                    prefix: Style.name_prefix,
                    args: new Verse.NamedArgument[] { id }
                );
                failed_attempts.Add(id, false);
                return false;
            }
        }

        public override void ExposeData() {
            base.ExposeData();
            Verse.Scribe_Collections.Look(ref saved_settings, "saved_settings", Verse.LookMode.Value, Verse.LookMode.Value);
        }
    }

    public class Settings_Page : Verse.Mod {
        public static Settings settings;

        public Settings_Page(Verse.ModContentPack content) : base(content) => settings = GetSettings<Settings>();

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect) {
            UnityEngine.Rect rect1 = new UnityEngine.Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f);

            Verse.Listing_Standard listingStandard1 = new Verse.Listing_Standard();

            listingStandard1.Begin(rect1);

            if (listingStandard1.ButtonText("Default")) {
                foreach (KeyValuePair<string, ObjectsDef.Metadata> utility in Settings.utilities) Settings.utilities[utility.Key].toggle = Settings.utilities[utility.Key].default_toggle;
            }

            listingStandard1.End();
        }

        public override string SettingsCategory() => "Universum";

        public override void WriteSettings() {
            Settings.saved_settings = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, ObjectsDef.Metadata> utility in Settings.utilities) {
                Settings.saved_settings.Add(utility.Value.id, utility.Value.toggle);
            }
            base.WriteSettings();
        }
    }
}
