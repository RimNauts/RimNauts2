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
            ((RimWorld.CompProperties_Glower) light.props).glowColor = new ColorInt(255, 255, 255, 0);
            ((RimWorld.CompProperties_Glower) light.props).glowRadius = 6f;
            ((RimWorld.CompProperties_Glower) light.props).darklightToggle = true;
            light.parent = this;
        }

        public override void Tick() {
            base.Tick();
            Log.Clear();
            if (Wearer != null) map = Wearer.Map;
            if (Wearer != null) {
                Log.Message("curr pos: " + Wearer.Position);
                Log.Message("prev pos: " + prev_pos);
                Log.Message("same pos: " + (Wearer.Position == prev_pos));
                Log.Message("lights_turned_off: " + lights_turned_off);
                Log.Message("is_dark: " + is_dark());
                Log.Message("InContainerEnclosed: " + Wearer.InContainerEnclosed);
            }
            if (Wearer == null || !is_dark() || Wearer.InContainerEnclosed) {
                if (lights_turned_off) return;
                Log.Message("turn_lights_off: true");
                turn_lights_off();
                lights_turned_off = true;
            } else if (lights_turned_off) {
                Log.Message("turn_lights_on: true");
                turn_lights_on();
                lights_turned_off = false;
            } else {
                lights_turned_off = false;

                if (Wearer.Position == prev_pos) {
                    Log.Message("update_light_position: false");
                    return;
                }
                Log.Message("update_light_position: true");
                update_light_position();
            }
        }

        private void update_light_position() {
            turn_lights_off();
            turn_lights_on();
        }

        private void turn_lights_off() {
            map.glowGrid.DeRegisterGlower(light);
        }

        private void turn_lights_on() {
            // update light values
            light.parent.Position = Wearer.Position;
            map.glowGrid.RegisterGlower(light);
            // update values
            prev_pos = Wearer.Position;
        }

        private bool is_dark() {
            if (!lights_turned_off) map.glowGrid.DeRegisterGlower(light);
            map.mapDrawer.MapMeshDirty(prev_pos, MapMeshFlag.Things);
            bool flag = light_value() < 0.349999994039536;
            Log.Message("light_value: " + light_value());
            if (!lights_turned_off) map.glowGrid.RegisterGlower(light);
            map.mapDrawer.MapMeshDirty(prev_pos, MapMeshFlag.Things);
            return true;
        }

        private float light_value() => map.glowGrid.GameGlowAt(Wearer.Position, ignoreCavePlants: true);
    }
}
