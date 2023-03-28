using HarmonyLib;
using UnityEngine;
using Verse.Steam;
using Verse;

namespace RimNauts2.World.Patch {
    [HarmonyPatch(typeof(RimWorld.Planet.WorldCameraDriver), "AltitudePercent", MethodType.Getter)]
    class WorldCameraDriver_AltitudePercent {
        public static bool Prefix(ref RimWorld.Planet.WorldCameraDriver __instance, ref float __result) {
            __result = Mathf.InverseLerp(RimWorld.Planet.WorldCameraDriver.MinAltitude, Defs.Of.general.max_altitude, __instance.altitude);
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldCameraDriver), "WorldCameraDriverOnGUI")]
    class WorldCameraDriver_WorldCameraDriverOnGUI {
        public static bool Prefix(ref RimWorld.Planet.WorldCameraDriver __instance) {
            if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
                __instance.releasedLeftWhileHoldingMiddle = true;
            else if (Event.current.rawType == EventType.MouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
                __instance.releasedLeftWhileHoldingMiddle = false;
            __instance.mouseCoveredByUI = false;
            if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
                __instance.mouseCoveredByUI = true;
            if (__instance.AnythingPreventsCameraMotion)
                return false;
            if (!UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild && Event.current.type == EventType.MouseDrag && Event.current.button == 2 || UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild && Input.GetMouseButton(2) && (!SteamDeck.IsSteamDeck || !Find.WorldSelector.AnyCaravanSelected)) {
                Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
                if (Event.current.type == EventType.MouseDrag)
                    Event.current.Use();
                if (currentEventDelta != Vector2.zero) {
                    RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.FrameInteraction);
                    currentEventDelta.x *= -1f;
                    __instance.desiredRotationRaw += currentEventDelta / RimWorld.Planet.GenWorldUI.CurUITileSize() * 0.273f * (Prefs.MapDragSensitivity * Defs.Of.general.drag_sensitivity_multiplier);
                }
            }
            float num = 0.0f;
            if (Event.current.type == EventType.ScrollWheel) {
                num -= Event.current.delta.y * 0.1f;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            if (RimWorld.KeyBindingDefOf.MapZoom_In.KeyDownEvent) {
                num += 2f;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            if (RimWorld.KeyBindingDefOf.MapZoom_Out.KeyDownEvent) {
                num -= 2f;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            __instance.desiredAltitude -= (float) ((double) num * (double) (__instance.config.zoomSpeed * Defs.Of.general.zoom_sensitivity_multiplier) * (double) __instance.altitude / 12.0);
            __instance.desiredAltitude = Mathf.Clamp(__instance.desiredAltitude, RimWorld.Planet.WorldCameraDriver.MinAltitude, Defs.Of.general.max_altitude);
            __instance.desiredRotation = Vector2.zero;
            if (RimWorld.KeyBindingDefOf.MapDolly_Left.IsDown) {
                __instance.desiredRotation.x = -__instance.config.dollyRateKeys;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            if (RimWorld.KeyBindingDefOf.MapDolly_Right.IsDown) {
                __instance.desiredRotation.x = __instance.config.dollyRateKeys;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            if (RimWorld.KeyBindingDefOf.MapDolly_Up.IsDown) {
                __instance.desiredRotation.y = __instance.config.dollyRateKeys;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            if (RimWorld.KeyBindingDefOf.MapDolly_Down.IsDown) {
                __instance.desiredRotation.y = -__instance.config.dollyRateKeys;
                RimWorld.PlayerKnowledgeDatabase.KnowledgeDemonstrated(RimWorld.ConceptDefOf.WorldCameraMovement, RimWorld.KnowledgeAmount.SpecificInteraction);
            }
            __instance.config.ConfigOnGUI();
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldCameraDriver), "Update")]
    class WorldCameraDriver_Update {
        public static bool Prefix(ref RimWorld.Planet.WorldCameraDriver __instance) {
            if (LongEventHandler.ShouldWaitForEvent)
                return false;
            if (Find.World == null) {
                __instance.MyCamera.gameObject.SetActive(false);
            } else {
                if (!Find.WorldInterface.everReset)
                    Find.WorldInterface.Reset();
                Vector2 curInputDollyVect = __instance.CalculateCurInputDollyVect();
                if (curInputDollyVect != Vector2.zero) {
                    float num = (float) (((double) __instance.altitude - (double) RimWorld.Planet.WorldCameraDriver.MinAltitude) / (Defs.Of.general.max_altitude - (double) RimWorld.Planet.WorldCameraDriver.MinAltitude) * 0.850000023841858 + 0.150000005960464);
                    __instance.rotationVelocity = new Vector2(curInputDollyVect.x, curInputDollyVect.y) * num;
                }
                if ((!Input.GetMouseButton(2) || SteamDeck.IsSteamDeck && __instance.releasedLeftWhileHoldingMiddle) && __instance.dragTimeStamps.Any()) {
                    __instance.rotationVelocity += CameraDriver.GetExtraVelocityFromReleasingDragButton(__instance.dragTimeStamps, 5f * Defs.Of.general.drag_velocity_multiplier);
                    __instance.dragTimeStamps.Clear();
                }
                if (!__instance.AnythingPreventsCameraMotion) {
                    float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
                    __instance.sphereRotation *= Quaternion.AngleAxis(__instance.rotationVelocity.x * num * __instance.config.rotationSpeedScale, __instance.MyCamera.transform.up);
                    __instance.sphereRotation *= Quaternion.AngleAxis(-__instance.rotationVelocity.y * num * __instance.config.rotationSpeedScale, __instance.MyCamera.transform.right);
                    if (__instance.desiredRotationRaw != Vector2.zero) {
                        __instance.sphereRotation *= Quaternion.AngleAxis(__instance.desiredRotationRaw.x, __instance.MyCamera.transform.up);
                        __instance.sphereRotation *= Quaternion.AngleAxis(-__instance.desiredRotationRaw.y, __instance.MyCamera.transform.right);
                    }
                    __instance.dragTimeStamps.Add(new CameraDriver.DragTimeStamp() {
                        posDelta = __instance.desiredRotationRaw,
                        time = Time.time
                    });
                }
                __instance.desiredRotationRaw = Vector2.zero;
                int num1 = Gen.FixedTimeStepUpdate(ref __instance.fixedTimeStepBuffer, 60f);
                for (int index = 0; index < num1; ++index) {
                    if (__instance.rotationVelocity != Vector2.zero) {
                        __instance.rotationVelocity *= __instance.config.camRotationDecayFactor;
                        if ((double) __instance.rotationVelocity.magnitude < 0.0500000007450581)
                            __instance.rotationVelocity = Vector2.zero;
                    }
                    if (__instance.config.smoothZoom) {
                        float num2 = Mathf.Lerp(__instance.altitude, __instance.desiredAltitude, 0.05f);
                        __instance.desiredAltitude += (num2 - __instance.altitude) * __instance.config.zoomPreserveFactor;
                        __instance.altitude = num2;
                    } else {
                        float num2 = (float) (((double) __instance.desiredAltitude - (double) __instance.altitude) * 0.400000005960464);
                        __instance.desiredAltitude += __instance.config.zoomPreserveFactor * num2;
                        __instance.altitude += num2;
                    }
                }
                __instance.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
                if (Find.PlaySettings.lockNorthUp) {
                    __instance.RotateSoNorthIsUp(false);
                    __instance.ClampXRotation(ref __instance.sphereRotation);
                }
                for (int index = 0; index < num1; ++index)
                    __instance.config.ConfigFixedUpdate_60(ref __instance.rotationVelocity);
                __instance.ApplyPositionToGameObject();
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(RimWorld.Planet.WorldCameraDriver), "MinAltitude", MethodType.Getter)]
    class WorldCameraDriver_MinAltitude {
        public static bool Prefix(ref float __result) {
            __result = (float) ((double) Defs.Of.general.min_altitude + (SteamDeck.IsSteamDeck ? 17.0 : 25.0));
            return false;
        }
    }
}
