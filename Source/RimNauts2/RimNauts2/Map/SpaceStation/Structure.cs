﻿using System;
using Verse;

namespace RimNauts2.SpaceStation {
    class Structure : GenStep {
        public override int SeedPart => 7856467;

        public override void Generate(Map map, GenStepParams parms) {
            ThingDef wall_thing = DefDatabase<ThingDef>.GetNamed(Defs.Of.general.space_station_wall);
            // main room
            set_floor_rect(map, 11, 11, new IntVec3(0, 0, 0));
            set_wall_rect(map, 11, 11, new IntVec3(0, 0, 0));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Door, wall_thing), map, new IntVec3(-5, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Door, wall_thing), map, new IntVec3(5, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Door, wall_thing), map, new IntVec3(0, 0, 5) + map.Center);
            // solarpanels walkway
            set_floor_rect(map, 31, 1, new IntVec3(0, 0, 0));
            set_floor_rect(map, 1, 3, new IntVec3(16, 0, 0));
            set_floor_rect(map, 1, 3, new IntVec3(10, 0, 0));
            set_floor_rect(map, 1, 3, new IntVec3(-10, 0, 0));
            set_floor_rect(map, 1, 3, new IntVec3(-16, 0, 0));
            // solarpanels
            set_floor_rect(map, 4, 4, new IntVec3(-15, 0, 4));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(-16, 0, 3) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(-15, 0, -3));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(-16, 0, -4) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(-9, 0, 4));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(-10, 0, 3) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(-9, 0, -3));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(-10, 0, -4) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(10, 0, 4));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(9, 0, 3) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(10, 0, -3));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(9, 0, -4) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(16, 0, 4));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(15, 0, 3) + map.Center);
            set_floor_rect(map, 4, 4, new IntVec3(16, 0, -3));
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.SolarGenerator), map, new IntVec3(15, 0, -4) + map.Center);
            // pad
            set_floor_rect(map, 1, 3, new IntVec3(0, 0, 7));
            set_floor_rect(map, 5, 5, new IntVec3(0, 0, 11));
            Thing podLauncher = GenSpawn.Spawn(ThingDef.Named("RimNauts2_PodLauncher"), new IntVec3(0, 0, 11) + map.Center, map);
            podLauncher.SetFaction(RimWorld.Faction.OfPlayer);
            // set power cables
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(16, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(15, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(14, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(13, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(12, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(11, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(10, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(9, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(8, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(7, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(6, 0, 0) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-16, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-15, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-14, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-13, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-12, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-11, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-10, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-9, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-8, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-7, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-6, 0, 0) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(16, 0, 1) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(16, 0, -1) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(10, 0, 1) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(10, 0, -1) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-10, 0, 1) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-10, 0, -1) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-16, 0, 1) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, new IntVec3(-16, 0, -1) + map.Center);
            // set batteries
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(-4, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(-3, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(-2, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(-1, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(0, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(1, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(2, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(3, 0, -4) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Battery), map, new IntVec3(4, 0, -4) + map.Center);
            // set temp
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Heater), map, new IntVec3(1, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Heater), map, new IntVec3(-1, 0, 0) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Heater), map, new IntVec3(0, 0, 1) + map.Center);
            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Heater), map, new IntVec3(0, 0, -1) + map.Center);

            set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Wall, wall_thing), map, map.Center);
        }

        public void set_floor_rect(Map map, int rect_width, int rect_height, IntVec3 center_offset) {
            int rect_width_middle = (int) Math.Floor(rect_width / 2.0f);
            int rect_height_middle = (int) Math.Floor(rect_height / 2.0f);

            for (int i = 0; i < rect_width; i++) {
                for (int j = 0; j < rect_height; j++) {
                    IntVec3 pos = new IntVec3(i - rect_width_middle, 0, j - rect_height_middle) + (center_offset + map.Center);
                    set_floor(map, pos);
                }
            }
        }

        public void set_wall_rect(Map map, int rect_width, int rect_height, IntVec3 center_offset) {
            ThingDef wall_thing = DefDatabase<ThingDef>.GetNamed(Defs.Of.general.space_station_wall);
            int rect_width_middle = (int) Math.Floor(rect_width / 2.0f);
            int rect_height_middle = (int) Math.Floor(rect_height / 2.0f);

            for (int i = 0; i < rect_width; i++) {
                for (int j = 0; j < rect_height; j++) {
                    IntVec3 pos = new IntVec3(i - rect_width_middle, 0, j - rect_height_middle) + (center_offset + map.Center);
                    if (i == 0 || i == rect_width - 1 || j == 0 || j == rect_height - 1) {
                        set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.Wall, wall_thing), map, pos);
                        set_thing(ThingMaker.MakeThing(RimWorld.ThingDefOf.PowerConduit), map, pos);
                    }
                    set_roof(map, pos);
                }
            }
        }

        public void set_floor(Map map, IntVec3 pos) {
            TerrainDef floor_thing = DefDatabase<TerrainDef>.GetNamed(Defs.Of.general.space_station_floor);
            map.terrainGrid.SetTerrain(pos, floor_thing);
        }

        public void set_thing(Thing thing, Map map, IntVec3 pos) {
            Thing placedThing = GenSpawn.Spawn(thing, pos, map, WipeMode.Vanish);
            placedThing.SetFaction(RimWorld.Faction.OfPlayer);
        }

        public void set_roof(Map map, IntVec3 pos) {
            map.roofGrid.SetRoof(pos, RimWorld.RoofDefOf.RoofConstructed);
        }
    }
}
