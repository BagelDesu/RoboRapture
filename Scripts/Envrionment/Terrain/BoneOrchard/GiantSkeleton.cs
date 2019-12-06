

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.SpawnSystem;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class GiantSkeleton : DestroyableBlockers
{

    private SpawnManager Spawner;
    [SerializeField]
    private UnitType SpawnedUnit;

    private void Start()
    {
        Spawner = SpawnManager.Instance;    
    }

    public override void DestroyTerrain()
    {
        if(Health.IsDead())
        {
            UnitsMap.Remove(GetPosition());
            Spawner.SpawnUnit(SpawnedUnit, this.WorldPosition);
            EnvironmentManager.EnvironmentCollection.Remove(WorldPosition);
            gameObject.SetActive(false);
        }
    }
}
