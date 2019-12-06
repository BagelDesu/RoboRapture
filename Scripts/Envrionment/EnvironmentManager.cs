

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
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

///<summary>
/// Handles quering and manipulation of ALL Envrionmental/Enemy Units
/// that are pre-placed and Environmental Unit Spawning
///</summary>
public class EnvironmentManager : MonoBehaviour
{
    public static Dictionary<Point, EnviromentalUnit> EnvironmentCollection { get; private set; } = new Dictionary<Point, EnviromentalUnit>();  // A Dictionary containing all of this board's terrain entities.
    public static Dictionary<Point, EnemyUnit> EnemyCollection {get; private set; } = new Dictionary<Point, EnemyUnit>();
    public UnitsMap Units;
    public BoardController controller;
    public GameObject Mountain;

    private bool AddUnitToMap(Point point, Unit unit)
    {
        if(Units.Contains(point))
        {
            Debug.LogError($"[EnvironmentManager] Units map already contains a unit in specified point {unit.name} {unit.GetPosition()}, Conflicting Unit: {Units.Get(point)} {Units.Get(point).GetPosition()}", Units.Get(point));
            return false;
        }

        Units.Add(point, unit);
        return true;
    }
    
    private void OnDisable()
    {
        EnvironmentCollection.Clear();
        EnemyCollection.Clear();
    }

    public Unit GetUnit(Type type, Point point)
    {
        switch (type)
        {
            case Type.Enemy:
                if(EnemyCollection.ContainsKey(point))
                {
                    return EnemyCollection[point]; 
                }
                Debug.LogError("[EnvironmentManager] Enemy Does not exist. Returning Null");
                return null;
            case Type.Envrionment:
                if(EnvironmentCollection.ContainsKey(point))
                {
                    return EnvironmentCollection[point]; 
                }
                Debug.LogError("[EnvironmentManager] Environment Does not exist. Returning Null");
                return null;
            default:
                Debug.LogError("[EnvironmentManager] GetUnit() only accepts Type.Environment or Type.Enemy");
                return null;
        }
    }

    public void MoveUnit(Type type, Point curPoint, Point newPoint)
    {
        switch (type)
        {
            case Type.Envrionment:

                if(!EnvironmentCollection.ContainsKey(curPoint) || EnvironmentCollection.ContainsKey(newPoint))
                {
                    Debug.LogError("[EnvironmentManager] Failed to set unit to new position");
                    return;
                }

                EnviromentalUnit temp = EnvironmentCollection[curPoint];
                EnvironmentCollection.Remove(curPoint);
                EnvironmentCollection.Add(newPoint, temp);
            break;
            case Type.Enemy:

                if(!EnemyCollection.ContainsKey(curPoint) || EnemyCollection.ContainsKey(newPoint))
                {
                    Debug.LogError("[EnvironmentManager] Failed to set unit to new position");
                    return;
                }

                EnemyUnit enemyTemp = EnemyCollection[curPoint];
                EnemyCollection.Remove(curPoint);
                EnemyCollection.Add(newPoint, enemyTemp);
            break;
            default:
            break;
        }
    }

    public void AddEnvironment(Point point, EnviromentalUnit unit)
    {
        if(EnvironmentCollection.ContainsKey(point) || EnvironmentCollection.ContainsValue(unit))
        {
            Debug.LogError($"[EnvironmentManager] Envrionmental Unit Already Exists. {unit.name} {unit.WorldPosition}, Conflicting Unit: {EnvironmentCollection[point].name} {EnvironmentCollection[point].WorldPosition}" , unit);
            return;
        }

        if(AddUnitToMap(point, unit))
        {
            EnvironmentCollection.Add(point, unit);
            unit.gameObject.SetActive(false);
            return;
        }
 
        Debug.LogError("[EnvironmentManager] Failed setting environment");
    }

    public void AddSpawnedEnvironment(Point point, EnviromentalUnit unit)
    {
        if(EnvironmentCollection.ContainsKey(point) || EnvironmentCollection.ContainsValue(unit))
        {
            Debug.LogError($"[EnvironmentManager] Envrionmental Unit Already Exists. {unit.name} {unit.WorldPosition}, Conflicting Unit: {EnvironmentCollection[point].name} {EnvironmentCollection[point].WorldPosition}" , unit);
            return;
        }

        if(AddUnitToMap(point, unit))
        {
            EnvironmentCollection.Add(point, unit);
            return;
        }
 
        Debug.LogError("[EnvironmentManager] Failed setting environment");
    }

    public void AddEnemy(Point point, EnemyUnit unit)
    {
        if(EnemyCollection.ContainsKey(point) || EnemyCollection.ContainsValue(unit))
        {
            Debug.LogError($"[EnvironmentManager] Enemy Unit Already Exists. {unit.name} {unit.GetPosition()}");
            return;
        }

        if(AddUnitToMap(point, unit))
        {
            EnemyCollection.Add(point, unit);
            unit.gameObject.SetActive(false);
            return;
        }

        Debug.LogError($"[EnvironmentManager] Failed setting Enemy {unit.name}, {unit.GetPosition()}", unit);
    }

