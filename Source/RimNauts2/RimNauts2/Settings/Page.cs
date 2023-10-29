using RimNauts2.World;
using System;
using UnityEngine;
using Verse;

namespace RimNauts2.Settings {
    public class Page : Mod {
        bool toggle_buffer;
        string input_buffer;

        public Page(ModContentPack content) : base(content) => GetSettings<Container>();

        public override string SettingsCategory() => Info.name;

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
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.multi_thread_update_option", Container.get_multi_threaded_update);
            Container.multi_threaded_update = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.mineral_rich_asteroids", Container.get_asteroid_ore_toggle);
            Container.asteroid_ore_toggle = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.mineral_rich_asteroids_messages_option", Container.get_asteroid_ore_verbose);
            Container.asteroid_ore_verbose = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.world_feature_name", Container.get_world_feature_name);
            Container.world_feature_name = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.neo_trails", Container.get_neo_trails);
            Container.neo_trails = toggle_buffer;
            toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.incident_patch", Container.get_incident_patch);
            Container.incident_patch = toggle_buffer;
            if (Container.get_incident_patch) {
                toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.raids_in_space", Container.get_allow_raids_on_neos);
                Container.allow_raids_on_neos = toggle_buffer;
                toggle_buffer = add_checkbox(buttons_view, key: "RimNauts.quests_in_space", Container.get_allow_quests_on_neos);
                Container.allow_quests_on_neos = toggle_buffer;
            }
            buttons_view.End();
            // table header
            Rect table_header_rectangle = new Rect(buttons_rectangle.x, buttons_rectangle.y + (30f * 8) + 4.0f, buttons_rectangle.width, 30f);
            Listing_Standard table_header_view = new Listing_Standard();
            Widgets.DrawHighlight(table_header_rectangle);
            table_header_view.Begin(table_header_rectangle);
            table_header_view.Gap(5f);
            table_header_view.ColumnWidth = 300f;
            table_header_view.Label("Object type");
            table_header_view.NewColumn();
            table_header_view.Gap(5f);
            table_header_view.ColumnWidth = 500f;
            table_header_view.Label("Amount");
            table_header_view.End();
            // table content
            _ = Container.get_object_generation_steps;
            Rect table_content_rectangle = new Rect(table_header_rectangle.x, table_header_rectangle.y + (30f * 1), table_header_rectangle.width, 38.0f * 10);
            Listing_Standard table_content_view = new Listing_Standard();
            table_content_view.Begin(table_content_rectangle);
            int buffer = -1;
            foreach (var type_index in Enum.GetValues(typeof(World.Type))) {
                World.Type type = (World.Type) type_index;
                buffer = -1;
                buffer = Container.object_generation_steps.TryGetValue(type, buffer);
                if (buffer == -1) continue;
                Listing_Standard row_view = table_content_view.BeginSection(30f);
                row_view.Gap(5f);
                row_view.ColumnWidth = 300f;
                row_view.Label(type.ToString());
                row_view.NewColumn();
                row_view.Gap(5f);
                row_view.ColumnWidth = 500f;
                input_buffer = buffer.ToString();
                row_view.IntEntry(ref buffer, ref input_buffer);
                Container.object_generation_steps[type] = buffer;
                table_content_view.EndSection(row_view);
            }
            table_content_view.End();
        }

        public bool add_checkbox(Listing_Standard view, string key, bool toggle) {
            view.CheckboxLabeled(TranslatorFormattedStringExtensions.Translate(key), ref toggle);
            return toggle;
        }
    }
}
