

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Scriptables;
using UnityEngine;
using Cinemachine;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.Units.Actions;
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.EffectsSystem;
using Edu.Vfs.RoboRapture.AudioSystem;

namespace Edu.Vfs.RoboRapture.CameraSystem
{
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera GameplayCamera;

        [SerializeField]
        private CinemachineVirtualCamera DynamicCamera;

        [SerializeField]
        private GameObject DynamicCameraTracker;

        [SerializeField][Tooltip("How long does the camera stay active for?")]
        private float DynamicCameraLifetime;

        [SerializeField]
        private float ShakeDelay;

        [SerializeField]
        private CinemachineImpulseSource[] ScreenShakes;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private DebugOneShot cameraZoomSFX;

        private Unit SelectedUnit;
        private ActionType SelectedAction;
        private SkillAction ActionPerformed;
        private Unit Target;
        private Unit Instigator;
        private bool ActionCancelled = false;
        private bool HasTarget = false;

        private void OnEnable()
        {
            SkillAction.SkillActionExecuted       += SetSkillAction;
            Selector.CancelledAction              += PlayerCancelledAction;
        }

        private void OnDisable()
        {
            SkillAction.SkillActionExecuted       -= SetSkillAction;
            Selector.CancelledAction              -= PlayerCancelledAction;
        }

        private void SetSkillAction(SkillAction action, Point target)
        {
            ActionPerformed = action;

            SelectedUnit = action.Unit; 

            SelectedAction = action.ActionType;

            Instigator = action.Unit;

            if(map.Contains(target))
            {
                HasTarget = true;
                Target = map.Get(target);
            }

            EvaluateCameraMovement();
        }

        private void PlayerCancelledAction()
        {
            ActionCancelled = true;
        }

        private void EvaluateCameraMovement()
        {
            if(SelectedAction != ActionType.Movement && !ActionCancelled && HasTarget) 
            {
                StartCoroutine(ApplyRandomScreenShakeFromPool());
                DynamicCameraTracker.transform.position = SelectedUnit.transform.position;
                StartCoroutine(ToggleDynamicCamera());
            }

            ActionCancelled = false;
            HasTarget = false;
        }

        private IEnumerator ApplyRandomScreenShakeFromPool()
        {
            yield return new WaitForSeconds(ShakeDelay);
            ScreenShakes[Random.Range(0, ScreenShakes.Length)].GenerateImpulse();
        }

        public void AdjustCameraManual(Unit unit)
        {
            DynamicCameraTracker.transform.position = unit.transform.position;
        }

        public void ApplyShakeManual(CinemachineImpulseSource source)
        {
            source.GenerateImpulse();
        }

        public void ToggleDynamicCameraManual()
        {
            DynamicCamera.gameObject.SetActive(!DynamicCamera.gameObject.activeInHierarchy);
        }

        private IEnumerator ToggleDynamicCamera()
        {
            DynamicCamera.gameObject.SetActive(true);

            cameraZoomSFX.PlayAudio();
            
            yield return new WaitForSeconds(DynamicCameraLifetime);

            DynamicCamera.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        public void TestShake()
        {
            StartCoroutine(ApplyRandomScreenShakeFromPool());
        }
#endif
    }
}