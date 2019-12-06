

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
using UnityEngine.Events;

///<summary>
///-summary of script here-
///</summary>
public class OnGameSettingChanged : MonoBehaviour
{
    [System.Serializable]
    private class OnGammaChangedEvent : UnityEvent<float>
    {
    }

    [SerializeField]
    private GameSettings settings;

    public UnityEvent _OnVolumeChanged;
    [SerializeField]
    private OnGammaChangedEvent _OnGammaChanged;

    public UnityEvent _OnMuteStateChanged;

    private void OnEnable()
    {
        settings.OnGammaChanged += OnGammaChanged;
        settings.OnVolumeChanged += OnVolumeChanged;
        settings.OnSoundMute += OnMuteStatusChanged;
    }

    private void OnDisable()
    {
        settings.OnGammaChanged -= OnGammaChanged;
        settings.OnVolumeChanged -= OnVolumeChanged;
        settings.OnSoundMute -= OnMuteStatusChanged;
    }

    private void OnVolumeChanged()
    {
        _OnVolumeChanged.Invoke();

    }

    private void OnGammaChanged()
    {
        _OnGammaChanged.Invoke(settings.Gamma);
    }

    private void OnMuteStatusChanged()
    {
        _OnMuteStateChanged.Invoke();
    }
}
