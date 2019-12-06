

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.TurnSystem
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public interface ITurnEntity
    {
        void StartTurn(EntityTurnManager turnManager);
        void EndTurn();
    }

}