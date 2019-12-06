

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
using Edu.Vfs.RoboRapture.Helpers;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using Edu.Vfs.RoboRapture.Scriptables;
using Edu.Vfs.RoboRapture.SpawnSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Edu.Vfs.RoboRapture.Environment
{
    ///<summary>
    ///-Periodically Spawns Lightning in the available boards.-
    ///</summary>
    public class LightningSpawner : MonoBehaviour
    {
        [SerializeField][Tooltip("The amount of lightning that will be loaded per turn")]
        private int             LightningInstances;                     // The amount of lightning that will be loaded per turn;
        [SerializeField][Tooltip("The amount of damage that the lightning will cause")]
        private int             LightningDamage;                        // The amount of damage that the lightning will cause;
        [SerializeField][Tooltip("The delay between each lightning hits")]
        private float           LightningIntervalDelay;                 // The delay between each lightning hits


        [SerializeField]
        private BoardController controller;                             // Our reference to the boards that will be affected.
        [SerializeField]
        private UnitsMap        units;
        [SerializeField]
        private GameObject      LightningIndicator;                     // The oimage to be displayed when lightning has been loaded;
        [SerializeField]
        private RefInt          CurrentRaptureTile;

        public UnityEvent       OnLightningHit;

        [SerializeField]
        private FXWrapper       LightningStrikeFX;

        [SerializeField]
        private SpawnManager    SManager;


        [SerializeField]
        private float ExposureStrength;

        [SerializeField]
        private float ExposureSpeed;

        [SerializeField]
        private float ExposureDuration;


        private List<Point>     LoadedLightningPoints           = new List<Point>();                  // The loaded points.
        private Dictionary<Point, GameObject> SpawnedIndicators = new Dictionary<Point, GameObject>();

        //
        //      Summary:
        //
        //          Filtering the added points to prevent any conflicts.
        //
        private void AddPointsToLoadedPoints(List<Point> points)
        {
            foreach (Point lightning in points)
            {
                if(LoadedLightningPoints.Contains(lightning) || lightning.z > 7 || lightning.z < 0 || 
                lightning.x > controller.GetMaximumVisibleRow() || lightning.x < controller.GetMinimumVisibleRow() + 1 || 
                KillZoneSystem.KillZones.KillZoneCollection.Contains(lightning))
                {
                    continue;
                }

                LoadedLightningPoints.Add(lightning);
                AddIndicatorToLocation(lightning);
            }
        }
        //
        //      Summary:
        //
        //          Creates an indicator on a tile that's about to get hit by lightning
        //
        private void AddIndicatorToLocation(Point point)
        {
            GameObject go = Instantiate(LightningIndicator, new Vector3(point.x, 1.01f, point.z), LightningIndicator.transform.rotation);

            foreach (Tile item in controller.GetAllTilesFromVisibleBoard())
            {
                if(item.EntityPosition == point)
                {
                    item.SwitchDescription(TileAuxillary.DescriptionTypes.LIGHTNING);
                }
            }

            SpawnedIndicators.Add(point, go);
        }
        // 
        //      Summary:
        //
        //           Called to load lightning at a specified point. The origin and the four cardinal directions will take damage.
        // 
        public void LoadPoints(Point origin)
        {
            List<Point> LoadedPattern = GenerateCrossPattern(origin);
  
            foreach (Point points in LoadedPattern)
            {
                if(SManager.LoadedPoints[UnitType.WHELP].Contains(points))
                {
                    Debug.Log($"[LIGHTNING] Removing loaded pentagram at {points}");
                    SManager.LoadedPoints[UnitType.WHELP].Remove(points);

                    foreach (GameObject pentagram in SManager.Pentagrams[UnitType.WHELP])
                    {
                        if(pentagram?.transform.position == new Vector3(points.x, points.y, points.z))
                        {
                            Debug.Log($"[LIGHTNING] Deleting pentagram at {points}");
                            SManager.Pentagrams[UnitType.WHELP].Remove(pentagram);
                            Destroy(pentagram);
                        }
                    }
                }
            }
            AddPointsToLoadedPoints(LoadedPattern);
            // ChainLightning(LoadedPattern);
        }
        //
        //      Summary:
        //
        //          Called to Cause lightning to apply damage to units.
        //
        public IEnumerator CastLightning()
        {
            if(LoadedLightningPoints.Count > 0)
            {
                foreach (Point item in LoadedLightningPoints)
                {
                    yield return new WaitForSeconds(LightningIntervalDelay);

                    EffectsPostProcessingController.Instance.ApplyEffect(EffectType.ExposureFlash, ExposureDuration, ExposureSpeed, ExposureStrength);

                    OnLightningHit?.Invoke();

                    LightningStrikeFX.Play(new Vector3(item.x, 1, item.z));

                    if(units.Contains(item))
                    {
                        units.Get(item).Health.ReduceHealth(LightningDamage);
                    }

                    Destroy(SpawnedIndicators[item].gameObject);

                    foreach (Tile tile in controller.GetAllTilesFromVisibleBoard())
                    {
                        if(tile.EntityPosition == item)
                        {
                            tile.SwitchDescription(TileAuxillary.DescriptionTypes.NORMAL);
                        }
                    }
                    
                }
                LoadedLightningPoints.Clear();
                SpawnedIndicators.Clear();
            }
            else
            {
                LoadLightning();
            }
        }

        public void LoadLightning()
        {
            if(CheckBoardForLightning())
            {
                for (int i = 0; i < LightningInstances; i++)
                {
                    LoadPoints(GetRandomPoint(controller.GetMinimumVisibleRow() + 2, controller.GetMaximumVisibleRow() - 1));
                }
            }
        }

        public bool CheckBoardForLightning()
        {
            if(controller.PreviousBoard != null)
            {
                // does the board allow Lightning spawning?
                if(controller.PreviousBoard.SpawnsLightning)
                {
                    return true;
                }
                
            }

            // does the board allow Lightning spawning?
            if(controller.CurrentBoard.SpawnsLightning)
            {
                return true;
            }
        
            return false;
        }

        public Point GetRandomPoint(int min, int max)
        {
            // INFO: in the z of this point i've hard coded the range to go to 0 to 8, because we will never have any board with a z value smaller or larger than 8.
            Point randomPoint = new Point(UnityEngine.Random.Range(min, max), 0 , UnityEngine.Random.Range(0, 8));
            return randomPoint;
        }

        public void StartCastingLightning()
        {
            StartCoroutine(CastLightning());
        }

        private void ChainLightning(List<Point> origins)
        {
            foreach (Point item in origins)
            {
                if(controller.GetAllPointsWithTerrainTypeOf(TerrainSystem.TileType.FLOODED).Contains(item))
                {
                    CheckChain(item);
                }
            }
        }

        private List<Point> GenerateCrossPattern(Point origin)
        {
            List<Point> LoadedPattern = new List<Point>();

            LoadedPattern.Add(origin);
            LoadedPattern.Add(new Point( origin.x + 1, origin.y ,origin.z));
            LoadedPattern.Add(new Point( origin.x - 1, origin.y ,origin.z));
            LoadedPattern.Add(new Point( origin.x, origin.y ,origin.z + 1 ));
            LoadedPattern.Add(new Point( origin.x, origin.y ,origin.z - 1));

            return LoadedPattern;
        }
        //
        //
        //  Summary:
        //
        //      Reccursive function for checking if the lightning hit a flooded tile.
        //
        private void CheckChain(Point origin)
        {
            List<Point> chain = GenerateCrossPattern(origin);

            foreach (Point item in chain)
            {
                if(LoadedLightningPoints.Contains(item))
                {
                    Debug.Log($"[LIGHTNING] CHECKING CHAIN IN {item}");
                    continue;
                }

                if(controller.GetAllPointsWithTerrainTypeOf(TerrainSystem.TileType.FLOODED).Contains(item) && item.x < controller.GetMaximumVisibleRow() && item.x > controller.GetMinimumVisibleRow())
                {
                    Debug.Log($"[LIGHTNING] CHECKING CHAIN IN {item}");
                    LoadedLightningPoints.Add(item);
                    AddIndicatorToLocation(item);
                    CheckChain(item);
                }
                else
                {
                    continue;
                }
            }
        }
    }    
}

