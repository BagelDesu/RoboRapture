

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Edu.Vfs.RoboRapture.AudioSystem;
using Edu.Vfs.RoboRapture.CameraSystem;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.EffectsSystem;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.WeatherSystem;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GrandFinale
{
    ///<summary>
    ///-Handles the post boss fight sequence-
    ///</summary>
    public class BossEndingSequence : MonoBehaviour
    {
        [SerializeField]
        private CameraController camCom;

        [SerializeField]
        private Unit WeakenedNeoSatan;

        [SerializeField]
        private UnitsMap map;
        
        [HideInInspector]
        public Unit NeoSatan;
        
        [SerializeField]
        private GameObject[] UiElements;

        [SerializeField]
        private ParticleSystem ArenaFog;

        [SerializeField]
        private ParticleSystem BloodRain;

        [SerializeField]
        private ParticleSystem ChunkRain;

        [SerializeField]
        private ParticleSystem DropParts;

        [SerializeField]
        private CinemachineImpulseSource NSatanHitShake;

        [SerializeField]
        private CinemachineImpulseSource NSatanFinalStrikeShake;

        [SerializeField]
        private GameObject SoulSphere;

        [SerializeField]
        private CameraTracker tracker;

        [SerializeField]
        private DebugOneShot NeoSatanRoar;

        [SerializeField]
        private DebugOneShot MiniSatanRoar;

        [SerializeField]
        private WeatherManager WManager;

        private Unit WeakenedNeoSatanRef;

        private Point neoSatanPosition;


        private void OnEnable()
        {
            WeakenedSatan.OnHit += NeoSatanHit;
        }

        private void OnDisable()
        {
            WeakenedSatan.OnHit += NeoSatanHit;
        }

        public void SpawnWeakenedNeoSatan()
        {
            MusicController.Instance.StopCurrentManual();
            AmbienceController.Instance.StopCurrentManual();
            
            SoulSphere.SetActive(false);
            // Flash white
            EffectsPostProcessingController.Instance.ApplyEffect(EffectType.ExposureFlash, 1.5f, 0.7f, 20f);
            NeoSatanRoar.PlayAudio();

            foreach (GameObject item in UiElements)
            {
                item.SetActive(false);
            }

            // Delete NeoSatan after flashing white
            neoSatanPosition = NeoSatan.GetPosition();
            RemoveNeoSatan();
            NeoSatan.GetComponentInChildren<AnimationStateUpdater>().Attack(NeoSatan.Health);

            // Spawn weakened version of neo-satan
            WeakenedNeoSatanRef = AIPlacementHelper.AddUnit(null, neoSatanPosition, WeakenedNeoSatan);
        }

        private void NeoSatanHit(int hits)
        {
            MiniSatanRoar.PlayAudio();
            camCom.ApplyShakeManual(NSatanHitShake);

            switch (hits)
            {
                case 1:
                    var Bemission = BloodRain.emission;
                    Bemission.enabled = false;
                    
                    var Demission = DropParts.emission;
                    Demission.enabled = false;
                    break;
                case 2:
                    MusicController.Instance.PlayManual(MusicTypes.CREDITS);
                    var Aemission = ArenaFog.emission;
                    Aemission.enabled = false;

                    var Cemission = ChunkRain.emission;
                    Cemission.enabled = false;

                    break;
                case 3:
                    // Delete weakened satan and pan up.
                    StartCoroutine(FinalShake());
                    break;
                default:
                break;
            }
        }

        private IEnumerator FinalShake()
        {
            
            camCom.ApplyShakeManual(NSatanFinalStrikeShake);
            WManager.ManualBoardSwap(Environment.BiomeType.GLACIER);
            yield return new WaitForSeconds(1.5f);
            
            tracker.PerformEndroll();
        }

        private void RemoveNeoSatan()
        {
            NeoSatan?.GetComponent<EnemyUnit>()?.NotifyDyingEvent();
            NeoSatan?.UnitsMap.Remove(NeoSatan.GetPosition());
            MonoBehaviour.Destroy(NeoSatan.gameObject);
        }
    }
}
