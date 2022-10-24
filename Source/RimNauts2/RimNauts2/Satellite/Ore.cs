using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;

namespace RimNauts2 {
    public class WorldObjectCompProperties_HarvestAsteroid : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_HarvestAsteroid() => compClass = typeof(HarvestAsteroid);
    }
    public class HarvestAsteroid : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (!parent.has_map) {
                yield return new Command_Action {
                    defaultLabel = "Harvest asteroid",
                    defaultDesc = "Harvest the mineral rich asteroid.",
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true),
                    action = () => Ore.generate_map(parent),
                };
            }
        }
    }
    public class WorldObjectCompProperties_SpawnOre : RimWorld.WorldObjectCompProperties {
        public WorldObjectCompProperties_SpawnOre() => compClass = typeof(SpawnOre);
    }

    public class SpawnOre : RimWorld.Planet.WorldObjectComp {
        public override IEnumerable<Gizmo> GetGizmos() {
            Satellite parent = this.parent as Satellite;

            if (Prefs.DevMode && parent.type == Satellite_Type.Asteroid) {
                yield return new Command_Action {
                    defaultLabel = "Convert to ore (Dev)",
                    defaultDesc = "Convert asteroid to ore.",
                    action = () => Ore.generate_ore(parent),
                };
            }
        }
    }

    public static class Ore {
        public static void generate_ore(Satellite satellite) {
            // generate new satellite with the values saved from before (this process is done to get the new texture)
            Satellite new_satellite = Generate_Satellites.copy_satellite(satellite, Satellite_Type_Methods.WorldObjects(Satellite_Type.Asteroid_Ore).RandomElement(), Satellite_Type.Asteroid_Ore);
            satellite.type = Satellite_Type.Buffer;
            satellite.Destroy();
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).biome = DefDatabase<RimWorld.BiomeDef>.GetNamed(new_satellite.def_name + "_Biome");
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).elevation = 100f;
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).hilliness = RimWorld.Planet.Hilliness.Flat;
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).rainfall = 0f;
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).swampiness = 0f;
            Find.World.grid.tiles.ElementAt(new_satellite.Tile).temperature = -60f;
        }

        public static void generate_map(Satellite satellite) {
            Map map = MapGenerator.GenerateMap(SatelliteDefOf.Satellite.MapSize(satellite.Biome.defName), satellite, satellite.MapGeneratorDef, satellite.ExtraGenStepDefs, null);
            foreach (WeatherDef weather in DefDatabase<WeatherDef>.AllDefs) {
                if (weather.defName.Equals("OuterSpaceWeather")) {
                    map.weatherManager.curWeather = WeatherDef.Named("OuterSpaceWeather");
                    if (Prefs.DevMode) Log.Message("RimNauts2: Found SOS2 space weather.");
                    break;
                }
            }
            satellite.has_map = true;
            satellite.SetFaction(RimWorld.Faction.OfPlayer);
            Find.World.WorldUpdate();
        }
    }
}
