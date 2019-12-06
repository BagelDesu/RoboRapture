

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.EffectsSystem;
using Edu.Vfs.RoboRapture.Environment;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.TurnSystem;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class TesterASwell : MonoBehaviour
{
    [SerializeField]
    private UnitsMap map;

    [SerializeField]
    private CameraTracker tracker;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {

            tracker.PerformEndroll();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Unit item in map.GetUnits(Type.Enemy))
            {
                if(item.GetSpawnedUnitType() == UnitType.NEOSATAN_HEAD)
                {
                    item.Health.ReduceHealth(item.Health.Data.MaxValue - 1);
                }
            }
        }
    }
}