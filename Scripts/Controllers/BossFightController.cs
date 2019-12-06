

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Edu.Vfs.RoboRapture.CameraSystem;
using Edu.Vfs.RoboRapture.EffectsSystem;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.Units.Actions.Enemy;
using Edu.Vfs.RoboRapture.Units.Enemies;
using UnityEngine;


namespace Edu.Vfs.RoboRapture.GrandFinale
{    
    ///<summary>
    ///-Controlls the Boss Fight's Mid Battle sequence-
    ///</summary>
    public class BossFightController : MonoBehaviour
    {
        [SerializeField]
        private CameraController camCom;

        [SerializeField]
        private ParticleSystem BloodRain;

        [SerializeField]
        private CinemachineImpulseSource NDeathMarchShake;

        [SerializeField]
        private CinemachineImpulseSource NSatanSpawnShake;

        [SerializeField]
        private CinemachineImpulseSource NPukeShake;

        [SerializeField]
        private SpawnManager SManager;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private BossEndingSequence endingSequence;
        
        private Unit NeoSatan;

        private bool HasBloodRainIncreased = false;

        private bool HasSummonedWeakSatan = false;

        private void OnEnable()
        {
            NeoSatanLegDeathMarchAction.LegStomping += DeathMarchShake;
            NeoSatanBehaviour.RoarAttack += OnRoarAttack;
            NeoSatanBehaviour.PukeAttack += OnPukeAttack;
            NeoSatanBehaviour.SatanDamaged += OnNeoSatanDamaged;
        }

        private void OnDisable()
        {
            NeoSatanLegDeathMarchAction.LegStomping -= DeathMarchShake;
            NeoSatanBehaviour.RoarAttack -= OnRoarAttack;
            NeoSatanBehaviour.PukeAttack -= OnPukeAttack;
            NeoSatanBehaviour.SatanDamaged -= OnNeoSatanDamaged;
        }

        private void OnRoarAttack()
        {
            camCom.AdjustCameraManual(NeoSatan);
            camCom.ToggleDynamicCameraManual();
            StartCoroutine(AbberationDelay());
        }
        
        private IEnumerator AbberationDelay()
        {
            yield return new WaitForSeconds(.7f);
            
            EffectsPostProcessingController.Instance.ApplyEffect(EffectType.ChromaticAbberationFlash, .7f, .2f, 2);
            camCom.ApplyShakeManual(NSatanSpawnShake);
            camCom.ToggleDynamicCameraManual();

        }

        private void DeathMarchShake()
        {
            StartCoroutine(DelayedMarchShake());
        }

        private void OnPukeAttack()
        {
            camCom.AdjustCameraManual(NeoSatan);
            camCom.ToggleDynamicCameraManual();
            StartCoroutine(DelayedPukeShake());
        }

        private IEnumerator DelayedPukeShake()
        {
            yield return new WaitForSeconds(.3f);
            camCom.ApplyShakeManual(NPukeShake);
            camCom.ToggleDynamicCameraManual();
        }

        private IEnumerator DelayedMarchShake()
        {
            yield return new WaitForSeconds(.2f);
            camCom.ApplyShakeManual(NDeathMarchShake);
        }

        private void OnNeoSatanDamaged()
        {
            if(NeoSatan == null)
            {
                return;
            }

            Health NeoSatanHealth = NeoSatan.Health;

            if(NeoSatanHealth == null)
            {
                return;
            }

            if(NeoSatanHealth.Data.Value <= NeoSatanHealth.Data.MaxValue / 2)
            {
                IncreaseBloodRain();
            }
            
            if(NeoSatanHealth.Data.Value <= 5f)
            {
                SManager.MaxWhelps = 0;
            }   

            if(NeoSatanHealth.Data.Value <= 0f && !HasSummonedWeakSatan)
            {

                foreach (var item in map.GetUnits(Type.Enemy))
                {
                    if(item.GetSpawnedUnitType() == UnitType.WHELP)
                    {
                        item.Health.ReduceHealth(item.Health.Data.MaxValue);
                    }
                }
                
                endingSequence.gameObject.SetActive(true);
                endingSequence.NeoSatan = this.NeoSatan;
                endingSequence.SpawnWeakenedNeoSatan();
                HasSummonedWeakSatan = true;
            }
        }

        private void IncreaseBloodRain()
        {
            if(!HasBloodRainIncreased)
            {
                var BRain = BloodRain.emission;
                BRain.rateOverTime = 25f;
            }
        }

        public void SetNeoSatanRef(Unit unit)
        {
            NeoSatan = unit;
        }
    }
}
