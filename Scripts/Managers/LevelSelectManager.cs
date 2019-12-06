

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Environment;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.Managers
{

    [System.Serializable]
    public struct MapDataToBiomeType
    {
        public BiomeType type;
        public MapData data;
    }
    ///<summary>
    ///-Changes the pre-loaded levels-
    ///</summary>
    public class LevelSelectManager : MonoBehaviour
    {
        [SerializeField]
        private RefMapData data;

        [SerializeField]
        private MapDataToBiomeType[] Maps;

        private Dictionary<BiomeType, MapData> MapDataDictionary = new Dictionary<BiomeType, MapData>();

        private void Awake()
        {
            foreach (MapDataToBiomeType item in Maps)
            {
                MapDataDictionary.Add(item.type, item.data);
            }
        }

        public void SwitchPreLoadedMap(int type)
        {
            data.mapData = MapDataDictionary[(BiomeType)type];
        }
    }
}

