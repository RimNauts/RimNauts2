using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    class Satellites : GameComponent {
        public Satellites(Game game) : base() { }

        public RimWorld.Planet.Tile getTile(int tileNum) {
            return Find.World.grid.tiles.ElementAt(tileNum);
        }

        public bool applySatelliteSurface(int tileNum) {
            try {
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
                return true;
            } catch {
                return false;
            }
        }

        public bool tryGenSatellite() {
            int tile = Find.World.grid.TilesCount - numberOfSatellites - 2;
            try {
                Satellite worldObject_SmallMoon = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(
                DefDatabase<RimWorld.WorldObjectDef>.GetNamed(def.WorldObjectDefNames.RandomElement(), true));
                worldObject_SmallMoon.Tile = tile;
                Find.WorldObjects.Add(worldObject_SmallMoon);
                this.numberOfSatellites += 1;
                this.satellites.Add(worldObject_SmallMoon);
                this.applySatelliteSurface(tile);
                worldObject_SmallMoon.real_tile = getTile(tile);
                return true;
            } catch {
                Log.Error("Failed to add satellite"); return false;
            }
        }

        public void updateSatellites() {
            foreach (RimWorld.Planet.MapParent obj in this.satellites) {
                obj.SetFaction(RimWorld.Faction.OfPlayer);
            }
        }

        public bool tryGenSatellite(int tile, List<string> satellite_types) {
            try {
                Satellite worldObject_SmallMoon = (Satellite) RimWorld.Planet.WorldObjectMaker.MakeWorldObject(DefDatabase<RimWorld.WorldObjectDef>.GetNamed(satellite_types.RandomElement<string>(), true));
                worldObject_SmallMoon.Tile = tile;
                Find.WorldObjects.Add(worldObject_SmallMoon);
                return true;
            } catch {
                Log.Error("Failed to add satellite"); return false;
            }
        }

        public Map makeMoonMap() {
            Log.Message("Look at that moon!");
            Satellite target = this.satellites.Find((Satellite x) => !x.has_map);
            this.satellites.Remove(target);
            Map map2 = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), target, target.MapGeneratorDef, target.ExtraGenStepDefs, null);
            target.has_map = true;
            this.satellites.Add(target);
            try {
                bool flag = false;
                List<WeatherDef> wdefs = DefDatabase<WeatherDef>.AllDefs.ToList();
                foreach (WeatherDef defer in wdefs) {
                    if (defer.defName.Equals("OuterSpaceWeather")) {
                        flag = true;
                    }
                }
                if (flag) {
                    Log.Message("set weather");
                    map2.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                } else {
                    Log.Message("no weather");
                }
            } catch { if (Prefs.DevMode) Log.Message("No space weather catch"); }
            Find.World.WorldUpdate();
            return map2;
        }

        public void resetSatellite() {
            this.numberOfSatellites = 0;
            this.satellites = new List<Satellite>();
        }

        public void removeSatellite(Satellite satellite) {
            this.satellites.Remove(satellite);
            this.numberOfSatellites -= 1;
        }

        public int numberOfSatellites = 0;
        readonly SatelliteDef def = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public List<Satellite> satellites = new List<Satellite>();
    }
}
