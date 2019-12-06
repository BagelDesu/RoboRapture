

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.AudioSystem;
using UnityEngine;

///<summary>
///-Behaviour of Destroyable Blockers.-
///</summary>
public class DestroyableBlockers : EnviromentalUnit
{
    [SerializeField]
    private OneShot OnDeathSound;
    [SerializeField]
    private DestructibleMesh crackedMesh;

    public void CheckIfDestroyable()
    {
        if(Health.IsDead())
        {
            DestroyTerrain();
        }
    }
    
    public virtual void DestroyTerrain()
    {
        crackedMesh?.SplitMesh();
        OnDeathSound?.PlayAudio();
        UnitsMap.Remove(this.GetPosition());
        EnvironmentManager.EnvironmentCollection.Remove(WorldPosition);
        gameObject.SetActive(false);
    }
}
