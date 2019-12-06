

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.TerrainSystem;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.TileAuxillary;
using Edu.Vfs.RoboRapture.ObjectPooling;
using Edu.Vfs.RoboRapture.Units;
using Edu.Vfs.RoboRapture.Scriptables;
using System;
using Edu.Vfs.RoboRapture.Environment;

///<summary>
/// Handles the coordination and referencing of the Tiles.
///</summary>
public class Board : MonoBehaviour
{
    public BiomeType               Biome;
    public bool                    SpawnsHellBlooms;
    public bool                    SpawnsLightning;
    public int                     BoardOffset;                                                                // The offset of the board relative to 0,0
    public Vector3Int              BoardDimensions;                                                            // The Dimensions of the board.
    public Vector2Int              RaptureRowProgress  { get; private set; } = new Vector2Int(0, 0);           // How far along has the board revealed the tile.
    public Dictionary<Point, Tile> LocalTileCollection { get; private set; } = new Dictionary<Point, Tile>();  // A Dictionary relating the tiles within the board with it's Local position.
    public Dictionary<Point, Tile> WorldTileCollection { get; private set; } = new Dictionary<Point, Tile>();  // A Dictionary relating the tiles within the board with it's World position

    //TODO this adds the environmental units to the unit major list move this responsibility to Board Controller.
    public Dictionary<Point, EnviromentalUnit> EntityCollection { get; private set; } = new Dictionary<Point, EnviromentalUnit>();  // A Dictionary containing all of this board's terrain entities.
    public Dictionary<Point, EnemyUnit> EnemyCollection {get; private set; } = new Dictionary<Point, EnemyUnit>();

    public UnitsMap Units;

    [SerializeField]
    private EntityParser BoardEntities;


    public static event Action<List<Point>> OnRowSink;

    private bool HasSetUpTiles = false;

    private void Start()
    {
        SetUpTiles();
    }

    public void SetUpTiles()
    {
        if(HasSetUpTiles)
        {
            return;
        }

        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            tile.ParentBoard = this;
            LocalTileCollection[tile.EntityPosition] = tile;
            WorldTileCollection[tile.GetPosition()] = tile;
        }

        // Parse the board and look for the entities using the parser
        BoardEntities = GetComponentInChildren<EntityParser>(true);

        if (BoardEntities == null)
        {
            Debug.LogWarning("No Entities present on board. This means that the board is a pure open terrain.");
            return;
        }

        foreach (EnemyUnit unit in BoardEntities.GetEnemyUnits())
        {
            unit.SetPosition(new Point(unit.GetPosition().x + (BoardDimensions.x * BoardOffset), 0 , unit.GetPosition().z));
            if(EnemyCollection.ContainsKey(unit.GetPosition()))
            {
                continue;
            }
            EnemyCollection.Add(unit.GetPosition(), unit); 
        }

        foreach (EnviromentalUnit unit in BoardEntities.GetEntities())
        {
            unit.SetUpPosition(BoardOffset, BoardDimensions.x);
            if(EntityCollection.ContainsKey(unit.GetPosition()))
            {
                continue;
            }
            EntityCollection.Add(unit.GetPosition(), unit);
        }

