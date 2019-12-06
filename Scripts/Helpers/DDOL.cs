

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///-Marks gameobjects as DontDestroyOnLoad-
///</summary>
public class DDOL : MonoBehaviour
{
    [SerializeField] [Tooltip("Disables DontDestroyOnLoad and removes this gameObject when switching scenes. DO NOT DO THIS UNLESS DEBBUGING.")]
    private bool DisableDDOL = false;

    private void Awake()
    {
        if(DisableDDOL)
        {
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
