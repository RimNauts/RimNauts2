using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RimNauts2.World {
    [StaticConstructorOnStartup]
    public class ObjectHolder : RimWorld.Planet.MapParent {
        public Objects.NEO visual_object;
        public bool keep_after_abandon;
        private bool limited_time;
        private int created_tick;
        private int death_tick;
        public string label;
        public string description;
        public MapGeneratorDef map_generator;
        public string texture_overlay;
        public bool hide_now;
        public Type type;
        public Vector3 position = Vector3.zero;
        string texture_path;
        Vector3 orbit_position;
        float orbit_speed;
        Vector3 draw_size;
        int period;
        int time_offset;
        int orbit_direction;
        float color;
        float rotation_angle;
        Vector3 current_position;

        public override void ExposeData() {
            base.ExposeData();
            if (visual_object != null) {
                type = visual_object.type;
                texture_path = visual_object.texture_path;
                orbit_position = visual_object.orbit_position;
                orbit_speed = visual_object.orbit_speed;
                draw_size = visual_object.draw_size;
                period = visual_object.period;
                time_offset = visual_object.time_offset;
                orbit_direction = visual_object.orbit_direction;
                color = visual_object.color;
                rotation_angle = visual_object.rotation_angle;
                current_position = visual_object.current_position;
            }
            Scribe_Values.Look(ref keep_after_abandon, "keep_after_abandon");
            Scribe_Values.Look(ref limited_time, "limited_time");
            Scribe_Values.Look(ref created_tick, "created_tick");
            Scribe_Values.Look(ref death_tick, "death_tick");
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref description, "description");
            Scribe_Defs.Look(ref map_generator, "map_generator");
            Scribe_Values.Look(ref texture_overlay, "texture_overlay");
            Scribe_Values.Look(ref type, "type");
            Scribe_Values.Look(ref texture_path, "texture_path");
            Scribe_Values.Look(ref orbit_position, "orbit_position");
            Scribe_Values.Look(ref orbit_speed, "orbit_speed");
            Scribe_Values.Look(ref draw_size, "draw_size");
            Scribe_Values.Look(ref period, "period");
            Scribe_Values.Look(ref time_offset, "time_offset");
            Scribe_Values.Look(ref orbit_direction, "orbit_direction");
            Scribe_Values.Look(ref color, "color");
            Scribe_Values.Look(ref rotation_angle, "rotation_angle");
            Scribe_Values.Look(ref current_position, "current_position");
            if (visual_object == null) add_visual_object(
                type,
                texture_path: null,
                orbit_position,
                orbit_speed,
                draw_size,
                period,
                time_offset,
                orbit_direction,
                color,
                rotation_angle,
                current_position
            );
        }

        public override void Tick() {
            base.Tick();
            if (limited_time) {
                created_tick++;
                if (!HasMap && created_tick >= death_tick) {
                    keep_after_abandon = false;
                    Destroy();
                }
            }
        }

        public override void Draw() { }

        public override void Print(LayerSubMesh subMesh) { }

        public override Texture2D ExpandingIcon {
            get {
                if (!HasMap) return base.ExpandingIcon;
                return Assets.get_texture(texture_overlay) ?? base.ExpandingIcon;
            }
        }

        public override Vector3 DrawPos => position;

        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject) {
            alsoRemoveWorldObject = true;
            if ((from ob in Find.World.worldObjects.AllWorldObjects
                 where ob is RimWorld.Planet.TravelingTransportPods pods && ((int) typeof(RimWorld.Planet.TravelingTransportPods).GetField("initialTile", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ob) == Tile || pods.destinationTile == Tile)
                 select ob).Count() > 0) {
                return false;
            }
            return base.ShouldRemoveMapNow(out alsoRemoveWorldObject);
        }

        public override void PostRemove() {
            base.PostRemove();
            Find.World.grid.tiles.ElementAt(Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("Ocean");
            if (keep_after_abandon) {
                ObjectHolder object_holder = (ObjectHolder) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                    DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RimNauts2_ObjectHolder")
                );
                object_holder.Tile = Tile;
                object_holder.map_generator = map_generator;
                object_holder.label = label;
                object_holder.description = description;
                object_holder.keep_after_abandon = keep_after_abandon;
                object_holder.texture_overlay = texture_overlay;
                object_holder.type = type;
                object_holder.visual_object = visual_object;
                object_holder.visual_object.object_holder = object_holder;
            } else Generator.remove_visual_object(visual_object);
        }

        public void add_expiration_date(float min_days, float max_days) {
            limited_time = true;
            created_tick = RenderingManager.tick;
            death_tick = created_tick + (int) Rand.Range(min_days * 60000, max_days * 60000);
        }

        public string add_expiration_date_label() {
            if ((death_tick - created_tick) < 60000.0f) {
                return label + " (Hours left " + Math.Ceiling((death_tick - created_tick) / 2500.0f).ToString() + ")";
            } else return label + " (Days left " + ((death_tick - created_tick) / 60000.0f).ToString("0.00") + ")";
        }

        public override string Label => limited_time && !HasMap ? add_expiration_date_label() : label;

        public override string GetDescription() {
            def.description = description;
            return base.GetDescription();
        }

        public override MapGeneratorDef MapGeneratorDef => map_generator;

        public void add_visual_object(
            Type type,
            string texture_path = null,
            Vector3? orbit_position = null,
            float? orbit_speed = null,
            Vector3? draw_size = null,
            int? period = null,
            int? time_offset = null,
            int? orbit_direction = null,
            float? color = null,
            float? rotation_angle = null,
            Vector3? current_position = null
        ) {
            visual_object = Generator.add_visual_object(
                type,
                texture_path,
                orbit_position,
                orbit_speed,
                draw_size,
                period,
                time_offset,
                orbit_direction,
                color,
                rotation_angle,
                current_position
            );
            visual_object.object_holder = this;
            this.type = type;
        }
    }
}
