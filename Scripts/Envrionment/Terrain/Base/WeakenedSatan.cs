

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Helpers;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GrandFinale
{
    ///<summary>
    ///-Handles the weakened satan's Interactions-
    ///</summary>
    public class WeakenedSatan : DestroyableBlockers
    {
        public static event Action<int> OnHit;

        [SerializeField]
        private FXWrapper DestructionBits;

        public int Hits {get; private set;} = -1;

        [SerializeField]
        private Animator anim;
        
        public void OnWeakenedHit()
        {
            Hits++;

            if(Hits > 0)
            {
                OnHit.Invoke(Hits);
                anim.SetTrigger("Hit");
            }

            if(Hits == 3)
            {
                DestructionBits.Play(this.gameObject.transform.position);
            }
        }
    }
}
