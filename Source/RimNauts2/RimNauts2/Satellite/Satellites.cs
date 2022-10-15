using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    public class Satellites : GameComponent {
        public bool moon_exists = false;
        readonly SatelliteDef def = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public static Dictionary<int, Satellite> cachedWorldObjectTiles = new Dictionary<int, Satellite>();

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
                Find.World.grid.tiles.ElementAt(tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("RockMoonBiome");
            }
            Find.World.grid.tiles.ElementAt(tileNum).elevation = 100f;
            Find.World.grid.tiles.ElementAt(tileNum).hilliness = RimWorld.Planet.Hilliness.Flat;
            Find.World.grid.tiles.ElementAt(tileNum).rainfall = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).swampiness = 0f;
            Find.World.grid.tiles.ElementAt(tileNum).temperature = -40f;
            Find.World.grid.tiles.ElementAt(tileNum).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed("RockMoonBiome");
        }

        public Satellite tryGenSatellite() {
            try {
                int i = Generate_Satellites.total_satellite_amount + 1;
                int tile = gen_new_tile(i);
                if (cachedWorldObjectTiles.ContainsKey(tile) && cachedWorldObjectTiles[tile] is Satellite) {
                    moon_exists = true;
                    return null;
                }
                Satellite worldObject_SmallMoon = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def.WorldObjectDefNames.RandomElement(), true));
                worldObject_SmallMoon.Tile = gen_new_tile(tile);
                Find.WorldObjects.Add(worldObject_SmallMoon);
                applySatelliteSurface(worldObject_SmallMoon.Tile);
                worldObject_SmallMoon.real_tile = getTile(worldObject_SmallMoon.Tile);
                worldObject_SmallMoon.has_map = true;
                SatelliteTiles_Utilities.add_satellite(worldObject_SmallMoon);
                return worldObject_SmallMoon;
            } catch {
                Log.Error("Failed to add satellite");
                return null;
            }
        }

        public void updateSatellites() {
            foreach (RimWorld.Planet.MapParent obj in cachedWorldObjectTiles.Values) {
                obj.SetFaction(RimWorld.Faction.OfPlayer);
            }
        }

        public void tryGenSatellite(int i, List<string> satellite_types) {
            int tile = gen_new_tile(i);
            if (cachedWorldObjectTiles.ContainsKey(tile) && cachedWorldObjectTiles[tile] is Satellite) return;
            Satellite satellite = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(satellite_types.RandomElement(), true)
            );
            satellite.Tile = tile;
            Find.WorldObjects.Add(satellite);
            SatelliteTiles_Utilities.add_satellite(satellite);
        }

        public Map makeMoonMap() {
            Satellite target = tryGenSatellite();
            if (moon_exists) {
                return null;
            } else {
                moon_exists = true;
            }
            Map map2 = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), target, target.MapGeneratorDef, target.ExtraGenStepDefs, null);
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
