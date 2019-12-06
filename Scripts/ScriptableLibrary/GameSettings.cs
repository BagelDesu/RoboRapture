

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>

[CreateAssetMenu(menuName="Game/Settings")]
public class GameSettings : ScriptableObject
{
    public event Action OnGammaChanged;
    public event Action OnVolumeChanged;
    public event Action OnSoundMute;

    public float Volume;
    public float Gamma;
    public bool IsMuted;

    public void UpdateVolume(float value)
    {
        Volume = value;
        OnVolumeChanged.Invoke();
    }

    public void UpdateGamma(float value)
    {
        Gamma = value;
        OnGammaChanged.Invoke();

    }

    public void UpdateMuteStatus(bool status)
    {
        IsMuted = status;
        OnSoundMute.Invoke();

    }
}
