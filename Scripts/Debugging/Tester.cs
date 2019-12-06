

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
public class Tester : MonoBehaviour
{
    [SerializeField] private BoardController Controller;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            foreach (var item in Controller.GetAllPointsWithTerrainTypeOf(Edu.Vfs.RoboRapture.TerrainSystem.TileType.ICELAKE))
            {
                Debug.Log(item);
            }
        }
    }
}