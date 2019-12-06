

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{    
    ///<summary>
    ///- Stores the current active map-
    ///</summary>
    /// 
    [CreateAssetMenu(menuName = "Map/RefMapData")]
    public class RefMapData : ScriptableObject
    {
        public MapData mapData;
    }
}
