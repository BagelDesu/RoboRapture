

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.TurnSystem;
using Edu.Vfs.RoboRapture.Controllers;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.TerrainSystem;
using Edu.Vfs.RoboRapture.TileAuxillary;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using UnityEngine.Events;
using System;
using Edu.Vfs.RoboRapture.WeatherSystem;

///<summary>
/// Handles the Movement and Building of the Map.
/// Defines which Board is currently active.
/// 
/// Definition:
///     Map - Composed of Boards
///     Boards - Composed of Tiles, Coordinates Tile's joined behaviours
///     Tiles - Individual blocks that are displayed. 
public class BoardController : MonoBehaviour, IController, ITurnEntity
///</summary>
{
    [SerializeField]
    private MapBuilder          Builder;
    public  MapBuilder          BuilderProp   {get => Builder; private set => Builder = value;}
    public EnvironmentManager   EnvManager    {get; private set;}

    [SerializeField]
    private WeatherManager      WManager;
    
    public  Board               CurrentBoard  {get; private set;}
    public  Board               PreviousBoard {get; private set;}

    private EntityTurnManager   TurnManager;
    [SerializeField]
    private float               EndDelay;

    [SerializeField]
    private int                 StartingTileRows  = 5;
    private int                 CurrentBoardShown = 0;
    public  int                 CurrentRowShown   {get; private set;}

    [SerializeField]
    private RefInt              TileSetupData;

    [SerializeField]
    private RefInt                     RapturedRow;

    public UnityEvent                  OnRowDestroy;
    public UnityEvent                  OnRowCreate;
    public UnityEvent                  OnTurnStart;

    public static event Action<Board>    OnBoardSwitch;
    public event Action<int>             OnBoardLoad;
    

    private void Awake()
    {
        RapturedRow.Value = -1;
        // Registering this script to the Turn Manager.
        EntityTurnManager.RegisterEntity(this, TurnEntities.ENVIRONMENT);
        EnvManager = GetComponent<EnvironmentManager>();

    }

    private void OnDisable()
    {
        TileSetupData.Value = 0;
        RapturedRow.Value = -1;
    }

    private void Start()
    {
        if(Builder == null )
        {
            Debug.LogError("Missing Map Builder Reference");
            return;
        }
        
        foreach (Board board in Builder.GeneratedBoard)
        {
            board.gameObject.SetActive(false);
        }
        
        Builder.GeneratedBoard[CurrentBoardShown].gameObject.SetActive(true);
        CurrentBoard = Builder.GeneratedBoard[CurrentBoardShown];

        WManager.PerformInstantSwap(CurrentBoard);

        foreach (Board item in Builder.GeneratedBoard)
        {
            EnvManager.InitializeUnits(item);
        }

        for (int i = 0; i < StartingTileRows; i++)
        {
            CurrentBoard.ShiftVisibleArea(i);
            EnvManager.RaiseEntities(i + (CurrentBoard.BoardDimensions.x * CurrentBoard.BoardOffset));
        }
 
        for (int i = 0; i < CurrentBoard.BoardDimensions.z; i++)
        {
            CurrentBoard.LocalTileCollection[new Point(0, 0, i)].Decorator.ApplyMaterialState(TileMaterials.SINK);
        }
        
        CurrentRowShown = StartingTileRows;
        Board.OnRowSink += BridgeMethod;
    }
    
    private void BridgeMethod(List<Point> points)
    {
        OnRowDestroy.Invoke();
    }
    //
    //Summary:
    //  Returns a point on the board offseted by the board's offset.
    //
    private Point CreatePointWithOffset(Tile tile, Board board)
    {
        return new Point(tile.EntityPosition.x + (board.BoardDimensions.x * board.BoardOffset), tile.EntityPosition.y, tile.EntityPosition.z);
    }
    //
    //Summary:
    //  Shift's the CurrentBoard's VisibleArea. Calls the "ShiftVisibleArea()" method inside of the CurrentBoard.
    //
    //
    private bool ShiftBoardRow()
    {

        if(CurrentRowShown >= CurrentBoard.BoardDimensions.x)
        {
            return false;
        }

        OnRowCreate?.Invoke();
        CurrentBoard.ShiftVisibleArea(CurrentRowShown);
        EnvManager.RaiseEntities(CurrentRowShown + (CurrentBoard.BoardDimensions.x * CurrentBoard.BoardOffset));
        PreviousBoard?.SinkVisibleArea(CurrentRowShown);
        EnvManager.SinkEntities(CurrentRowShown + (PreviousBoard.BoardDimensions.x * PreviousBoard.BoardOffset));

        CurrentRowShown++;

        RapturedRow.Value++;

        if(CurrentRowShown == CurrentBoard.BoardDimensions.x )
        {
            for (int i = 0; i < CurrentBoard.BoardDimensions.z; i++)
            {
                CurrentBoard.LocalTileCollection[new Point(0, 0, i)].Decorator.ApplyMaterialState(TileMaterials.SINK);
            }
        }


        StartCoroutine(TurnDelay());
        return true;
    }
    
    private IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(EndDelay);
        EndTurn();    
    }
    //
    //Summary:
    //  Switches out the PreviousBoard with the CurrentBoard and sets the CurrentBoard active. 
    //  Returns True if the switch was successful, False if it has reached the end of the GeneratedBoards Count.
    //
    private bool ShiftToNextBoard()
    {
        if(CurrentBoardShown >= Builder.GeneratedBoard.Count) return false;


        CurrentBoardShown++;
        
        if(CurrentBoardShown >= Builder.GeneratedBoard.Count) return false;

        PreviousBoard = CurrentBoard;

        OnBoardLoad?.Invoke(CurrentBoardShown);
        CurrentBoard = Builder.GeneratedBoard[CurrentBoardShown];
        CurrentBoard.gameObject.SetActive(true);

        CurrentRowShown = 0;

        OnBoardSwitch?.Invoke(Builder.GeneratedBoard[CurrentBoardShown]);

        ShiftBoardRow();

        return true;
    }

    /// <summary>
    /// Returns all of the active Points on the board.
    /// If there's two boards active at a time, this script returns a combination of both board's tile position.
    /// </summary>
    /// <returns>Points that have been offsetted by their position.</returns>
    public List<Point> GetAllPointsFromActiveBoards()
    {
        if (CurrentBoard == null)
        { 
            return null;
        }
        List<Point> temporaryPointList = new List<Point>();

        foreach (var tile in CurrentBoard.WorldTileCollection)
        {
            if(tile.Key.x < GetMinimumVisibleRow() || tile.Key.x >= GetMaximumVisibleRow())
            {
                continue;
            }

            temporaryPointList.Add(tile.Key);
        }

        if(PreviousBoard == null) return temporaryPointList;

        foreach (var tile in PreviousBoard.WorldTileCollection)
        {
            if(tile.Key.x < GetMinimumVisibleRow() || tile.Key.x > GetMaximumVisibleRow())
            {
                continue;
            }

            temporaryPointList.Add(tile.Key);
        }

        return temporaryPointList;
    }
    //
    //Summary: 
    //      Returns a list of all tiles within the visible board.
    //
    //
    public List<Tile> GetAllTilesFromVisibleBoard()
    {
        if (CurrentBoard == null)
        { 
            return null;
        }
        List<Tile> temporaryPointList = new List<Tile>();

        foreach (var tile in CurrentBoard.WorldTileCollection)
        {
            if(tile.Key.x < GetMinimumVisibleRow() || tile.Key.x >= GetMaximumVisibleRow())
            {
                continue;
            }

            temporaryPointList.Add(tile.Value);
        }

        if(PreviousBoard == null) return temporaryPointList;

        foreach (var tile in PreviousBoard.WorldTileCollection)
        {
            if(tile.Key.x < GetMinimumVisibleRow() || tile.Key.x > GetMaximumVisibleRow())
            {
                continue;
            }

            temporaryPointList.Add(tile.Value);
        }

        return temporaryPointList;
    }
    //Summary:
    //  Returns a list of Tile Poisitions of with the specified Navigation Type.
    //
    //Parameters:
    //  Gets the TerrainNavigationType depending on the type given.
    //
    public List<Point> GetAllPointsWithNavigationTypeOf(TerrainNavigationType type)
    {
        if (CurrentBoard == null)
        { 
            return null;
        }
        List<Point> temporaryPointList = new List<Point>();

        foreach (var tile in CurrentBoard.GetTilesWithNavigationTypeOf(type))
        {
            if(CreatePointWithOffset(tile, CurrentBoard).x < GetMinimumVisibleRow() || CreatePointWithOffset(tile, CurrentBoard).x >= GetMaximumVisibleRow())
            {
                continue;
            }
            temporaryPointList.Add(CreatePointWithOffset(tile, CurrentBoard));
        }

        if(PreviousBoard == null) return temporaryPointList;

        foreach (var tile in PreviousBoard.GetTilesWithNavigationTypeOf(type))
        {
            if(CreatePointWithOffset(tile, PreviousBoard).x < GetMinimumVisibleRow() || CreatePointWithOffset(tile, PreviousBoard).x >= GetMaximumVisibleRow())
            {
                continue;
            }
            temporaryPointList.Add(CreatePointWithOffset(tile, PreviousBoard));
        }
        
        return temporaryPointList;
    }

    public List<Point> GetAllPointsWithTerrainTypeOf(TileType type)
    {
        if (CurrentBoard == null)
        { 
            return null;
        }
        List<Point> temporaryPointList = new List<Point>();

        foreach (var tile in CurrentBoard.GetTilesWithTerrainOf(type))
        {
            temporaryPointList.Add(CreatePointWithOffset(tile, CurrentBoard));
        }

        if(PreviousBoard == null) return temporaryPointList;

        foreach (var tile in PreviousBoard.GetTilesWithTerrainOf(type))
        {
            temporaryPointList.Add(CreatePointWithOffset(tile, PreviousBoard));
        }
        
        return temporaryPointList;
    }

    public int GetMinimumVisibleRow()
    {
        if(PreviousBoard == null)
        {
            return RapturedRow.Value + 1;
        }
        return CurrentRowShown + (PreviousBoard.BoardDimensions.x * PreviousBoard.BoardOffset);
    }

    public int GetMaximumVisibleRow()
    {
        return CurrentRowShown + (CurrentBoard.BoardDimensions.x * CurrentBoard.BoardOffset);
    }

    public void SwitchTilesFromActiveBoards(HashSet<Point> points, TileStates states)
    {
        if (CurrentBoard == null)
        { 
            return;
        }
        CurrentBoard.SwitchTileDecoration(points, states);
        if(PreviousBoard == null)
            return;
        PreviousBoard.SwitchTileDecoration(points, states);
    }

    public void ClearAllActiveBoardsDecorations()
    {
        if (CurrentBoard == null)
        { 
            return;
        }
        CurrentBoard.ClearAllDecorations();
        if(PreviousBoard == null)
            return;
        PreviousBoard.ClearAllDecorations();
    }

    public void ClearAllActiveBoardsDecorationsOfType(TileStates state)
    {
        if (CurrentBoard == null)
        { 
            return;
        }
        CurrentBoard.ClearAllDecorationsOfType(state);
        if(PreviousBoard == null)
            return;
        PreviousBoard.ClearAllDecorationsOfType(state);
    }
    //
    //Summary:
    //  Tells the TurnManager that this entity's turn is finished.
    //
    //
    //
    public void EndTurn()
    {
        if(TurnManager == null || TurnManager.ActiveTurnEntity != TurnEntities.ENVIRONMENT) return;
        TurnManager.NextEntityTurn();
    }
    //
    //Summary:
    //  Called by the EntityTurnManager to indicate the begining of this entity's turn.
    //  The BoardControll perform's it's shift inside of this method.
    //
    //
    public void StartTurn(EntityTurnManager turnManager)
    {
        if( TurnManager == null ) TurnManager = turnManager;

        OnTurnStart.Invoke();

        if(ShiftBoardRow() == true) 
            return;
        
        if(ShiftToNextBoard() == false)
        {
            EndTurn();
        }
    }
}
