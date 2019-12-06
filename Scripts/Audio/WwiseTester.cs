

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///-summary of script here-
///</summary>
public class WwiseTester : MonoBehaviour
{

    private void Awake()
    {
        // AkSoundEngine.LoadBank("RoboRapture_SoundBank",);  
    }

    public void PlayClip()
    {
        AkSoundEngine.PostEvent("Play_Amb_Glacier", this.gameObject);
    }

    public void StopClip()
    {
        AkSoundEngine.PostEvent("Stop_Amb_Glacier", this.gameObject);
    }

    public void PlayMusic()
    {
        AkSoundEngine.PostEvent("Play_Mux_Glacier_Prototype", this.gameObject);
    }
}
