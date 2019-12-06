

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Edu.Vfs.RoboRapture.AudioSystem;
using Edu.Vfs.RoboRapture.CameraSystem;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.EffectsSystem;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.Knockbacks;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.TurnSystem;
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.WeatherSystem;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Edu.Vfs.RoboRapture.GrandFinale
{    

    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class NeoSatanSpawner : MonoBehaviour
    {

        [SerializeField]
        private EntityTurnManager entityTurnManager;

        [SerializeField]
        private BoardController controller;

        [SerializeField]
        private CameraController camCom;

        [SerializeField]
        private WeatherManager weather;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private Unit NeoSatan;

        [SerializeField]
        private int SpawnSatanAtTurn;

        [SerializeField]
        private Point NeoSatanSpawnPosition;
        
        [SerializeField]
        private CinemachineImpulseSource NSatanSpawnShake;

        [SerializeField]
        private ParticleSystem EntryFog;

        [SerializeField]
        private GameObject SoulSphere;

        [SerializeField]
        private GameObject SoulSphereDisplay;

        [SerializeField]
        private GameObject MainHUD;

        [SerializeField]
        private GameObject BossFightController;

        [SerializeField]
        private DebugOneShot NeoSatanRoar;

        [SerializeField]
        private bool DisableHUD;

        // --- HIDDEN VALUES --- 

        private int CurrentTurns;

        private bool ShouldStartCountdown = false;

        private Board raptureReference;

        private KnockbackHandler handler;

        private Unit NeoSatanReference;

        public static event Action OnNeoSatanSpawn;


        private void OnEnable()
        {
            BoardController.OnBoardSwitch += SpawnSatan;
            entityTurnManager.OnTurnEnd += IncreaseTurnCounter;
        }

        private void OnDisable()
        {
            BoardController.OnBoardSwitch -= SpawnSatan;
            entityTurnManager.OnTurnEnd -= IncreaseTurnCounter;

        }

        private void Start()
        {
            handler = new KnockbackHandler(map);   
        }

        private void SpawnSatan(Board board)
        {
            if(board.Biome == Environment.BiomeType.RAPTURE)
            {
                EntryFog.enableEmission = true;

                // ADJUST THE SPAWN POSITION TO BE OFFSETED.
                NeoSatanSpawnPosition = new Point(NeoSatanSpawnPosition.x + (board.BoardDimensions.x * board.BoardOffset), 0, NeoSatanSpawnPosition.z);

                ShouldStartCountdown = true;
                raptureReference = board;

                StartCoroutine(CountdownToSatan());
            }
        }

        private void ActivateSoulSphere()
        {
            // ACTIVEATE THE SOUL SPHERE FOR BACKGROUND IMAPCT
            SoulSphere.SetActive(true);
            SoulSphereDisplay.SetActive(true);
        }

        private IEnumerator CountdownToSatan()
        {
            // Await for for the proper turn.
            while (CurrentTurns < SpawnSatanAtTurn)
            {
                yield return null;
            }

            StartCoroutine(SpawnNeoSatan());

            // COUNTDOWN NEO-SATAN'S SCREAM
            StartCoroutine(StartScream());
        }

        private IEnumerator SpawnNeoSatan()
        {
            BossFightController.SetActive(true);

            if(DisableHUD)
            {
                MainHUD.SetActive(false);
            }
            
            // APPLY KNOCKBACK TO NEOSATAN SPAWN AREA
            foreach (Point item in LoadSpawnArea())
            {    
                handler.Execute(controller, NeoSatanSpawnPosition, item, 1);
            }

            handler.Execute(controller, new Point( this.NeoSatanSpawnPosition.x - 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z), NeoSatanSpawnPosition, 1);

            do
            {
                yield return null;

            } while (map.Contains(NeoSatanSpawnPosition));
            ActivateSoulSphere();
            // SPAWN NEO-SATAN
            NeoSatanReference = AIPlacementHelper.AddUnit(null, NeoSatanSpawnPosition, NeoSatan);
            BossFightController.GetComponent<BossFightController>().SetNeoSatanRef(NeoSatanReference);
            OnNeoSatanSpawn?.Invoke();
        }

        private IEnumerator StartScream()
        {
            yield return new WaitForSeconds(1f);
            camCom.AdjustCameraManual(NeoSatanReference);
            camCom.ToggleDynamicCameraManual();
            StartCoroutine(ScreamDelay());
        }

        private IEnumerator ScreamDelay()
        {
            yield return new WaitForSeconds(1f);
            NeoSatanReference.GetComponentInChildren<NeoSatanAnimationCaller>().Scream();
            StartCoroutine(ShakeDelay());
            StartCoroutine(DelayedCameraReset(2f));
            weather.ManualBoardSwap(raptureReference);
        }

        private IEnumerator ShakeDelay()
        {
            yield return new WaitForSeconds(0.7f);
            EffectsPostProcessingController.Instance.ApplyEffect(EffectType.ChromaticAbberationFlash, .7f, .2f, 2);
            NeoSatanRoar.PlayAudio();
            camCom.ApplyShakeManual(NSatanSpawnShake);

            MusicController.Instance?.UpdateIntensityManual(MusicIntensity.High);
            
            // Remove the covering fog...
            var vel = EntryFog.velocityOverLifetime;
            vel.yMultiplier = 10f;
            vel.orbitalZ = 10f;
            StartCoroutine(FogDisappearance());
            MainHUD.SetActive(true);
        }

        private IEnumerator DelayedCameraReset(float amount)
        {
            yield return new WaitForSeconds(amount);
            camCom.ToggleDynamicCameraManual();  
        }

        private IEnumerator FogDisappearance()
        {
            yield return new WaitForSeconds(0.7f);
            EntryFog.enableEmission = false;
        }

        private List<Point> LoadSpawnArea()
        {
            List<Point> attackArea = new List<Point>();

            attackArea.Add(new Point(this.NeoSatanSpawnPosition.x + 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z));
            attackArea.Add(new Point( this.NeoSatanSpawnPosition.x - 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z));
            attackArea.Add(new Point(this.NeoSatanSpawnPosition.x, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z + 1 ));
            attackArea.Add(new Point( this.NeoSatanSpawnPosition.x, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z - 1));


            attackArea.Add(new Point(this.NeoSatanSpawnPosition.x + 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z + 1));
            attackArea.Add(new Point(this.NeoSatanSpawnPosition.x + 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z - 1));

            attackArea.Add(new Point( this.NeoSatanSpawnPosition.x - 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z + 1));
            attackArea.Add(new Point(this.NeoSatanSpawnPosition.x - 1, this.NeoSatanSpawnPosition.y ,this.NeoSatanSpawnPosition.z - 1 ));
            
            return attackArea;
        }

        private void IncreaseTurnCounter(TurnEntities entity)
        {
            if(entity == TurnEntities.PLAYER && ShouldStartCountdown)
            {
                CurrentTurns++;
            }
        }
    }
}