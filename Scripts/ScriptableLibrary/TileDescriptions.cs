

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.TileAuxillary
{
    ///<summary>
    ///-Stores the description of tiles-
    ///</summary>
    [CreateAssetMenu(menuName="Map/Description")]
    public class TileDescriptions : ScriptableObject
    {
        public Sprite NormalImage;

        public string NormalDescription;

        public Sprite RaptureImage;

        public string RaptureDescription;

        public Sprite LightningImage;

        public string  LightningDescription;

        public Sprite LegStompImage;

        public string LegStompDescription;

        public Sprite PentagramImage;

        public string PentagramDescription;

        public Sprite SigilImage;

        public string SigilDescription;
    }
}
