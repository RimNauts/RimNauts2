using UnityEngine;
using Verse;

namespace RimNauts2 {
    public class RimNauts_GameComponent : GameComponent {
        public RimNauts_GameComponent(Game game) : base() { }
    }

    [StaticConstructorOnStartup]
    public class Satellite : RimWorld.Planet.MapParent {
        public override void ExposeData() {
            base.ExposeData();
            Destroy();
        }

        public override Vector3 DrawPos => Vector3.zero;

        public override void Print(LayerSubMesh subMesh) { }

        public override void Draw() { }
    }
}
