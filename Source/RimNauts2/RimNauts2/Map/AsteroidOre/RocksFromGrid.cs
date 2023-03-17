using Verse;
using UnityEngine;

namespace RimNauts2.AsteroidOre {
    class RocksFromGrid : GenStep {
        public string rock_def_name = "Limestone";
        public string ore_def_name = "MineableSteel";
        public Vector2 ore_chance_outer = new Vector3(0.30f, 0.50f);
        public Vector2 ore_chance_inner = new Vector3(0.50f, 0.70f);
        public float inner_elevation = 0.75f;
        public float roof_elevation = 0.55f;

        public override int SeedPart => 262606459;

        public override void Generate(Map map, GenStepParams parms) {
            float outer_chance = Rand.Range(ore_chance_outer.x, ore_chance_outer.y);
            float inner_chance = Rand.Range(ore_chance_inner.x, ore_chance_inner.y);
            foreach (IntVec3 current in map.AllCells) {
                if (MapGenerator.Fertility[current] <= 0.1f) {
                    if (MapGenerator.Elevation[current] > inner_elevation && MapGenerator.Caves[current] <= 0.0f) {
                        GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(Rand.Value > inner_chance ? rock_def_name : ore_def_name, true), current, map);
                    } else {
                        GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(Rand.Value > outer_chance ? rock_def_name : ore_def_name, true), current, map);
                    }
                    map.terrainGrid.SetTerrain(current, ThingDef.Named(rock_def_name).building.naturalTerrain);
                    if (MapGenerator.Elevation[current] > roof_elevation) map.roofGrid.SetRoof(current, RimWorld.RoofDefOf.RoofRockThin);
                }
            }
        }
    }
}
