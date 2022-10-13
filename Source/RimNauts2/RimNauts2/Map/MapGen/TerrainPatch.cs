using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.GenStep_Terrain), "Generate")]
    static class TerrainPatch {
        static bool Prefix(Map map, GenStepParams parms) {
            // check if it's our biome. If not, skip the patch
            if (map.Biome.defName != "RockMoonBiome") {
                return true;
            }

            (new GenStep_MoonTerrain()).Generate(map, parms);
            return false;
        }
    }
}
