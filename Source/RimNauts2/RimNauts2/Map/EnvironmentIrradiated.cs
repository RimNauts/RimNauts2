using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Universum.Utilities;
using Verse;

namespace RimNauts2 {
    class EnvironmentIrradiated : GenStep {
        public override int SeedPart => 262606459;                                      // my research leads me to believe that this doesn't matter, and that the game has
                                                                                        // failsafe in place to stop duplicates... however, perhaps we should implement some sort of randomization?
        public override void Generate(Map map, GenStepParams parms) {
            map.weatherManager.lastWeather = WeatherDef.Named("RimNauts2_ToxicOuterSpaceWeather");
            map.weatherManager.curWeather = WeatherDef.Named("RimNauts2_ToxicOuterSpaceWeather");
            GameConditionManager gameConditionManager = map.gameConditionManager;
            GameCondition condition = GameConditionMaker.MakeConditionPermanent(GameConditionDefOf.CompletelyIrradiated);
            gameConditionManager.RegisterCondition(condition);
        }
    }
    /*class WeatherEvent_CompletelyIrradiated : WeatherEvent {
        public WeatherEvent_CompletelyIrradiated(Map map) : base(map) { }
        public override bool Expired => true;]
        public override void FireEvent() {
            List<GameCondition> conditions = map.gameConditionManager.ActiveConditions;
            for (int i = 0; i < conditions.Count; i++) {
                if (conditions[i] is GameCondition_* cond) {
                                                                                        // do stuff... you could even clear the CompletelyIrradiated GameCondition if, say,
                                                                                        // X amount of Uranium ore is mined after X amount of time. for now it is permanent
                }
            }
        }
        public override void WeatherEventTick() { }
    }*/
    class GameCondition_CompletelyIrradiated : GameCondition_ToxicFallout {             // there is much cosmetic stuff that appears to be overrideable or patchable as well, so i am certain we can implement additional
                                                                                        // desired effects in the future by inheriting from GameCondition_ToxicFallout
        public override void DoCellSteadyEffects(IntVec3 c, Map map) {
            if (c.Roofed(map))
                return;
            List<Thing> thingList = c.GetThingList(map);
            for (int index=0; index < thingList.Count; ++index)                         // Iterate through all things on map
            {
                Thing thing = thingList[index];
                if (thing.def.category == ThingCategory.Item) {                         // Rot rottable things faster
                    CompRottable comp = thing.TryGetComp<CompRottable>();
                    if (comp != null && comp.Stage < RotStage.Dessicated)
                        comp.RotProgress += 3000f;
                }
                                                                                        // TODO: Add desired effects to on anything on the map; this is decompiled code i have used override with,
                                                                                        // not including certain things like killing plants that i think should be covered in Universum's WeatherEvent_Vacuum instead.
                                                                                        // Maybe add custom hediffs. For now I am thinking about just hammering map pawns with carcinomas based on the severity of
                                                                                        // toxic buildup that each pawn is already getting from the inherited vanilla GameCondition_ToxicFallout.DoPawnToxicDamage() method
            }
        }
    }
}