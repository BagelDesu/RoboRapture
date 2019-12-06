

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using Edu.Vfs.RoboRapture.Scriptables;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class ScriptobjectCacheCleaner : MonoBehaviour
{
    [SerializeField]
    private UnitsMap[] Units;

    [SerializeField]
    private RefInt[] Ints;

    [SerializeField]
    private RefFloat[] Floats;

    [SerializeField]
    private RefPoint[] Points;

    [SerializeField]
    private GameSettings GSettings;

    [SerializeField]
    private bool AtStartUp = true;

    [SerializeField]
    private RefMapData MapDatas;
 
    private void Awake()
    {
        if (AtStartUp)
        {
            CleanUp();
        }
    }

    public void CleanUp()
    {
        // Clear UnitsMap
        foreach (var item in Units)
        {
            item.Clear();
        }

        foreach (var item in Ints)
        {
            item.Value = 0;
        }

        foreach (var item in Floats)
        {
            item.Value = 0;
        }

        foreach (var item in Points)
        {
            item.Value = new Point(0, 0, 0);
        }

        GSettings.Gamma = 0;
        GSettings.IsMuted = false;
        GSettings.Volume = 0;

        MapDatas.mapData = null;
    }
}
