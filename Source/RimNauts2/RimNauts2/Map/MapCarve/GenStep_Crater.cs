using System;
using Verse;
using System.Linq;

namespace RimNauts2 {
    public class GenStep_Crater : GenStep {
        public override int SeedPart {
            get {
                return 1148858716;
            }
        }

        public void wideBrush(IntVec3 pos, string defName, Map map, int halfWidth) {
            for (int dz = -halfWidth; dz <= halfWidth; dz++) {
                map.terrainGrid.SetTerrain(new IntVec3(pos.x, 0, pos.z + dz), DefDatabase<TerrainDef>.GetNamed(defName));
            }
        }

        public override void Generate(Map map, GenStepParams parms) {
            int radius = (map.Size.x / 2) - 3;
            for (int ind = 1; ind <= Rand.RangeInclusive(4, 10); ind++) {
                int dz = (int) ((Rand.Value - 0.5) * map.Size.z);
                for (int dx = 1; dx <= map.Size.x - 1; dx++) {
                    int set = (int) (-22f * Math.Sin((3.14f / map.Size.x) * dx - 0) + map.Center.z + dz * 0.75);
                    wideBrush(new IntVec3(dx, 0, set), "lunarMaria", map, Rand.RangeInclusive(1, 4));
                }
            }

            for (int ind = 1; ind <= Rand.RangeInclusive(1, 1); ind++) {
                IntVec3 location = new IntVec3(map.Center.x + (int) ((Rand.Value - 0.5) * map.Size.x * 0.75f), map.Center.y + (int) ((Rand.Value - 0.5) * map.Size.y * 0.75f), map.Center.z + (int) ((Rand.Value - 0.5) * map.Size.z * 0.75f));
                genCrater(location, new IntVec3(Rand.RangeInclusive(20, 30), 0, Rand.RangeInclusive(20, 30)), map);
            }
            for (int ind = 1; ind <= Rand.RangeInclusive(1, 1); ind++) {
                IntVec3 location = new IntVec3(map.Center.x + (int) ((Rand.Value - 0.5) * map.Size.x * 0.75f), map.Center.y + (int) ((Rand.Value - 0.5) * map.Size.y * 0.75f), map.Center.z + (int) ((Rand.Value - 0.5) * map.Size.z * 0.75f));
                genCrater(location, new IntVec3(Rand.RangeInclusive(10, 25), 0, Rand.RangeInclusive(10, 25)), map);
            }
            for (int ind = 1; ind <= Rand.RangeInclusive(1, 1); ind++) {
                IntVec3 location = new IntVec3(map.Center.x + (int) ((Rand.Value - 0.5) * map.Size.x * 0.75f), map.Center.y + (int) ((Rand.Value - 0.5) * map.Size.y * 0.75f), map.Center.z + (int) ((Rand.Value - 0.5) * map.Size.z * 0.75f));
                genCrater(location, new IntVec3(Rand.RangeInclusive(5, 15), 0, Rand.RangeInclusive(5, 15)), map);
            }
            foreach (IntVec3 intVec in map.AllCells) {
                if (((intVec.x - map.Center.x) * (intVec.x - map.Center.x)) + ((intVec.z - map.Center.z) * (intVec.z - map.Center.z)) >= (radius * radius)) {
                    Thing thing = GenSpawn.Spawn(RimWorld.ThingDefOf.Sandstone, intVec, map, WipeMode.Vanish);
                    thing.Destroy(DestroyMode.Vanish);
                    if (DefDatabase<TerrainDef>.AllDefs.Contains<TerrainDef>(TerrainDef.Named("OpenSpace"))) {
                        map.terrainGrid.SetTerrain(intVec, DefDatabase<TerrainDef>.GetNamed("OpenSpace"));
                    }
                }
                if (map.fertilityGrid.FertilityAt(intVec) > 0f) {
                    map.terrainGrid.SetTerrain(intVec, DefDatabase<TerrainDef>.GetNamed("lunarMaria"));
                }
            }
            MapGenerator.PlayerStartSpot = new IntVec3(1, 0, 1);
        }

        public void thingSwap(Map map, IntVec3 location, string targetDef, string swapDef) {
            try {
                if (map.thingGrid.ThingAt<Thing>(location).def.defName.Equals(targetDef)) {
                    GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed(swapDef, true), location, map, WipeMode.Vanish);
                }
            } catch { }
        }

        public void terrainSwap(Map map, IntVec3 location, string targetDef) {
            try {
                if (map.terrainGrid.TerrainAt(location).defName.Equals(targetDef)) {
                    map.terrainGrid.SetTerrain(location, DefDatabase<TerrainDef>.GetNamed(targetDef));
                }
            } catch { }
        }

        public void genCrater(IntVec3 position, IntVec3 size, Map map) {
            float A_coef = 1f / (float) (size.x * size.x);
            float B_coef = 1f / (float) (size.z * size.z);
            float n_const = 0.75f * size.x;
            float m_const = 0.75f * size.z;
            float o_const = 0.87f * size.x;
            float p_const = 0.87f * size.z;
            float a_coef = 1f / (n_const * n_const);
            float b_coef = 1f / (m_const * m_const);
            float c_coef = 1f / (o_const * o_const);
            float d_coef = 1f / (p_const * p_const);
            float Xpos;
            float hpos = (size.x * 0.3f) * (Rand.Bool ? -1 : 1);
            float Zpos;
            int kpos = 0;
            for (int dx = (-size.x); dx <= (size.x); dx++) {
                for (int dz = (-size.z); dz <= (size.z); dz++) {
                    Xpos = position.x + dx;
                    Zpos = position.z + dz;
                    if ((A_coef * ((Xpos - position.x) * (Xpos - position.x))) + (B_coef * ((Zpos - position.z) * (Zpos - position.z))) <= 1) {
                        if ((a_coef * ((Xpos - position.x + hpos) * (Xpos - position.x + hpos))) + (b_coef * ((Zpos - position.z + kpos) * (Zpos - position.z + (Rand.Bool ? kpos : -kpos)))) <= 1) {
                            map.terrainGrid.SetTerrain(new IntVec3((int) Xpos, 1, (int) Zpos), DefDatabase<TerrainDef>.GetNamed("lunarMaria"));
                        } else if ((c_coef * ((Xpos - position.x + hpos) * (Xpos - position.x + hpos))) + (d_coef * ((Zpos - position.z + kpos) * (Zpos - position.z + (Rand.Bool ? kpos : -kpos)))) <= 1) {
                            GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed((Rand.Value > 0.65 ? "MineableUranium" : "BiomesNEO_MariaRock"), true), new IntVec3((int) Xpos, 0, (int) Zpos), map, WipeMode.Vanish);
                        } else {
                            GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed((Rand.Value > 0.65 ? "MineablePlasteel" : "BiomesNEO_HighlandRock"), true), new IntVec3((int) Xpos, 0, (int) Zpos), map, WipeMode.Vanish);
                        }
                    }
                }
            }
        }
    }
}
