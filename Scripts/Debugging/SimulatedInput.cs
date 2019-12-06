

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Edu.Vfs.RoboRapture.Debbugging
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class SimulatedInput : MonoBehaviour
    {
        public UnityEvent OnEscapeKeyDown;
        public UnityEvent OnSKeyDown;
        public UnityEvent OnDKeyDown;
        public UnityEvent OnQKeyDown;
        public UnityEvent OnWKeyDown;
        public UnityEvent OnEKeyDown;


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscapeKeyDown.Invoke();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                OnSKeyDown.Invoke();
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                OnDKeyDown.Invoke();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnQKeyDown.Invoke();
            }
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnWKeyDown.Invoke();
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                OnEKeyDown.Invoke();
            }
        }
    }
}

