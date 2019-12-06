

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.Units;

namespace Edu.Vfs.RoboRapture.TerrainSystem
{   

    ///<summary>
    /// The base class containing the base behaviour of terrains.
    /// 
    /// <remarks> this class requires/adds the Tile script onto the gameobject.</remarks>
    ///</summary>
    [RequireComponent(typeof(Tile))]
    public class RoboRaptureTerrain : MonoBehaviour
    {
        [SerializeField]
        public TerrainNavigationType NavigationType  = TerrainNavigationType.BOTH;
        [SerializeField]
        public TileType              TerrainEffect   = TileType.OPEN;

        /// <summary>
        ///  Returns the valid units that can pass/step on this terrain.
        /// </summary>
        /// <returns>TerrainNavigationType</returns>
        public TerrainNavigationType GetValidNavigationType()
        {
            return NavigationType;
        }

        /// <summary>
        ///  Returns the status effect that this terrain applies.
        /// </summary>
        /// <returns>StatusEffect</returns>
        public TileType GetTileType()
        {
            return TerrainEffect;
        }
    }
}

