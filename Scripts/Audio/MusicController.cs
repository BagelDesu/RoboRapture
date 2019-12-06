

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.TurnSystem;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edu.Vfs.RoboRapture.AudioSystem
{

    public enum MusicIntensity
    {
        Low,
        High
    }
    
    public enum MusicTypes
    {
        MENU,
        GLACIER,
        BONEYARD,
        QUAGMIRE,
        RAPTURE,
        LOSE,
        CREDITS
    }

    ///<summary>
    ///-Controlls the music for the game.-
    ///</summary>
    public class MusicController : MonoBehaviour
    {
        [SerializeField]
        private MusicTypes StartMusic;

        [SerializeField]
        private EntityTurnManager turnManager;

        [SerializeField]
        private BoardController boardController;

        [SerializeField]
        private bool PlayOnStart;

        [SerializeField]
        private UnitsMap map;

        [SerializeField]
        private int[] enemyThresholds;

        private MusicTypes CurrentPlaying;

        private bool OverrideIntensity = false;

        public bool IsPlaying {get; private set;} = false;

        private static MusicController instance;
        public static MusicController Instance {get => instance; private set => instance = value;}

        private void Awake()
        {

            if(instance != null)
            {
                instance.PlayManual(StartMusic);
                Debug.LogWarning($"WARNING MUSIC CONTROLLER ALREADY EXISTS, DELETING FROM {this.name}.", this);
                Destroy(this);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);
            
            SceneManager.sceneLoaded += RegisterOnNewScene;
            
            Debug.Log($"[ MUSIC ] Successfully created Music Controller.");
        }

        private void Start()
        {
            AkSoundEngine.SetState("Turn", "Player");
            AkSoundEngine.SetSwitch("Music_Intention", "Low", this.gameObject);
      
            if(PlayOnStart)
            {
                PlayManual(StartMusic);
            }
        }

        //
        // Summary:
        //      
        //       This method is subscribed to Unity's sceneLoaded delegate from UnityEngine.SceneManager.
        //       This allows the script to Rereference references during a scene load.
        //
        private void RegisterOnNewScene(Scene scene, LoadSceneMode mode)
        {
            // if(IsPlaying)
            // {
            //     StopCurrentManual();
            // }

            OverrideIntensity = false;

            turnManager = GameObject.FindObjectOfType<EntityTurnManager>();
            boardController = GameObject.FindObjectOfType<BoardController>();

            if(turnManager != null && boardController != null)
            {
                turnManager.OnTurnEnd += UpdateTurnState;
            }
        }

        private void OnEnable()
        {
            SpawnManager.OnUnitSpawn += UpdateIntensityState;
            EnemyUnit.EnemyDied += UpdateIntensityState;
            BoardController.OnBoardSwitch += UpdateMusic;
        }

        private void OnDisable()
        {
            StopCurrentManual();
            SpawnManager.OnUnitSpawn -= UpdateIntensityState;
            BoardController.OnBoardSwitch -= UpdateMusic;
            EnemyUnit.EnemyDied -= UpdateIntensityState;
        }

        //TODO: do Enum.ToString() method instead.
        private string ConvertMuxToString(MusicTypes type)
        {
            switch (type)
            {
                case MusicTypes.MENU:
                    return "Menu";
                case MusicTypes.GLACIER:
                    return "Glacier";
                case MusicTypes.BONEYARD:
                    return "Boneyard";
                case MusicTypes.QUAGMIRE:
                    return "Quagmire";
                case MusicTypes.RAPTURE:
                    return "Rapture";
                case MusicTypes.LOSE:
                    return "LoseScreen";
                case MusicTypes.CREDITS:
                    return "Credit";
                default:
                    return "Null";
            }
        }

        private string ConverTurnToString(TurnEntities turn)
        {
            switch (turn)
            {
                case TurnEntities.ENEMY:
                    return "Enemy";
                case TurnEntities.PLAYER:
                    return "Player";
                default:
                    return "None";
            }
        }

        private void UpdateMusic(Board board)
        {
            switch (board.Biome)
            {
                case Environment.BiomeType.GLACIER:
                    PlayManual(MusicTypes.GLACIER);
                    break;
                case Environment.BiomeType.QUAGMIRE:
                    PlayManual(MusicTypes.QUAGMIRE);
                    break;
                case Environment.BiomeType.BONEYARD:
                    PlayManual(MusicTypes.BONEYARD);
                    break;
                case Environment.BiomeType.RAPTURE:
                    PlayManual(MusicTypes.RAPTURE);
                    break;
                default:
                    StopCurrentManual();
                    break;
            }
        }

        public void PlayManual(MusicTypes type)
        {
            if (IsPlaying)
            {
                StopCurrentManual();
            }

            if(type == MusicTypes.RAPTURE)
            {
                UpdateIntensityManual(MusicIntensity.Low);
            }

            Debug.Log($"[ MUSIC ] Playing {ConvertMuxToString(type)}");
            CurrentPlaying = type;

            AkSoundEngine.PostEvent($"Play_Mux_{ConvertMuxToString(type)}", this.gameObject);
            IsPlaying = true;
        }

        public void StopCurrentManual()
        {
            Debug.Log("Stoping Current Mux");
            IsPlaying = false;
            AkSoundEngine.PostEvent($"Stop_Mux_{ConvertMuxToString(CurrentPlaying)}", this.gameObject);
        }

        public void ToggleCurrentManual()
        {
            if(IsPlaying)
            {
                StopCurrentManual();
            }
            else
            {
                PlayManual(CurrentPlaying);
            }
        }

        public void UpdateRTPC(float value)
        {
            AkSoundEngine.SetRTPCValue("Board_Number", value, this.gameObject);
        }

        public void UpdateTurnState(TurnEntities turn)
        {
            if(turn == TurnEntities.ENVIRONMENT){ return; }
            AkSoundEngine.SetState("Turn", ConverTurnToString(turn));
        }

        public void UpdateIntensityManual(MusicIntensity intensity)
        {
            OverrideIntensity = true;
            AkSoundEngine.SetSwitch("Music_Intention", $"{intensity.ToString()}", this.gameObject);
            
        }

        public void UpdateIntensityState()
        {
            if(OverrideIntensity)
            {
                return;
            }
            int activeUnits = 0;
            foreach (Unit item in map?.GetUnits(Type.Enemy))
            {
                if(item.isActiveAndEnabled)
                {
                    activeUnits++;
                }
            }
            
            if(activeUnits >= enemyThresholds[2])
            {
                AkSoundEngine.SetSwitch("Music_Intention", "High", this.gameObject);
            }
            else if(activeUnits >= enemyThresholds[1])
            {
                AkSoundEngine.SetSwitch("Music_Intention", "Mid", this.gameObject);
            }
            else if(activeUnits >= enemyThresholds[0])
            {            
                AkSoundEngine.SetSwitch("Music_Intention", "Low", this.gameObject);
            }
        }

        public void UpdateIntensityState(Point point)
        {
            if(OverrideIntensity)
            {
                return;
            }
            int activeUnits = 0;
            foreach (Unit item in map?.GetUnits(Type.Enemy))
            {
                if(item.isActiveAndEnabled)
                {
                    activeUnits++;
                }
            }

            if(activeUnits >= enemyThresholds[2])
            {
                AkSoundEngine.SetSwitch("Music_Intention", "High", this.gameObject);
            }
            else if(activeUnits >= enemyThresholds[1])
            {
                AkSoundEngine.SetSwitch("Music_Intention", "Mid", this.gameObject);
            }
            else if(activeUnits >= enemyThresholds[0])
            {            
                AkSoundEngine.SetSwitch("Music_Intention", "Low", this.gameObject);
            }
        }
    }
}

