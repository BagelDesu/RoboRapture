

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
    ///-Debug One shot to play anything.-
    ///</summary>
    public class DebugOneShot : OneShot
    {
        public override void PlayAudio()
        {
            AkSoundEngine.PostEvent($"Play_{AudioName}", this.gameObject);
        }

        public override void StopAudio()
        {
            AkSoundEngine.PostEvent($"Stop_{AudioName}", this.gameObject);
        }
    }
}
