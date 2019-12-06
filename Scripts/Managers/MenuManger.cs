

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
///-summary of script here-
///</summary>
public class MenuManger : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;

    public void ToggleMenu()
    {
        if(Menu.activeInHierarchy)
        {
            Menu.SetActive(false);
        }
        else
        {
            Menu.SetActive(true);
        }
    }
}
