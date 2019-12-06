

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
    ///-Plays One Shots from the UI Menu Audio selection-
    ///</summary>
    public class UIMenuOneShot : OneShot
    {

        // Posts a play request
        public override void PlayAudio()
        {
            AkSoundEngine.PostEvent($"Play_UI_{AudioName}", this.gameObject);
        }

        // No need for a stop request.
        public override void StopAudio()
        {
            // AkSoundEngine.PostEvent($"UI_Menu_{AudioName}", this.gameObject);
        }
    }
}
