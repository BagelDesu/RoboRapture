

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Edu.Vfs.RoboRapture.WeatherSystem;

///<summary>
///-Settings Manager is a singleton that controlls all of the setting value changes-
///</summary>
public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume activeVolume;
    private ColorGrading colorGrading;

    [SerializeField]
    private GameSettings settings;

    private static SettingsManager instance;
    public static SettingsManager Instance {get; private set;}

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning($"WARNING SETTINGS MANAGER ALREADY EXISTS, DELETING FROM {this.name}.", this);
            Destroy(this);
            return;
        }

        instance = this;
        
        SceneManager.sceneLoaded += RegisterOnNewScene;
        settings.OnGammaChanged += UpdateFromGameSettings;

        UpdatePostProcessingSettings();
        Debug.Log($"[ SETTINGS ]Successfully created Settings Manager.", this);
    }

    private void OnDisable()
    {
        WeatherManager.OnBiomeChanged -= SwitchActiveProfile;
    }

    private void UpdateFromGameSettings()
    {
        AdjustGamma( settings.Gamma );
    }
    //
    // Summary:
    //      
    //       This method is subscribed to Unity's sceneLoaded delegate from UnityEngine.SceneManager.
    //       This allows the script to Rereference references during a scene load.
    //
    private void RegisterOnNewScene(Scene scene, LoadSceneMode mode)
    {
        WeatherManager.OnBiomeChanged += SwitchActiveProfile;
    }
    //
    // Summary:
    //      Adjusts the gamma of the game.
    //
    //
    public void AdjustGamma(float amount)
    {
        activeVolume.profile.TryGetSettings(out colorGrading);
        if(colorGrading == null) return;
        colorGrading.gamma.value.Set(colorGrading.gamma.value.x, colorGrading.gamma.value.y, colorGrading.gamma.value.z, amount);
    }
    //
    // Summary:
    //      Update the current scene's settings to what's set in the game settings.
    //
    public void UpdatePostProcessingSettings()
    {
        AdjustGamma(settings.Gamma);
    }
    //
    // Summary:
    //      Update the active biome in order to adjust their settings.
    //
    public void SwitchActiveProfile(PostProcessVolume vol)
    {
        activeVolume = vol;
        UpdatePostProcessingSettings();
    }
    //
    // Summary:
    //      saves the settings to the local disk
    //
    private void OnApplicationQuit()
    {
        
    }
}
