

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class EntityParser : MonoBehaviour
{

    public EnemyUnit[] GetEnemyUnits()
    {
        return GetComponentsInChildren<EnemyUnit>();
    }

    public EnviromentalUnit[] GetEntities()
    {
        return GetComponentsInChildren<EnviromentalUnit>();
    }
}
