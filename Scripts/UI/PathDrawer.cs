

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Units.Actions;
using Edu.Vfs.RoboRapture.AudioSystem;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class PathDrawer : MonoBehaviour
{
    [SerializeField]
    private BoardController controller;

    List<Point> previousPath = new List<Point>();

    [SerializeField]
    private DebugOneShot TileHighlightAudio; 
    
    private void OnEnable()
    {
        NonFlyingMovementAction.MovementPath += DrawPath;
        MaximilianMovementAction.MovementPath += DrawPath;
        PlayerController.PlayerActionExecuted += ClearPath;
    }

    private void OnDisable()
    {
        NonFlyingMovementAction.MovementPath -= DrawPath;
        MaximilianMovementAction.MovementPath -= DrawPath;
        PlayerController.PlayerActionExecuted -= ClearPath;
    }

    public void DrawPath(List<Point> path)
    {
        ResetPath();

        previousPath = path;

        if(path.Count <= 0)
        {
            return;
        }

        TileHighlightAudio.PlayAudio();

        HashSet<Point> selectionPoint = new HashSet<Point>();
        selectionPoint.Add(path[path.Count - 1]);

        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(path), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.ActiveHighlight);
        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(selectionPoint), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.SelectionHighlight);
    }

    public void DrawPath(List<Point> path, Point destination)
    {
        ResetPath();

        previousPath = path;

        if(path.Count <= 0)
        {
            return;
        }

        TileHighlightAudio.PlayAudio();

        HashSet<Point> selectionPoint = new HashSet<Point>();
        selectionPoint.Add(destination);
        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(selectionPoint), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.SelectionHighlight);
    }

    public void ResetPath()
    {
        if(previousPath.Count <= 0)
        {
            return;
        }

        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(previousPath), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.BaseHighlight);
    }

    public void ClearPath()
    {
        if(previousPath.Count <= 0)
        {
            return;
        }

        controller.SwitchTilesFromActiveBoards(new HashSet<Point>(previousPath), Edu.Vfs.RoboRapture.TileAuxillary.TileStates.NORMAL);
        previousPath.Clear();
    }
}
