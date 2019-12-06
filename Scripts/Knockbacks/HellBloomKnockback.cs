

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/
namespace Edu.Vfs.RoboRapture.Knockbacks
{
    using System.Collections;
    using System.Collections.Generic;
    using Edu.Vfs.RoboRapture.DataTypes;
    using Edu.Vfs.RoboRapture.Helpers;
    using Edu.Vfs.RoboRapture.Units;
    using Edu.Vfs.RoboRapture.Validators;
    using UnityEngine;

    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class HellBloomKnockback : MonoBehaviour, IKnockback
    {
        private HellBloom hellBloom;    

        public void Awake()
        {
            hellBloom = GetComponent<HellBloom>();
        }

        public bool Handle(Unit target)
        {
            hellBloom.Explode();
            return false;
        }
    }
}