using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimNauts2 {
    public class WorldObjectCompProperties_Settle : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_Settle() => compClass = typeof(GenerateMapButton);
    }

    public class GenerateMapButton : RimWorld.Planet.WorldObjectComp {
        public static void generate_moon_map(Satellite satellite) {
            satellite.has_map = true;
            applySatelliteSurface(satellite.Tile);
            Map map = MapGenerator.GenerateMap(new IntVec3(300, 1, 300), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            try {
                List<WeatherDef> wdefs = DefDatabase<WeatherDef>.AllDefs.ToList();
                foreach (WeatherDef defer in wdefs) {
                    if (defer.defName.Equals("OuterSpaceWeather")) {
                        if (Prefs.DevMode) Log.Message("RimNauts2: Found space weather.");
                        map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                        break;
                    }
                }
            } catch { }
            Find.World.WorldUpdate();
            satellite.SetFaction(RimWorld.Faction.OfPlayer);
            CameraJumper.TryJump(map.Center, map);
            Find.MapUI.Notify_SwitchedMap();
        }

        private static void applySatelliteSurface(int tileNum) {
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

        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;
            if (!parent.has_map) {
                yield return new Command_Action {
                    defaultLabel = "CommandSettle".Translate(),
                    defaultDesc = "CommandSettleDesc".Translate(),
                    icon = RimWorld.Planet.SettleUtility.SettleCommandTex,
                    action = () => generate_moon_map(parent),
                };
            }
        }
    }
}
