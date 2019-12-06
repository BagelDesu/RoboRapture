

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
    ///-Handles the fading in and out of the multiple screens-
    ///</summary>
    public class CreditsSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Animator[] Canvases = default;

        private int CurrentCanvas;

        private void OnEnable()
        {
            CameraTracker.OnPanFinished += StartFade;
        }

        private void OnDisable()
        {
            CameraTracker.OnPanFinished -= StartFade;
        }

        public void StartFade()
        {
            Canvases[CurrentCanvas].SetTrigger("FadeIn");
            StartFOut();
            CurrentCanvas++;
        }

        public void StartFOut()
        {
            if(CurrentCanvas <= 0)
            {
                return;
            }

            Canvases[CurrentCanvas-1].SetTrigger("FadeOut");
        }

        public void ResetLastPanel()
        {
            Canvases[CurrentCanvas-1].SetTrigger("FadeOut");

            foreach (var item in Canvases)
            {
                item.SetTrigger("Reset");   
            }
            
            CurrentCanvas = 0;
        }
    }
}
