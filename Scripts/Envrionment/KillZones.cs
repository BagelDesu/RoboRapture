

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.KillZoneSystem
{    
    ///<summary>
    ///-Handles the containment of all killzones -
    ///</summary>
    public class KillZones : MonoBehaviour
    {
        public static List<Point> KillZoneCollection {get; private set;} = new List<Point>();       // KillZoneCollection. A static list

        [SerializeField]
        private BoardController boardController;                                                    // The BoardController. Used to figure out which tiles are tagged as "NONE"

        //
        //  Summary:
        // 
        //      On Enable, We'll get all of the locations on the board that has been set to "NONE" navigation
        //
        private void Start()
        {   
            foreach (Board board in boardController.BuilderProp.GeneratedBoard)
            {
                foreach(Tile tile in board.GetTilesWithNavigationTypeOf(TerrainSystem.TerrainNavigationType.NONE))
                {
                    AddKillZone(new Point(tile.EntityPosition.x + (board.BoardDimensions.x * board.BoardOffset), tile.EntityPosition.y, tile.EntityPosition.z));
                }
            }
        }
        //
        //   Summary:
        //
        //      On Disable, We'll clear the static collection.
        //
        private void OnDisable()
        {
            KillZoneCollection.Clear();
        }
#if UNITY_EDITOR
        //
        //  Summary:
        //
        //      Debug method for checking where the killzones are.
        //
        public static void PrintKillZones()
        {
            Debug.Log($"[KILL ZONE] {KillZoneCollection.Count}");

            foreach (Point item in KillZoneCollection)
            {
                Debug.Log($"[KILL ZONE] {item} is a Killzone");
            }
        }

        public void LocalPrintKillZones()
        {
            Debug.Log($"[KILL ZONE] {KillZoneCollection.Count}");

            foreach (Point item in KillZoneCollection)
            {
                Debug.Log($"[KILL ZONE] {item} is a Killzone");
            }
        }
#endif
        //
        //  Summary:
        // 
        //      Add a point to the killzone collection. Automatically checks for duplicates
        //
        public static void AddKillZone(Point location)
        {
            if(KillZoneCollection.Contains(location))
            {
                return;
            }

            KillZoneCollection.Add(location);
        }
        //
        //  Summary:
        // 
        //      Remove a point to the killzone collection. Automatically checks for duplicates
        //
        public static void RemoveKillZone(Point location)
        {
            Debug.Log($"[KILL ZONE] Removing {location}");
            if(!KillZoneCollection.Contains(location))
            {
                return;
            }

            KillZoneCollection.Remove(location); 
        }
    }
}
