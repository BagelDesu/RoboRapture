/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.AudioSystem;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Knockbacks;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
///-Controlls the Behaviour of the HellBlooms-
///</summary>
public class HellBloom : DestroyableBlockers
{

    [SerializeField][Tooltip("Damage dealt to the area around the unit")]
    private float Damage = 1f;

    [SerializeField][Tooltip("How long does the hellbloom display the attack highlight for?")]
    private float HighlightWaitTime = 1f;

    [SerializeField]
    private Animator animator;

    // TODO REMOVE BOARDCONTROLLER FROM HERE.
    public BoardController controller {get; private set;}

    private KnockbackHandler handler;

    public List<Point> attackArea {get; private set;}
    [SerializeField]
    private DebugOneShot SpawnSound;

    private void Start()
    {

        animator = GetComponentInChildren<Animator>();
        controller = GameObject.FindObjectOfType<BoardController>();
        handler = new KnockbackHandler(UnitsMap);
        attackArea = LoadAttackZones();
    }

    private void OnEnable()
    {
        SpawnSound.PlayAudio();
    }

    public override void DestroyTerrain()
    {
        if(Health.IsDead())
        {
            animator.SetTrigger("Explode");
            Explode();
        }
    }

    public void Explode()
    {
        
        List<Point> foundTargets = new List<Point>();

        // TODO: Query Units Map for all surrounding units. in the cardinal directions
        foreach (Point item in attackArea)
        {
            if(UnitsMap.Contains(item))
            {
                foundTargets.Add(item);
            }

            controller.SwitchTilesFromActiveBoards(new HashSet<Point>(attackArea), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.ATTACK);
        }
        
        // TODO: Apply target highlight to surrounding tiles and attack highlight.

        foreach (Point item in foundTargets)
        {
            controller.SwitchTilesFromActiveBoards(new HashSet<Point>(attackArea), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.TARGET);            
        }

        StartCoroutine(ApplyExplosion(foundTargets.ToArray(), attackArea.ToArray()));    
    }

    private IEnumerator ApplyExplosion(Point[] targets, Point[] attackArea)
    {
        yield return new WaitForSeconds(HighlightWaitTime); 

        foreach (Point item in targets)
        {
            if(UnitsMap.Contains(item))
            {
                Unit targetUnit = UnitsMap.Get(item);
                targetUnit.Health.ReduceHealth(Damage);
                handler.Execute(controller, this.GetPosition(), targetUnit.GetPosition(), 1);
            }
        }

        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(attackArea), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.NORMAL);

        EnvironmentManager.EnvironmentCollection.Remove(WorldPosition);
        UnitsMap.Remove(this.GetPosition());
        gameObject.SetActive(false);
    }

    private List<Point> LoadAttackZones()
    {
        List<Point> attackArea = new List<Point>();

        attackArea.Add(new Point(this.WorldPosition.x + 1, this.WorldPosition.y ,this.WorldPosition.z));
        attackArea.Add(new Point( this.WorldPosition.x - 1, this.WorldPosition.y ,this.WorldPosition.z));
        attackArea.Add(new Point(this.WorldPosition.x, this.WorldPosition.y ,this.WorldPosition.z + 1 ));
        attackArea.Add(new Point( this.WorldPosition.x, this.WorldPosition.y ,this.WorldPosition.z - 1));

        attackArea.Add(new Point(this.WorldPosition.x + 1, this.WorldPosition.y ,this.WorldPosition.z + 1));
        attackArea.Add(new Point(this.WorldPosition.x + 1, this.WorldPosition.y ,this.WorldPosition.z - 1));

        attackArea.Add(new Point( this.WorldPosition.x - 1, this.WorldPosition.y ,this.WorldPosition.z + 1));
        attackArea.Add(new Point(this.WorldPosition.x - 1, this.WorldPosition.y ,this.WorldPosition.z - 1 ));
        

        return attackArea;
    }
}
