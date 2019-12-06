

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.GridSystem;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.KillZoneSystem
{    
    ///<summary>
    ///- Marks a location as a killzone -
    ///</summary>
    public class KillZoneLocation : GridLockedEntity
    {
        public Board parent;

        private void Start()
        {
            parent = transform.parent.transform.parent.GetComponent<Board>();
        }

        private void OnEnable()
        {
            KillZones.AddKillZone(new Point(this.EntityPosition.x + parent.BoardDimensions.x * parent.BoardOffset, 0 , this.EntityPosition.z));
        }

        private void OnDisable()
        {            
            KillZones.AddKillZone(new Point(this.EntityPosition.x + parent.BoardDimensions.x * parent.BoardOffset, 0 , this.EntityPosition.z));
        }
    }
}