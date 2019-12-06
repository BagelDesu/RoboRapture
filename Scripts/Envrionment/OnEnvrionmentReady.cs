/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class OnEnvrionmentReady : MonoBehaviour
    {
        [SerializeField]
        private RefInt LoadedBoards;

        [SerializeField]
        private RefBool HasGeneratedMap;

        [SerializeField]
        private RefInt TargetBoardAmount;

        [SerializeField]
        private UEvent_EnvironmentReady environmentReady;
        

        /// <summary>
        /// Register the listeners to events.
        /// </summary>
        private void OnEnable()
        {
            this.LoadedBoards.Listeners += this.OnValueChanged;
            HasGeneratedMap.Listeners += this.OnValueChanged;
        }

        /// <summary>
        /// Unregister the listeners from events.
        /// </summary>
        private void OnDisable()
        {
            this.LoadedBoards.Listeners -= this.OnValueChanged;
            HasGeneratedMap.Listeners -= this.OnValueChanged;
        }
        
        private void OnValueChanged()
        {
            if(LoadedBoards.Value == TargetBoardAmount.Value && HasGeneratedMap.Value == true )
            {
                environmentReady.Invoke();
            }
        }

        [System.Serializable]
        private class UEvent_EnvironmentReady: UnityEvent
        {

        }
    }
}