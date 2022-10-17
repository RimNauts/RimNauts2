﻿using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    public class Satellites : GameComponent {
        public static int rock_moon_tile = -1;
        public static bool has_moon_map = false;

        public Satellites(Game game) : base() { }

        public RimWorld.Planet.Tile getTile(int tileNum) {
            return Find.World.grid.tiles.ElementAt(tileNum);
        }

        public int gen_new_tile(int i) {
            return (Find.World.grid.TilesCount - 1) - i;
        }

        public void applySatelliteSurface(int tileNum) {
            List<int> neighbors = new List<int>();
            Find.World.grid.GetTileNeighbors(tileNum, neighbors);
            foreach (int tile in neighbors) {
                Find.World.grid.tiles.ElementAt(tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(BiomeDefOf.RockMoonBiome.defName);
            }
            Find.World.grid.tiles.ElementAt(tileNum).elevation = 100f;
            Find.World.grid.tiles.ElementAt(tileNum).hilliness = RimWorld.Planet.Hilliness.Flat;
            Find.World.grid.tiles.ElementAt(tileNum).rainfall = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).swampiness = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).temperature = -40f;
            Find.World.grid.tiles.ElementAt(tileNum).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(BiomeDefOf.RockMoonBiome.defName);
        }

        public void updateSatellites() {
            foreach (RimWorld.Planet.MapParent obj in SatelliteContainer.satellites.Values) {
                obj.SetFaction(RimWorld.Faction.OfPlayer);
            }
        }

        public Map makeMoonMap() {
            int tile_id = -1;

            for (int i = 0; i < Find.World.grid.TilesCount; i++) {
                if (Find.World.grid.tiles.ElementAt(i).biome == BiomeDefOf.SatelliteBiome) {
                    tile_id = i;
                    break;
                }
            }

            if (tile_id == -1) return null;

            Satellite moon = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed("RockMoon", true)
            );
            moon.Tile = tile_id;
            moon.type = Satellite_Type.Moon;
            rock_moon_tile = tile_id;
            applySatelliteSurface(tile_id);
            Find.WorldObjects.Add(moon);
            SatelliteContainer.add(moon);
            Map map2 = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), moon, moon.MapGeneratorDef, moon.ExtraGenStepDefs, null);
            try {
                List<WeatherDef> wdefs = DefDatabase<WeatherDef>.AllDefs.ToList();
                foreach (WeatherDef defer in wdefs) {
                    if (defer.defName.Equals("OuterSpaceWeather")) {
                        if (Prefs.DevMode) Log.Message("RimNauts2: Found space weather.");
                        map2.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                        break;
                    }
                }
                if (Prefs.DevMode) Log.Message("RimNauts2: Didn't find space weather.");
            } catch {
                if (Prefs.DevMode) Log.Message("RimNauts2: Didn't find space weather.");
            }
            Find.World.WorldUpdate();
            return map2;
        }
    }
}
