

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.TurnSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Edu.Vfs.RoboRapture.UI
{    
    ///<summary>
    ///-Handles the tracking of the player's progress via turns passed-
    ///</summary>
    public class LevelProgressIndicator : MonoBehaviour
    {
        [SerializeField]
        private EntityTurnManager entityTurnManager;
        [SerializeField]
        private BoardController boardController;

        private Slider progressBar;

        [SerializeField]
        private int TotalTurns;

        private void Start()
        {
            progressBar = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            entityTurnManager.OnTurnEnd += UpdateProgress;            
        }

        private void OnDisable()
        {
            entityTurnManager.OnTurnEnd -= UpdateProgress;            
        }

        private void UpdateProgress(TurnEntities entity)
        {
            if(entity == TurnEntities.PLAYER)
            {
                TotalTurns++;
                if(boardController.BuilderProp.GeneratedBoard.Count <= 1)
                {
                    progressBar.value = 1;
                    return;
                }

                progressBar.value = (float)TotalTurns / (((float)boardController.BuilderProp.GeneratedBoard.Count - 1 )* 8);
            }

            // Debug.Log($"[ETM] Progress: {(float)TotalTurns / ((float)boardController.BuilderProp.GeneratedBoard.Count - 1) * 8} | {(float)TotalTurns}/{((float)boardController.BuilderProp.GeneratedBoard.Count - 1) * 8}");
        }
    }
}