    public void RaiseEntities(int refPoint)
    {
        //TODO/HACK: hard coded length
        for (int i = 0; i < 8; i++)
        {
            Point tarPoint = new Point(refPoint, 0, i);

            if(EnvironmentCollection.ContainsKey(tarPoint))
            {
                if(EnvironmentCollection[tarPoint] == null)
                {
                    return;
                }
                EnvironmentCollection[tarPoint]?.gameObject.SetActive(true);
                EnvironmentCollection[tarPoint]?.GetComponent<TileDecorator>()?.RaiseComponents();
            }

            if(EnemyCollection.ContainsKey(tarPoint))
            {

                if(EnemyCollection[tarPoint] == null)
                {
                    return;
                }
                EnemyCollection[tarPoint]?.gameObject.SetActive(true);
                EnemyCollection[tarPoint]?.GetComponent<TileDecorator>()?.RaiseComponents();
            }

        }
    }

    public void SinkEntities(int refPoint)
    {
        //TODO/HACK: hard coded length
        for (int i = 0; i < 8; i++)
        {
            Point tarPoint = new Point(refPoint, 0, i);
            if(EnvironmentCollection.ContainsKey(tarPoint))
            {
                if(EnvironmentCollection[tarPoint] == null)
                {
                    continue;
                }
                EnvironmentCollection[tarPoint].GetComponent<TileDecorator>().SinkComponents();
                EnvironmentCollection[tarPoint].gameObject.SetActive(false);

            }
        }

        for (int i = 0; i < 8; i++)
        {
            Point tarPoint = new Point(refPoint, 0, i);
            if(EnemyCollection.ContainsKey(tarPoint))
            {
                if(EnemyCollection[tarPoint] == null)
                {
                    continue;
                }
                EnemyCollection[tarPoint].GetComponent<TileDecorator>()?.SinkComponents();
            }
        }
    }

    public void InitializeUnits(Board board)
    {
        foreach (var unit in board.EntityCollection)
        {
            AddEnvironment(unit.Key, unit.Value);
        }

        foreach (var unit in board.EnemyCollection)
        {
            AddEnemy(unit.Key, unit.Value);
        }
    }

    public void SpawnMountain(Point SpawnPoint)
    {
        if(EnvironmentCollection.ContainsKey(SpawnPoint) || Units.Contains(SpawnPoint))
        {
            return;
        }

        GameObject temp = GameObject.Instantiate(Mountain, new Vector3(SpawnPoint.x, 1, SpawnPoint.z), Quaternion.identity);
        temp.GetComponent<EnviromentalUnit>().WorldPosition = SpawnPoint;
        temp.GetComponent<EnviromentalUnit>().SetPosition(SpawnPoint);
        temp.GetComponent<TileDecorator>().RaiseComponents();
        AddSpawnedEnvironment(SpawnPoint, temp.GetComponent<EnviromentalUnit>());
    }

    public void SpawnHellBloom (Point SpawnPoint, HellBloom hellbloomPrefab)
    {
        if(EnvironmentCollection.ContainsKey(SpawnPoint) || Units.Contains(SpawnPoint))
        {
            return;
        }

        HellBloom temp = Instantiate(hellbloomPrefab, new Vector3(SpawnPoint.x, 1, SpawnPoint.z), Quaternion.identity);
        temp.GetComponent<EnviromentalUnit>().WorldPosition = SpawnPoint;
        temp.GetComponent<EnviromentalUnit>().SetPosition(SpawnPoint);
        temp.GetComponent<TileDecorator>().RaiseInstant();
        AddSpawnedEnvironment(SpawnPoint, temp.GetComponent<EnviromentalUnit>());
    }

    public bool HellbloomCheckSpawn(Point SpawnPoint)
    {
        // is the spawn point in the previous board?
        if(controller.PreviousBoard != null)
        {
            if(controller.PreviousBoard.WorldTileCollection.ContainsKey(SpawnPoint))
            {
                // does the board allow hellbloom spawning?
                if(controller.PreviousBoard.SpawnsHellBlooms)
                {
                    return true;
                }
            }
        }

        // is the spawn point in the current board?
        if(controller.CurrentBoard.WorldTileCollection.ContainsKey(SpawnPoint))
        {
            // does the board allow hellbloom spawning?
            if(controller.CurrentBoard.SpawnsHellBlooms)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backslash))
        {
            SpawnMountain(new Point(3, 0 , 2));
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            foreach (var item in EnvironmentCollection)
            {
                Debug.Log($"[EnvironmentManager] Has The following Registered in the envrionment : {item.Key} {item.Value}");
            }

            foreach (var item in EnemyCollection)
            {
                Debug.Log($"[EnvironmentManager] Has The following Registered in the enemy : {item.Key} {item.Value}");
            }
        }
    }
}