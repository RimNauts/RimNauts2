using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Universum.Utilities {
    public enum Vacuum_Protection {
        None = 0,
        Oxygen = 1,
        Decompression = 2,
        All = 3,
    }

    public class WeatherEvent_Decompression : Verse.WeatherEvent {
        public WeatherEvent_Decompression(Verse.Map map) : base(map) { }

        public override bool Expired => true;

        public override void FireEvent() {
            bool vacuum_decompression = Cache.allowed_utility(map, "Universum.vacuum_decompression");
            bool vacuum_suffocation = Cache.allowed_utility(map, "Universum.vacuum_suffocation");
            if (!vacuum_decompression && !vacuum_suffocation) return;
            List<Verse.Pawn> allPawns = base.map.mapPawns.AllPawnsSpawned;
            List<Verse.Pawn> pawnsToDamage = new List<Verse.Pawn>();
            List<Verse.Pawn> pawnsToSuffocate = new List<Verse.Pawn>();
            foreach (Verse.Pawn pawn in allPawns.Where(p => !p.Dead)) {
                Verse.Room room = pawn.Position.GetRoom(map);
                bool vacuum = room == null || room.OpenRoofCount > 0 || room.TouchesMapEdge;
                if (vacuum) {
                    Vacuum_Protection protection = Cache.spacesuit_protection(pawn);
                    Log.Message(protection.ToString());
                    switch (protection) {
                        case Vacuum_Protection.None:
                            if (vacuum_decompression) pawnsToDamage.Add(pawn);
                            if (vacuum_suffocation) pawnsToSuffocate.Add(pawn);
                            break;
                        case Vacuum_Protection.Oxygen:
                            if (vacuum_decompression) pawnsToDamage.Add(pawn);
                            break;
                        case Vacuum_Protection.Decompression:
                            if (vacuum_suffocation) pawnsToSuffocate.Add(pawn);
                            break;
                    }
                } else { /* Add life support system here */ }
            }
            foreach (Verse.Pawn thePawn in pawnsToDamage) {
                thePawn.TakeDamage(new Verse.DamageInfo(Verse.DefDatabase<Verse.DamageDef>.GetNamed("Universum_Decompression_Damage"), 1));
            }
            foreach (Verse.Pawn thePawn in pawnsToSuffocate) {
                Verse.HealthUtility.AdjustSeverity(thePawn, Verse.HediffDef.Named("Universum_Suffocation_Hediff"), 0.05f);
            }
        }

        public override void WeatherEventTick() { }
    }
}
