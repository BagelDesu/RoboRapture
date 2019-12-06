

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edu.Vfs.RoboRapture.AudioSystem
{
    public enum AmbienceTypes
    {
        GLACIER,
        BONEYARD,
        QUAGMIRE,
        RAPTURE
    }

    ///<summary>
    ///-Controlls the ambience for the game-
    ///</summary>
    public class AmbienceController : MonoBehaviour
    {
        [SerializeField]
        private AmbienceTypes StartAmbience;
        
        [SerializeField]
        private BoardController boardController;

        [SerializeField]
        private bool PlayOnStart;

        private AmbienceTypes CurrentPlaying;

        public bool IsPlaying {get; private set;} = false;

        private static AmbienceController instance;
        public static AmbienceController Instance {get => instance; private set => instance = value;}

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogWarning($"WARNING AMBIENCE CONTROLLER ALREADY EXISTS, DELETING FROM {this.name}.", this);
                Destroy(this);
                return;
            }

            instance = this;
            
            if(PlayOnStart)
            {
                PlayManual(StartAmbience);
            }

            DontDestroyOnLoad(this.gameObject);

            Debug.Log($"[ AMBIENCE ] Successfully created Music Controller.");

            SceneManager.sceneLoaded += RegisterOnNewScene;
        }
        //
        // Summary:
        //      
        //       This method is subscribed to Unity's sceneLoaded delegate from UnityEngine.SceneManager.
        //       This allows the script to Rereference references during a scene load.
        //
        private void RegisterOnNewScene(Scene scene, LoadSceneMode mode)
        {
            boardController = GameObject.FindObjectOfType<BoardController>();
        }

        private void OnEnable()
        {
            BoardController.OnBoardSwitch += UpdateAmbience;
        }

        private void OnDisable()
        {
            BoardController.OnBoardSwitch -= UpdateAmbience;            
        }

        private string ConvertAmbToString(AmbienceTypes type)
        {
            switch (type)
            {
                case AmbienceTypes.GLACIER:
                    return "Glacier";
                case AmbienceTypes.BONEYARD:
                    return "Boneyard";
                case AmbienceTypes.QUAGMIRE:
                    return "Quagmire";
                case AmbienceTypes.RAPTURE:
                    return "FinalBoss";
                default:
                    return "Null";
            }
        }

        private void UpdateAmbience(Board board)
        {
            switch (board.Biome)
            {
                case Environment.BiomeType.GLACIER:
                    PlayManual(AmbienceTypes.GLACIER);
                    break;
                case Environment.BiomeType.QUAGMIRE:
                    PlayManual(AmbienceTypes.QUAGMIRE);
                    break;
                case Environment.BiomeType.BONEYARD:
                    PlayManual(AmbienceTypes.BONEYARD);
                    break;
                case Environment.BiomeType.RAPTURE:
                    PlayManual(AmbienceTypes.RAPTURE);
                    break;
                default:
                    StopCurrentManual();
                    break;
            }
        }

        public void PlayManual(AmbienceTypes type)
        {
            StopCurrentManual();
            Debug.Log($"[ AMBIENCE ] Playing {type}");
            CurrentPlaying = type;
            IsPlaying = true;
            AkSoundEngine.PostEvent($"Play_Amb_{ConvertAmbToString(type)}", this.gameObject);
        }

        public void StopCurrentManual()
        {
            IsPlaying = false;
            AkSoundEngine.PostEvent($"Stop_Amb_{ConvertAmbToString(CurrentPlaying)}", this.gameObject);
        }
    }
}