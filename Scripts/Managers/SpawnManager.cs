/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.KillZoneSystem;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.Units;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.SpawnSystem
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;

        public static SpawnManager Instance
        {
            get => _instance;
            private set => _instance = value;
        }

        [SerializeField]
        private SpawnData Data;
        [SerializeField]
        private UnitsMap UnitMap;
        [SerializeField]
        private BoardController Board;
        [SerializeField]
        private EnvironmentManager EnvManager;
        [SerializeField]
        private FXWrapper SpawnFX;
        [SerializeField]
        private RefInt SpawnedWhelps;
        public int MaxWhelps;
        [SerializeField]
        private float PentagramSpawnDelay;


        private Dictionary<UnitType, GameObject> SpawnDictionary = new Dictionary<UnitType, GameObject>();
        private Dictionary<UnitType, GameObject> PentagramDictionary = new Dictionary<UnitType, GameObject>();


        // Eventually switch out with a pooling mechanic.
        public Dictionary<UnitType, List<Point>>      LoadedPoints {get; private set;} = new Dictionary<UnitType, List<Point>>();
        public Dictionary<UnitType, List<GameObject>> Pentagrams   {get; private set;} = new Dictionary<UnitType, List<GameObject>>();

        public static Action OnUnitSpawn;
        

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);

            if(Data == null || UnitMap == null)
            {
                Debug.LogError("[SpawnManager] : SpawnData or UnitsMap reference is missing! did you forget to assign it in the inspector?");
                return;
            }

            GetDictionaries();
            CreateContainers();
        }

        private void OnEnable()
        {
            SpawnedWhelps.Value = 0;
            EnemyUnit.EnemyDied += PrepareHellBloom;
        }

        private void OnDisable()
        {
            SpawnedWhelps.Value = 0;
            EnemyUnit.EnemyDied -= PrepareHellBloom;
        }

        private void CreateContainers()
        {
            LoadedPoints.Add(UnitType.WHELP, new List<Point>());
            LoadedPoints.Add(UnitType.TESTAMENT, new List<Point>());
            LoadedPoints.Add(UnitType.HELLBLOOM, new List<Point>());

            Pentagrams.Add(UnitType.WHELP, new List<GameObject>());
            Pentagrams.Add(UnitType.TESTAMENT, new List<GameObject>());
            Pentagrams.Add(UnitType.HELLBLOOM, new List<GameObject>());
        }

        private void GetDictionaries()
        {
            SpawnDictionary = Data.GetDictionary();
            PentagramDictionary = Data.GetPentagramDictionary();
        }

        public Point GetRandomPoint(int min, int max)
        {
            // INFO: in the z of this point i've hard coded the range to go to 0 to 8, because we will never have any board with a z value smaller or larger than 8.
            Point randomPoint = new Point(UnityEngine.Random.Range(min, max), 0 , UnityEngine.Random.Range(0, 8));
            return randomPoint;
        }

        private Point GetRandomFromCurrentThree()
        {
            Point ranPoint;

            int refPoint = (Board.CurrentRowShown - 3) + (Board.CurrentBoard.BoardOffset * Board.CurrentBoard.BoardDimensions.x);

            ranPoint = GetRandomPoint(refPoint, refPoint + 3);

            foreach (var item in LoadedPoints)
            {
                if(item.Value.Contains(ranPoint))
                {
                    return new Point(-1, -1 , -1);
                }
            }

            return ranPoint;
        }

        public Unit SpawnUnitAtRandom(UnitType type)
        {
            Point randomPoint = GetRandomFromCurrentThree();

            return SpawnUnit(type, randomPoint);
        }

        public Unit SpawnUnit(UnitType type, Point position)
        {
            if(UnitMap.Contains(position))
            {
                return null;
            }

            SpawnFX.Play(new Vector3(position.x, 1, position.z));
            //TODO:  Eventually switch out with a pooling mechanic.
            GameObject unit = Instantiate(SpawnDictionary[type], new Vector3(position.x, 1, position.z), Quaternion.identity);
            Unit spawnedUnit = unit.GetComponent<Unit>();
            spawnedUnit.SetPosition(position);

            // TODO: Do any extra set up for units here.
            // TODO: Add null check for if the given gameobject is not a unit.
            UnitMap.Add(spawnedUnit.GetPosition(), spawnedUnit);

            return spawnedUnit;
        }

        private IEnumerator DelaySpawnPoint()
        {

            if(SpawnedWhelps.Value < MaxWhelps)
            {
                int SpawnAmount = MaxWhelps - SpawnedWhelps.Value; 

                if(SpawnAmount > 0)
                {
                    for (int i = 0; i < SpawnAmount; i++)
                    {
                        Point ranPoint = GetRandomFromCurrentThree();

                        if(ranPoint.y == -1 || UnitMap.Contains(ranPoint) || KillZones.KillZoneCollection.Contains(ranPoint))
                        {
                            continue;
                        }

                        LoadSpawnPoint(UnitType.WHELP, ranPoint);
                        yield return new WaitForSeconds(PentagramSpawnDelay);
                    }
                }
            }
        }

        public void LoadSpawnPoint(UnitType type, Point point)
        {
            GameObject pentagram = Instantiate(PentagramDictionary[type], new Vector3(point.x, 1.01f, point.z), Quaternion.identity);

            foreach (Tile item in Board.GetAllTilesFromVisibleBoard())
            {
                if(item.EntityPosition == point)
                {
                    switch (type)
                    {
                        case UnitType.WHELP:
                            item.SwitchDescription(TileAuxillary.DescriptionTypes.PENTAGRAM);
                        break;
                        case UnitType.TESTAMENT:
                            item.SwitchDescription(TileAuxillary.DescriptionTypes.SIGIL);
                        break;
                        default:
                            item.SwitchDescription(TileAuxillary.DescriptionTypes.NORMAL);
                        break;
                    }
                }
            }

            Pentagrams[type].Add(pentagram);
            LoadedPoints[type].Add(point);
        }

        public void PrepareWhelp()
        {
            StartCoroutine(DelaySpawnPoint());
        }

        public void PrepareTestament(Point ranPoint)
        {
            Debug.Log($"[SpawnManager] Preparing Testament at {ranPoint}");
            LoadSpawnPoint(UnitType.TESTAMENT, ranPoint);
        }

        public void PrepareHellBloom(Point spawnPoint)
        {
            if(EnvManager.HellbloomCheckSpawn(spawnPoint) == false || UnitMap.Contains(spawnPoint) 
            || KillZones.KillZoneCollection.Contains(spawnPoint) || spawnPoint.x < Board.GetMinimumVisibleRow() || spawnPoint.x > Board.GetMaximumVisibleRow())
            {
                return;
            }

            LoadSpawnPoint(UnitType.HELLBLOOM, spawnPoint);
        }

        public void SpawnLoadedUnits()
        {
            foreach (var item in LoadedPoints)
            {                
                if(LoadedPoints[item.Key].Count > 0)
                {
                    for (int i = 0; i < LoadedPoints[item.Key].Count; i++)
                    {
                        GameObject tempObj = new GameObject();
                        tempObj.transform.position = new Vector3(LoadedPoints[item.Key][i].x, LoadedPoints[item.Key][i].y, LoadedPoints[item.Key][i].z);

                        if(item.Key != UnitType.HELLBLOOM)
                        {
                            SpawnUnit(item.Key, LoadedPoints[item.Key][i]);
                            OnUnitSpawn?.Invoke();
                            continue;
                        }

                        OnUnitSpawn?.Invoke();
                        EnvManager.SpawnHellBloom(LoadedPoints[item.Key][i], SpawnDictionary[UnitType.HELLBLOOM].GetComponent<HellBloom>());
                    }

                    foreach (GameObject obj in Pentagrams[item.Key])
                    {
                        if(obj == null)
                        {
                            continue;
                        }

                        foreach (Tile tile in Board.GetAllTilesFromVisibleBoard())
                        {
                            Point pos = new Point((int)obj.transform.position.x, 0 , (int)obj.transform.position.z);
                            if(tile.EntityPosition == pos)
                            {
                                tile.SwitchDescription(TileAuxillary.DescriptionTypes.NORMAL);
                            }
                        }

                        Destroy(obj);
                    }

                    LoadedPoints[item.Key].Clear();
                }
            }
        }
    }
}