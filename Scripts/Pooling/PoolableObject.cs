

/*
*      @Author: Carlos Miguel Aquino
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Note: PoolableObject.cs was created by Willy Campos as part of an assignment within VFS Unity 3 Class. We have received permission to use this script.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.ObjectPooling
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public abstract class PoolableObject : MonoBehaviour
    {
        [SerializeField]
        protected float LifeSpan = 3f;
        protected bool HasLifeSpan = false;

        protected virtual void OnEnable()
        {
            if(HasLifeSpan)
            {
                StartCoroutine(Disabler());
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Disabler()
        {
            yield return new WaitForSeconds(LifeSpan);
            gameObject.SetActive(false);
        }
    }
}