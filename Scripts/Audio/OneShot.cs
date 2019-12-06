

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.AudioSystem
{    
    ///<summary>
    ///-Base Class for OneShot Audio Cues-
    /// 
    /// Our Definition of a "OneShot"
    /// Audio that does not need a dynamic variable to be changed.
    ///</summary>
    public abstract class OneShot : MonoBehaviour
    {
        [SerializeField]
        protected string AudioName;

        public abstract void PlayAudio();
        public abstract void StopAudio();
    }
}

