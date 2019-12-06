

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
    ///-Plays / Stops SFXs-
    ///</summary>
    public class SFXOneShot : OneShot
    {
        public override void PlayAudio()
        {
            AkSoundEngine.PostEvent($"Play_{AudioName}_SFX",this.gameObject);
        }
        
        public override void StopAudio()
        {
            AkSoundEngine.PostEvent($"Stop_{AudioName}_SFX",this.gameObject);
        }
    }
}

