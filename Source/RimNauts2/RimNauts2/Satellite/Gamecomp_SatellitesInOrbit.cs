using System;
using System.Collections.Generic;
using RimWorld;
using System.Linq;
using RimWorld.Planet;
using Verse;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Security.Policy;

namespace RimNauts2 {
    class Gamecomp_SatellitesInOrbit : GameComponent {
        public Gamecomp_SatellitesInOrbit(Game game) : base() {
        }
        public Tile getTile(int tileNum) {
            return Find.World.grid.tiles.ElementAt<Tile>(tileNum);
        }
        public bool applySatelliteSurface(int tileNum) {
            try {
                List<int> neighbors = new List<int>();
                Find.World.grid.GetTileNeighbors(tileNum, neighbors);
                foreach (int tile in neighbors) {
                    Find.World.grid.tiles.ElementAt(tile).biome = DefDatabase<BiomeDef>.GetNamed("RockMoonBiome");
                }
                Find.World.grid.tiles.ElementAt(tileNum).elevation = 100f;
                Find.World.grid.tiles.ElementAt(tileNum).hilliness = Hilliness.Flat;
                Find.World.grid.tiles.ElementAt(tileNum).rainfall = 0f;
                Find.World.grid.tiles.ElementAt(tileNum).swampiness = 0f;
                Find.World.grid.tiles.ElementAt(tileNum).temperature = -40f;
                Find.World.grid.tiles.ElementAt(tileNum).biome = DefDatabase<BiomeDef>.GetNamed("RockMoonBiome");
                return true;
            } catch {
                return false;
            }
        }


        public bool tryGenSatellite() {
            int tile = Find.World.grid.TilesCount - this.numberOfSatellites - 2;
            Vector3 baseVec = satDef.getOrbitVectorBase;
            Vector3 varVec = satDef.getOrbitVectorRange;
            try {
                WorldObjectChild_Satellite worldObject_SmallMoon = (WorldObjectChild_Satellite) WorldObjectMaker.MakeWorldObject(
                DefDatabase<WorldObjectDef>.GetNamed(satDef.WorldObjectDefNames.RandomElement<string>(), true));
                worldObject_SmallMoon.Tile = tile;
                Find.WorldObjects.Add(worldObject_SmallMoon);
                this.numberOfSatellites += 1;
                this.satellites.Add(worldObject_SmallMoon);
                this.applySatelliteSurface(tile);
                worldObject_SmallMoon.realTile = getTile(tile);
                return true;
            } catch {
                Log.Error("Failed to add satellite"); return false;
            }
        }
        public void updateSatellites() {
            foreach (MapParent obj in this.satellites) {
                obj.SetFaction(Faction.OfPlayer);
            }
        }
        public bool tryGenSatellite(int tile) {
            try {
                //this.defs.AddRange(this.satDef.getWorldObjectDefNames);
                WorldObjectChild_Satellite worldObject_SmallMoon = (WorldObjectChild_Satellite) WorldObjectMaker.MakeWorldObject(DefDatabase<WorldObjectDef>.GetNamed(defs.RandomElement<string>(), true));
                worldObject_SmallMoon.Tile = tile;
                Find.WorldObjects.Add(worldObject_SmallMoon);
                return true;
            } catch {
                Log.Error("Failed to add satellite"); return false;
            }
        }

        public Map makeMoonMap() {
            Log.Message("Look at that moon!");
            WorldObjectChild_Satellite target = this.satellites.Find((WorldObjectChild_Satellite x) => !x.hasMap);
            this.satellites.Remove(target);
            Map map2 = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), target, target.MapGeneratorDef, target.ExtraGenStepDefs, null);
            target.hasMap = true;
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
            this.satellites = new List<WorldObjectChild_Satellite>();
        }
        public void removeSatellite(WorldObjectChild_Satellite satellite) {
            this.satellites.Remove(satellite);
            this.numberOfSatellites -= 1;
        }
        public bool tryGenSatelliteMap(int tile) {
            return false;
        }
        public int numberOfSatellites = 0;
        SatelliteDef satDef = DefDatabase<SatelliteDef>.GetNamed("SatelliteCore");
        public List<WorldObjectChild_Satellite> satellites = new List<WorldObjectChild_Satellite>();
        List<string> defs = new List<string>() { "Junk1", "Junk2", "Junk3", "Junk4", "Junk5", "Junk6", "Junk7", "Junk8", "Junk9" };
    }
}
