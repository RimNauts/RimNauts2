using RimWorld.Planet;
using RimWorld;

namespace RimNauts2 {
    public class BiomeWorker_Satellite : BiomeWorker {
        public override float GetScore(Tile tile, int tileID) {
            return -999f;
        }
    }
}