        HasSetUpTiles = true;
    }
    /// <summary>
    /// Switches the decorations of the provided points, with the provided state.
    /// </summary>
    /// <param name="tilePositions">points of  the desired tiles to be edited</param>
    /// <param name="state">the state to switch to</param>
    public void SwitchTileDecoration(HashSet<Point> tilePositions, TileStates state)
    {
        foreach (Point position in tilePositions)
        {
            if (WorldTileCollection.ContainsKey(position) == false)
                continue;
            WorldTileCollection[position].Decorator.ApplyTileState(state);
        }
    }
    /// <summary>
    /// Reverts the decoration of specific tiles with the decoration proveded to normal.
    /// </summary>
    /// <param name="state">the state to target</param>
    public void ClearAllDecorationsOfType(TileStates state)
    {
        foreach (var tile in LocalTileCollection)
        {
            if(tile.Value.Decorator.AppliedState == state)
            {
                tile.Value.Decorator.ApplyTileState(TileStates.NORMAL);
            }
        }
    }
    /// <summary>
    /// Set all of the tiles on the board to the normal state.
    /// </summary>
    public void ClearAllDecorations()
    {
        foreach (var tile in LocalTileCollection)
        {
            tile.Value.Decorator.ApplyTileState(TileStates.NORMAL);
        }
    }
    /// <summary>
    /// Get the tiles that allow a specific Navigation Type on it.
    /// </summary>
    /// <param name="type">the type to target</param>
    /// <returns>A List of tiles with the navigation type specified.</returns>
    public List<Tile> GetTilesWithNavigationTypeOf(TerrainNavigationType type)
    {
        List<Tile> acceptableTiles = new List<Tile>();

        foreach (var item in LocalTileCollection)
        {
            if (item.Value.TerrainType.GetValidNavigationType() == type)
            {
                acceptableTiles.Add(item.Value);
            }
        }

        return acceptableTiles;
    }
    //
    //  Summary:
    //
    //          Returns a list of tiles that has the Terrain type of the given.
    //
    public List<Tile> GetTilesWithTerrainOf(TileType type)
    {
        List<Tile> acceptableTiles = new List<Tile>();

        foreach (var item in LocalTileCollection)
        {
            if (item.Value.TerrainType.GetTileType() == type)
            {
                acceptableTiles.Add(item.Value);
            }
        }

        return acceptableTiles;
    }
    /// <summary>
    /// Increases the Rapture effect's Ascend value for the specified rows.
    /// </summary>
    /// <param name="numRow">The row that will be affected</param>
    public void ShiftVisibleArea(int numRow)
    {
        for (int i = 0; i < BoardDimensions.x; i++)
        {
            Point shiftingPoint = new Point(numRow, 0, i);

            LocalTileCollection[shiftingPoint].RaiseComponents();
        }
    }
    /// <summary>
    /// 
    ///     Calls the "SinkComponents" method on the row that is specified.
    /// 
    /// </summary>
    /// <param name="numRow">the row to sink</param>
    public void SinkVisibleArea(int numRow)
    {
        List<Point> sunkenPoints = new List<Point>();

        for (int i = 0; i < BoardDimensions.x; i++)
        {
            Point sPoint = new Point(numRow, 0, i);
            LocalTileCollection[sPoint].SinkComponents();
            
            sunkenPoints.Add(new Point(sPoint.x + (BoardOffset * BoardDimensions.x), 0, i));
        }

        OnRowSink.Invoke(sunkenPoints);
        ApplySinkShader(numRow);
    }
    /// <summary>
    /// 
    /// Applies the Sink decoration on the tiles about to get sunk
    /// 
    /// </summary>
    /// <param name="numRow"> The row to apply the decoration to</param>
    public void ApplySinkShader(int numRow)
    {
        for (int i = 0; i < BoardDimensions.x; i++)
        {
            Point sinkingPoint = new Point(numRow + 1, 0, i);

            if (LocalTileCollection.ContainsKey(sinkingPoint))
            {
                LocalTileCollection[sinkingPoint]?.Decorator.ApplyMaterialState(TileMaterials.SINK);
                LocalTileCollection[sinkingPoint].SwitchDescription(DescriptionTypes.RAPTURE);
            }
        }
    }

#if UNITY_ENGINE
    /// <summary>
    ///     
    /// *** DEPRICATED / ICEBOXED***
    /// 
    ///     Applies the raise shader to the given row.
    /// 
    /// </summary>
    /// <param name="numRow">the row to apply the shader to.</param>
    public void ApplyRaiseShader(int numRow)
    {
        for (int i = 0; i < BoardDimensions.x; i++)
        {
            Point[] raisedPoints = new Point[2];

            raisedPoints[0] = new Point(numRow + 1, 0, i);
            raisedPoints[1] = new Point(numRow + 2, 0, i);

            foreach (Point raisingPoint in raisedPoints)
            {                
                if (LocalTileCollection.ContainsKey(raisingPoint))
                {
                    LocalTileCollection[raisingPoint].Decorator.ApplyMaterialState(TileMaterials.RAISE);
                }
            }
        }
    }
#endif

}
