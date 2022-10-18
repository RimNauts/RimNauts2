using HarmonyLib;
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;

namespace RimNauts2 {
    [HarmonyPatch(typeof(RimWorld.IncidentWorker), "CanFireNow")]
    public class IncidentWorker_CanFireNow {
        static int wait = 0;

        public static void Postfix(ref RimWorld.IncidentWorker __instance, RimWorld.IncidentParms parms, ref bool __result) {
            if (!__result) return;
            incident = __instance.def.defName;
            bool incident_not_on_moon_biome = Find.WorldGrid[parms.target.Tile].biome != BiomeDefOf.RockMoonBiome;
            if (incident_not_on_moon_biome) return;
            if (allowed_incidents.Contains(incident)) return;
            __result = false;
            if (wait > 0) {
                wait--;
                return;
            }

            Map target = (Map) parms.target;
            IntVec3 cell;
            if (!TryFindCell(out cell, target)) {
                return;
            }
            List<Thing> thingList = RimWorld.ThingSetMakerDefOf.Meteorite.root.Generate();
            RimWorld.SkyfallerMaker.SpawnSkyfaller(RimWorld.ThingDefOf.MeteoriteIncoming, thingList, cell, target);
            LetterDef baseLetterDef = thingList[0].def.building.isResourceRock ? RimWorld.LetterDefOf.PositiveEvent : RimWorld.LetterDefOf.NeutralEvent;
            string str = "A large meteorite has struck ground in the area. It has left behind a lump of " + thingList[0].def.label + ".";
            SendStandardLetter("Meteorite: " + thingList[0].def.LabelCap, str, baseLetterDef, parms, new TargetInfo(cell, target), Array.Empty<NamedArgument>());
            if (Prefs.DevMode) Log.Message("RimNauts2: Replaced '" + incident + "' incident to Metorite incident.");
            wait = 5;
            return;
        }

        private static bool TryFindCell(out IntVec3 cell, Map map) {
            int maxMineables = RimWorld.ThingSetMaker_Meteorite.MineablesCountRange.max;
            return CellFinderLoose.TryFindSkyfallerCell(
                RimWorld.ThingDefOf.MeteoriteIncoming,
                map,
                out cell,
                alwaysAvoidColonists: true,
                extraValidator: x => {
                    int num1 = Mathf.CeilToInt(Mathf.Sqrt(maxMineables)) + 2;
                    CellRect cellRect = CellRect.CenteredOn(x, num1, num1);
                    int num2 = 0;
                    foreach (IntVec3 c in cellRect) {
                        if (c.InBounds(map) && c.Standable(map))
                            ++num2;
                    }
                    return num2 >= maxMineables;
                }
            );
        }

        private static void SendStandardLetter(
            TaggedString baseLetterLabel,
            TaggedString baseLetterText,
            LetterDef baseLetterDef,
            RimWorld.IncidentParms parms,
            LookTargets lookTargets,
            params NamedArgument[] textArgs
        ) {
            if (!parms.sendLetter) return;
            if (baseLetterLabel.NullOrEmpty() || baseLetterText.NullOrEmpty())
                Log.Error("Sending standard incident letter with no label or text.");
            TaggedString taggedString1 = baseLetterText.Formatted(textArgs);
            TaggedString text;
            if (parms.customLetterText.NullOrEmpty()) {
                text = taggedString1;
            } else {
                List<NamedArgument> namedArgumentList = new List<NamedArgument>();
                if (textArgs != null)
                    namedArgumentList.AddRange(textArgs);
                namedArgumentList.Add(taggedString1.Named("BASETEXT"));
                text = parms.customLetterText.Formatted(namedArgumentList.ToArray());
            }
            TaggedString taggedString2 = baseLetterLabel.Formatted(textArgs);
            TaggedString label;
            if (parms.customLetterLabel.NullOrEmpty()) {
                label = taggedString2;
            } else {
                List<NamedArgument> namedArgumentList = new List<NamedArgument>();
                if (textArgs != null)
                    namedArgumentList.AddRange(textArgs);
                namedArgumentList.Add(taggedString2.Named("BASELABEL"));
                label = parms.customLetterLabel.Formatted(namedArgumentList.ToArray());
            }
            ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, parms.customLetterDef ?? baseLetterDef, lookTargets, parms.faction, parms.quest, parms.letterHyperlinkThingDefs);

            Find.LetterStack.ReceiveLetter(choiceLetter);
        }

        private static string incident = "";
        private static readonly List<string> allowed_incidents = new List<string>() {
            "Disease_AnimalFlu",
            "Disease_AnimalPlague",
            "Disease_FibrousMechanites",
            "Disease_Flu",
            "Disease_Plague",
            "Disease_SensoryMechanites",
            "Disease_SleepingSickness",
            "MeteoriteImpact",
            "OrbitalTraderArrival",
            "PsychicDrone",
            "PsychicSoothe",
            "ResourcePodCrash",
            "ShipChunkDrop",
        };
    }
}
