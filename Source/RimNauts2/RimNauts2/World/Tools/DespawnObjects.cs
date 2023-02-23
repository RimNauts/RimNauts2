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
            Caching_Handler.render_manager.depopulate(Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (1)",
            displayPriority: 6,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid() {
            Caching_Handler.render_manager.depopulate(amount: 1, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (10)",
            displayPriority: 5,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_ten() {
            Caching_Handler.render_manager.depopulate(amount: 10, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn Asteroid (100)",
            displayPriority: 4,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_houndred() {
            Caching_Handler.render_manager.depopulate(amount: 100, Type.Asteroid);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn all AsteroidCrashing",
            displayPriority: 3,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_all() {
            Caching_Handler.render_manager.depopulate(Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (1)",
            displayPriority: 2,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing() {
            Caching_Handler.render_manager.depopulate(amount: 1, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (10)",
            displayPriority: 1,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_ten() {
            Caching_Handler.render_manager.depopulate(amount: 10, Type.AsteroidCrashing);
        }

        [DebugAction(
            category: "RimNauts 2: Despawn",
            name: "Despawn AsteroidCrashing (100)",
            displayPriority: 0,
            actionType = DebugActionType.Action,
            allowedGameStates = AllowedGameStates.PlayingOnWorld
        )]
        public static void despawn_asteroid_crashing_houndred() {
            Caching_Handler.render_manager.depopulate(amount: 100, Type.AsteroidCrashing);
        }
    }
}
