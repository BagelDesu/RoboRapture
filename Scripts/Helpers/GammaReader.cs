

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace  Edu.Vfs.RoboRapture.Settings
{  
    ///<summary>
    ///-Prints out the value of the gamma to a Text field-
    ///</summary>
    public class GammaReader : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI gammaReader;

        [SerializeField]
        private GameSettings settings;

        private void OnEnable()
        {
            settings.OnGammaChanged += UpdateValue;
        }

        private void OnDisable()
        {
            settings.OnGammaChanged -= UpdateValue;
        }

        private void UpdateValue()
        {
            gammaReader.text = settings.Gamma.ToString("F2");
        }
    }
}
