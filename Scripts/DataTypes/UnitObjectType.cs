

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections.Generic;
using Edu.Vfs.RoboRapture.SpawnSystem;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.DataTypes
{
    ///<summary>
    ///-summary of script here-
    ///</summary>
    [System.Serializable]
    public struct UnitObjectType 
    {
        public UnitType Type;
        public GameObject UnitPrefab;
    }
}
