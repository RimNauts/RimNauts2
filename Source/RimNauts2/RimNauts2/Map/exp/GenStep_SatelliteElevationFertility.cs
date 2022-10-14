using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimNauts2 {
    public class GenStep_SatelliteElevationFertility : GenStep {
        public override int SeedPart {
            get {
                return 826504671;
            }
        }

        public override void Generate(Map map, GenStepParams parms) {
            NoiseRenderer.renderSize = new IntVec2(map.Size.x, map.Size.z);
            ModuleBase moduleBase = new Perlin(0.020999999716877937, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
            moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
            NoiseDebugUI.StoreNoiseRender(moduleBase, "elev base");
            float num = 1f;
            switch (map.TileInfo.hilliness) {
                case RimWorld.Planet.Hilliness.Flat:
                    num = RimWorld.MapGenTuning.ElevationFactorFlat;
                    break;
                case RimWorld.Planet.Hilliness.SmallHills:
                    num = RimWorld.MapGenTuning.ElevationFactorSmallHills;
                    break;
                case RimWorld.Planet.Hilliness.LargeHills:
                    num = RimWorld.MapGenTuning.ElevationFactorLargeHills;
                    break;
                case RimWorld.Planet.Hilliness.Mountainous:
                    num = RimWorld.MapGenTuning.ElevationFactorMountains;
                    break;
                case RimWorld.Planet.Hilliness.Impassable:
                    num = RimWorld.MapGenTuning.ElevationFactorImpassableMountains;
                    break;
            }
            moduleBase = new Multiply(moduleBase, new Const(num));
            NoiseDebugUI.StoreNoiseRender(moduleBase, "elev world-factored");
            if (map.TileInfo.hilliness == RimWorld.Planet.Hilliness.Mountainous || map.TileInfo.hilliness == RimWorld.Planet.Hilliness.Impassable) {
                ModuleBase moduleBase2 = new DistFromAxis(map.Size.x * 0.42f);
                moduleBase2 = new Clamp(0.0, 1.0, moduleBase2);
                moduleBase2 = new Invert(moduleBase2);
                moduleBase2 = new ScaleBias(1.0, 1.0, moduleBase2);
                Rot4 random;
                do {
                    random = Rot4.Random;
                }
                while (random == Find.World.CoastDirectionAt(map.Tile));
                if (random == Rot4.North) {
                    moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
                    moduleBase2 = new Translate(0.0, 0.0, (-map.Size.z), moduleBase2);
                } else if (random == Rot4.East) {
                    moduleBase2 = new Translate((-map.Size.x), 0.0, 0.0, moduleBase2);
                } else if (random == Rot4.South) {
                    moduleBase2 = new Rotate(0.0, 90.0, 0.0, moduleBase2);
                }
                NoiseDebugUI.StoreNoiseRender(moduleBase2, "mountain");
                moduleBase = new Add(moduleBase, moduleBase2);
                NoiseDebugUI.StoreNoiseRender(moduleBase, "elev + mountain");
            }
            float b = map.TileInfo.WaterCovered ? 0f : float.MaxValue;
            MapGenFloatGrid elevation = MapGenerator.Elevation;
            foreach (IntVec3 intVec in map.AllCells) {
                elevation[intVec] = Mathf.Min(moduleBase.GetValue(intVec), b);
            }
            ModuleBase moduleBase3 = new Perlin(0.020999999716877937, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
            moduleBase3 = new ScaleBias(0.5, 0.5, moduleBase3);
            NoiseDebugUI.StoreNoiseRender(moduleBase3, "noiseFert base");
            MapGenFloatGrid fertility = MapGenerator.Fertility;
            foreach (IntVec3 intVec2 in map.AllCells) {
                fertility[intVec2] = moduleBase3.GetValue(intVec2);
            }
        }
    }
}
