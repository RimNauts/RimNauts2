using System.Linq;

namespace Universum.Utilities.Biome {
    public static class Handler {
        private static int total_biomes_found;
        private static int total_configurations_found;
        private static Properties[] properties;
        public static Properties get_properties(this RimWorld.BiomeDef biome_def) {
            try {
                return properties[biome_def.index];
            } catch {
                return new Properties();
            }
        }

        public static void Init() {
            properties = Properties.GetAll();
            // set stats
            total_biomes_found = properties.Count();
            total_configurations_found = 0;
            foreach (Properties property in properties) {
                total_configurations_found += property.allowed_utilities.Count();
            }
            // print stats
            Verse.Log.Message("\t" + total_biomes_found + " biomes found");
            Verse.Log.Message("\t" + total_configurations_found + " configurations found");
        }
    }
}
