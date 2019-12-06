

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
///-Handles how the City Ruins behave-
///</summary>
public class CityRuins : DestroyableBlockers
{
    [SerializeField]
    private float HealAmount;

    [SerializeField]
    private UnitsMap map;

    public override void DestroyTerrain()
    {
        foreach (Point item in map.GetUnits())
        {
            if(map.Get(item).GetUnitType() == Type.Player)
            {
                map.Get(item).Health.IncreaseHealth(HealAmount);
            }
        }

        base.DestroyTerrain();
    }
}
