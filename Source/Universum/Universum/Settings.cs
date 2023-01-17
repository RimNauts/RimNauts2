using System.Collections.Generic;

namespace Universum {
    public class Settings : Verse.ModSettings {
        private static int total_configurations_found;
        public static Dictionary<string, ObjectsDef.Metadata> utilities = new Dictionary<string, ObjectsDef.Metadata>();
        public static Dictionary<string, bool> failed_attempts = new Dictionary<string, bool>();

        public static void init() {
            if (utilities.Count <= 0) utilities = load_utilities();
            total_configurations_found = utilities.Count;
            // print stats
            Logger.print(
                Logger.Importance.Info,
                key: "Universum.Info.settings_loader_done",
                prefix: Style.tab,
                args: new Verse.NamedArgument[] { total_configurations_found }
            );
        }

        public static Dictionary<string, ObjectsDef.Metadata> load_utilities() {
            Dictionary<string, ObjectsDef.Metadata> tmp = new Dictionary<string, ObjectsDef.Metadata>();
            foreach (ObjectsDef.Metadata metadata in DefOf.Objects.Utilities) tmp.Add(metadata.id, metadata);
            return tmp;
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
            Verse.Scribe_Values.Look(ref utilities, "utilities", new Dictionary<string, ObjectsDef.Metadata>());
            // check with defs to update list
            Dictionary<string, ObjectsDef.Metadata> tmp = load_utilities();
            foreach (KeyValuePair<string, ObjectsDef.Metadata> utility in utilities) {
                if (tmp.ContainsKey(utility.Key)) {
                    if (tmp[utility.Key].default_toggle == utility.Value.default_toggle && !tmp[utility.Key].hide_in_settings) tmp[utility.Key].toggle = utility.Value.toggle;
                }
            }
            utilities = tmp;
        }
    }
}
