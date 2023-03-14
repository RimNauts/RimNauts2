using Verse;

namespace RimNauts2.World.Tools {
    public static class SpawnObjects {
        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid Ore (1)",
            displayPriority: 8,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_ore() {
            Generator.add_object_holder(amount: 1, Type.AsteroidOre);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid Ore (10)",
            displayPriority: 7,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_ore_ten() {
            Generator.add_object_holder(amount: 10, Type.AsteroidOre);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid Ore (100)",
            displayPriority: 6,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_ore_houndred() {
            Generator.add_object_holder(amount: 100, Type.AsteroidOre);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid (1)",
            displayPriority: 5,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid() {
            Generator.add_visual_object(amount: 1, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid (10)",
            displayPriority: 4,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_ten() {
            Generator.add_visual_object(amount: 10, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn Asteroid (100)",
            displayPriority: 3,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_houndred() {
            Generator.add_visual_object(amount: 100, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn AsteroidCrashing (1)",
            displayPriority: 2,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void spawn_asteroid_crashing() {
            Generator.add_visual_object(amount: 1, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn AsteroidCrashing (10)",
            displayPriority: 1,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_ten() {
            Generator.add_visual_object(amount: 10, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Spawn",
            name: "Spawn AsteroidCrashing (100)",
            displayPriority: 0,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_houndred() {
            Generator.add_visual_object(amount: 100, Type.AsteroidCrashing);
        }
    }
}
