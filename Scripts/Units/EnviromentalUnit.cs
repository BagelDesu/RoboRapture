

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.TileAuxillary;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class EnviromentalUnit : Unit
{

    // TODO: Carve the Shader aspects of the Envrionmental units out of this class. It really doesn't need to be here.

    public MeshRenderer MatRenderer       {get; private set;}
    
    [HideInInspector]
    public Point WorldPosition;

    public string Description;
    
    private new void Awake()
    {
        base.Awake();
    }

    // --- HELPERS ---

    // Point transforming
    public void SetUpPosition(int boardOffset, int boardWidth)
    {
        WorldPosition = new Point(base.GetPosition().x + (boardWidth * boardOffset), 0, base.GetPosition().z);
        SetPosition(WorldPosition);
    }

    public override Point GetPosition()
    {
        return base.GetPosition();
    }

    public override void SetPosition(Point position)
    {
        base.SetPosition(position);
    }
}
