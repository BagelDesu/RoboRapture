

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.TileAuxillary;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public abstract class ATileDecoration : ScriptableObject, ITileDecoration
{
    public abstract Texture GetStateTexture(TileStates state);
    public abstract void ApplyMaterial(TileMaterials state, List<Material> appliedMaterials);    
}
