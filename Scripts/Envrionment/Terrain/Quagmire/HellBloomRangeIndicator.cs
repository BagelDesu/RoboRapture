

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.Units.Actions;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.Environment
{
    ///<summary>
    ///-Shows the range of the hellbloom's explosion on hover.-
    ///</summary>
    [RequireComponent(typeof(HellBloom))]
    public class HellBloomRangeIndicator : MonoBehaviour
    {
        private HellBloom hellBloom;

        private bool HasPlayerSelectedAction = false;

        private HashSet<DataTypes.Point> validP = new HashSet<DataTypes.Point>();


        private void Start()
        {
            hellBloom = GetComponent<HellBloom>();
            PlayerController.PlayerActionSelected += PlayerSelectedAction;
            PlayerController.PlayerActionExecuted += PlayerUnselectedAction;
            Selector.CancelledAction += PlayerUnselectedAction;
        }

        private void OnMouseEnter()
        {
            validP.Clear();
            
            foreach (DataTypes.Point item in hellBloom.attackArea)
            {
                if(item.x < hellBloom.controller.GetMinimumVisibleRow() || item.x > hellBloom.controller.GetMaximumVisibleRow() - 1)
                {
                    continue;
                }

                validP.Add(item);
            }

            if(!HasPlayerSelectedAction)
            {
                hellBloom.controller.SwitchTilesFromActiveBoards(validP, TileAuxillary.TileStates.ActiveAttack);
            }
        }

        private void OnMouseExit()
        {
            if(!HasPlayerSelectedAction && validP.Count > 0)
            {
                hellBloom.controller.SwitchTilesFromActiveBoards(validP, TileAuxillary.TileStates.NORMAL);                
            }
        }

        private void PlayerSelectedAction(ActionType type)
        {
            HasPlayerSelectedAction = true;
        }

        private void PlayerUnselectedAction()
        {
            HasPlayerSelectedAction = false;
        }
    }
}
