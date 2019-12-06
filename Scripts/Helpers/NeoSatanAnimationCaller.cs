

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Edu.Vfs.RoboRapture.GrandFinale
{    
    ///<summary>
    ///-Manually calls the animations needed from neo-Satan-
    ///</summary>
    public class NeoSatanAnimationCaller : MonoBehaviour
    {

        private Animator anim;

        private void OnEnable()
        {
            anim = GetComponent<Animator>();
        }

        public void Scream()
        {
            Debug.Log("[NSS] SCREAMING");
            anim.SetTrigger("ManualScream");
        }
    }
}
