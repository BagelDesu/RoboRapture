

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.DataTypes;

namespace Edu.Vfs.RoboRapture.GridSystem
{    
    /// <summary>
    /// Locks any script that derrive from this class to the grid.
    /// 
    /// 
    /// </summary>
    /// <see cref="Edu.Vfs.RoboRapture.DataTypes.Point"/>
    [ExecuteAlways]
    public abstract class GridLockedEntity : MonoBehaviour
    {
        [SerializeField] public Point EntityPosition = new Point(0,0,0);

        protected void OnDrawGizmos()
        {
            this.gameObject.transform.localPosition = new Vector3(EntityPosition.x, EntityPosition.y, EntityPosition.z);
        }
    }
}
