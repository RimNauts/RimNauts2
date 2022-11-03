using Verse;

namespace RimNauts2.SpaceSuit {
    [StaticConstructorOnStartup]
    class LightModule : RimWorld.Apparel {
        private readonly int wait_time_max = 100;
        private readonly int wait_time_min = -50;
        private readonly int radius = 6;
        private IntVec3 prev_pos;
        private bool lights_turned_off;
        private int wait_time;
        private Rot4 prev_rotation;
        private CompGlower light;
        private Map map;

        public LightModule() {
            wait_time = wait_time_min;
            prev_rotation = Rot4.South;
            lights_turned_off = true;

            light = new CompGlower {
                props = new RimWorld.CompProperties_Glower()
            };
            ((RimWorld.CompProperties_Glower) light.props).glowColor = new ColorInt(255, 255, 255, 0);
            ((RimWorld.CompProperties_Glower) light.props).glowRadius = radius;
            light.parent = this;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref prev_pos, "prev_pos");
            Scribe_Values.Look(ref lights_turned_off, "lights_turned_off", true);
            Scribe_Values.Look(ref wait_time, "wait_time", wait_time_min);
            Scribe_Values.Look(ref prev_rotation, "prev_rotation", Rot4.South);

            light = new CompGlower {
                props = new RimWorld.CompProperties_Glower()
            };
            ((RimWorld.CompProperties_Glower) light.props).glowColor = new ColorInt(255, 255, 255, 0);
            ((RimWorld.CompProperties_Glower) light.props).glowRadius = radius;
            light.parent = this;
        }

        public override void Tick() {
            base.Tick();
            IntVec3 rotation = new IntVec3(0, 0, 0);
            if (Wearer != null) {
                map = Wearer.Map;
                if (Wearer.Rotation == Rot4.East) {
                    rotation.x += 1;
                } else if (Wearer.Rotation == Rot4.North) {
                    rotation.z += 1;
                } else if (Wearer.Rotation == Rot4.West) {
                    rotation.x -= 1;
                } else if (Wearer.Rotation == Rot4.South) {
                    rotation.z -= 1;
                }
            }
            if (Wearer == null || !is_dark(rotation) || Wearer.InContainerEnclosed) {
                if (lights_turned_off) return;
                if (Wearer == null || Wearer.InContainerEnclosed) {
                    turn_lights_off();
                    lights_turned_off = true;
                    wait_time = wait_time_min;
                } else if (wait_time > 0) {
                    wait_time--;
                    if (Wearer.Position == prev_pos && Wearer.Rotation == prev_rotation) return;
                    update_light_position(rotation);
                    return;
                }
                turn_lights_off();
                lights_turned_off = true;
                wait_time = wait_time_min;
            } else if (lights_turned_off) {
                if (wait_time < 0) {
                    wait_time++;
                    return;
                }
                turn_lights_on(rotation);
                lights_turned_off = false;
                wait_time = wait_time_max;
            } else {
                if (wait_time < wait_time_max) wait_time = wait_time_max;
                lights_turned_off = false;
                if (Wearer.Position == prev_pos && Wearer.Rotation == prev_rotation) return;
                update_light_position(rotation);
            }
        }

        private void update_light_position(IntVec3 rotation) {
            turn_lights_off();
            turn_lights_on(rotation);
        }

        private void turn_lights_off() {
            map.glowGrid.DeRegisterGlower(light);
        }

        private void turn_lights_on(IntVec3 rotation) {
            // update light values
            light.parent.Position = rotation + Wearer.Position;
            map.glowGrid.RegisterGlower(light);
            // update values
            prev_pos = Wearer.Position;
            prev_rotation = Wearer.Rotation;
        }

        private bool is_dark(IntVec3 rotation) {
            float total_light_value = 0.0f;
            int radius_offset = radius - 1;
            int radius_offset_half = (radius / 2) + 1;

            if (Wearer.Rotation == Rot4.East) {
                total_light_value += light_value(rotation + new IntVec3(0, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(1, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, -radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, -1));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 0));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 1));
                total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(1, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(0, 0, radius_offset));
            } else if (Wearer.Rotation == Rot4.North) {
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 0));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 1));
                total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(1, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(0, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(-1, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 1));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 0));
            } else if (Wearer.Rotation == Rot4.West) {
                total_light_value += light_value(rotation + new IntVec3(0, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(-1, 0, radius_offset));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 1));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 0));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, -1));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, -radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(-1, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(0, 0, -radius_offset));
            } else if (Wearer.Rotation == Rot4.South) {
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 0));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, -1));
                total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, -radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(-1, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(0, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(1, 0, -radius_offset));
                total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, -radius_offset_half));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, -1));
                total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 0));
            }
            // top right
            total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 0));
            total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, 1));
            total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, radius_offset_half));
            total_light_value += light_value(rotation + new IntVec3(1, 0, radius_offset));
            // top left
            total_light_value += light_value(rotation + new IntVec3(0, 0, radius_offset));
            total_light_value += light_value(rotation + new IntVec3(-1, 0, radius_offset));
            total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, radius_offset_half));
            total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 1));
            // bottom left
            total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, 0));
            total_light_value += light_value(rotation + new IntVec3(-radius_offset, 0, -1));
            total_light_value += light_value(rotation + new IntVec3(-radius_offset_half, 0, -radius_offset_half));
            total_light_value += light_value(rotation + new IntVec3(-1, 0, -radius_offset));
            // bottom right
            total_light_value += light_value(rotation + new IntVec3(0, 0, -radius_offset));
            total_light_value += light_value(rotation + new IntVec3(1, 0, -radius_offset));
            total_light_value += light_value(rotation + new IntVec3(radius_offset_half, 0, -radius_offset_half));
            total_light_value += light_value(rotation + new IntVec3(radius_offset, 0, -1));

            float avg_light_value = total_light_value / 25.0f;

            return avg_light_value < 0.349999994039536;
        }

        private float light_value(IntVec3 offset) => map.glowGrid.GameGlowAt(offset + Wearer.Position, ignoreCavePlants: true);
    }
}
