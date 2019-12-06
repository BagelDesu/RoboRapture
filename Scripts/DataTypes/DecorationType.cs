

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.TileAuxillary;

///<summary>
///-summary of script here-
///</summary>

[System.Serializable]
public struct DecorationType
{
    [SerializeField]
    public TileStates state;
    [SerializeField]
    public ATileDecoration decoration;
}
