

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.WeatherSystem
{    
    ///<summary>
    ///-Stores the reference of the weather components-
    ///</summary>
    public class WeatherComponent : MonoBehaviour
    {
        public ParticleSystem[] WeatherParticles;
        public WindZone         WindZone;
        public ParticleSystem[] AmbienceParticle;
    }
}
