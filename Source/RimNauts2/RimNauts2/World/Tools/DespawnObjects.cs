using Verse;

namespace RimNauts2.World.Tools {
    public static class DespawnObjects {
        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn all Asteroid",
            displayPriority: 7,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_all() {
            Generator.remove_visual_object(Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (1)",
            displayPriority: 6,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid() {
            Generator.remove_visual_object(amount: 1, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (10)",
            displayPriority: 5,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_ten() {
            Generator.remove_visual_object(amount: 10, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (100)",
            displayPriority: 4,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_houndred() {
            Generator.remove_visual_object(amount: 100, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn all AsteroidCrashing",
            displayPriority: 3,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_all() {
            Generator.remove_visual_object(Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (1)",
            displayPriority: 2,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing() {
            Generator.remove_visual_object(amount: 1, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (10)",
            displayPriority: 1,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_ten() {
            Generator.remove_visual_object(amount: 10, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (100)",
            displayPriority: 0,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_houndred() {
            Generator.remove_visual_object(amount: 100, Type.AsteroidCrashing);
        }
    }
}
