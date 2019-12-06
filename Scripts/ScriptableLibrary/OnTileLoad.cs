

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

///<summary>
///-summary of script here-
///</summary>

namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{
    public class OnTileLoad : MonoBehaviour
    {
        [SerializeField]
        private RefInt refInt;

        [SerializeField]
        public RefInt targetAmount;

        [SerializeField]
        private UEvent_TileLoad TilesLoaded;

          /// <summary>
        /// Register the listeners to events.
        /// </summary>
        private void OnEnable()
        {
            this.refInt.Listeners += this.OnValueChanged;
        }

        /// <summary>
        /// Unregister the listeners from events.
        /// </summary>
        private void OnDisable()
        {
            this.refInt.Listeners -= this.OnValueChanged;
        }

        /// <summary>
        /// Event to invoke when a change occurs to the reference.
        /// </summary>
        private void OnValueChanged()
        {
            if(refInt.Value >= targetAmount.Value)
            {
              this.TilesLoaded.Invoke(this.refInt.Value);
            }
        }

        [System.Serializable]
        private class UEvent_TileLoad: UnityEvent<int>
        {

        }
    }

}
