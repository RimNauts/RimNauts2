using UnityEngine;
using Verse;

namespace RimNauts2.Settings {
    public class Page : Mod {
        bool toggle_buffer;

        public Page(ModContentPack content) : base(content) => GetSettings<Container>();

        public override string SettingsCategory() => RimNauts2_ModContent.instance.Content.ModMetaData.Name;

        public override void DoSettingsWindowContents(Rect inRect) {
            // default button
            Rect buttons_rectangle = new Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f);
            Listing_Standard buttons_view = new Listing_Standard();
            buttons_view.Begin(buttons_rectangle);
            if (buttons_view.ButtonText(TranslatorFormattedStringExtensions.Translate("RimNauts.default"))) Container.clear();
            buttons_view.End();

            buttons_view = new Listing_Standard();
            buttons_rectangle = new Rect(buttons_rectangle.x, buttons_rectangle.y + 30f * 4, buttons_rectangle.width, 30f * 7);
            buttons_view.Begin(buttons_rectangle);
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.mineral_rich_asteroids_messages_option", Container.get_asteroid_ore_verbose);
            Container.asteroid_ore_verbose = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.incident_patch", Container.get_incident_patch);
            Container.incident_patch = toggle_buffer;
            if (Container.get_incident_patch) {
                toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.raids_in_space", Container.get_allow_raids_on_neos);
                Container.allow_raids_on_neos = toggle_buffer;
                toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.quests_in_space", Container.get_allow_quests_on_neos);
                Container.allow_quests_on_neos = toggle_buffer;
            }
            buttons_view.End();
        }

        public bool add_checkbox(Listing_Standard view, string key, bool toggle) {
            view.CheckboxLabeled(TranslatorFormattedStringExtensions.Translate(key), ref toggle);
            return toggle;
        }
    }
}
