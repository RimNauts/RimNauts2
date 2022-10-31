using Verse;

namespace RimNauts2.SpaceSuit {
    [StaticConstructorOnStartup]
    class LightModule : RimWorld.Apparel {
        private CompGlower light;
        private IntVec3 prev_pos;
        private bool lights_turned_off = true;
        private Map map;

        public LightModule() {
            CompGlower compGlower1 = new CompGlower();
            compGlower1.props = new RimWorld.CompProperties_Glower();
            light = compGlower1;
            ((RimWorld.CompProperties_Glower) light.props).glowColor = new ColorInt(255, 255, 255, 255);
            ((RimWorld.CompProperties_Glower) light.props).glowRadius = 3f;
            light.parent = this;
        }

        public override void Tick() {
            base.Tick();
            if (Wearer != null && Wearer.Map != null) map = Wearer.Map;
            if (Wearer == null || !is_dark()) {
                if (lights_turned_off) return;
                turn_lights_off();
            } else if (Wearer.InContainerEnclosed) {
                if (lights_turned_off) return;
                turn_lights_off();
            } else if (lights_turned_off) {
                turn_lights_on();
            } else {
                if (prev_pos.IsInside(Wearer)) return;
                update_light_position();
            }
        }

        private void update_light_position() {
            turn_lights_off();
            turn_lights_on();
        }

        private void turn_lights_off() {
            // update light values
            map.mapDrawer.MapMeshDirty(prev_pos, MapMeshFlag.Things);
            map.glowGrid.DeRegisterGlower(light);
            // update values
            lights_turned_off = true;
        }

        private void turn_lights_on() {
            // update light values
            map.mapDrawer.MapMeshDirty(Wearer.Position, MapMeshFlag.Things);
            map.glowGrid.RegisterGlower(light);
            light.parent.Position = Wearer.Position;
            // update values
            prev_pos = Wearer.Position;
            lights_turned_off = false;
        }

        private bool is_dark() => light_value() < 0.349999994039536;

        private float light_value() => map.glowGrid.GameGlowAt(Wearer.Position, ignoreCavePlants: true);
    }
}
