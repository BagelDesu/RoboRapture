



/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.SpawnSystem;
using Edu.Vfs.RoboRapture.DataTypes;


namespace Edu.Vfs.RoboRapture.ScriptableLibrary
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    [CreateAssetMenu(menuName="Units/SpawnData")]
    public class SpawnData : ScriptableObject
    {
        public int MaxWhelpSpawn = 0;

        [SerializeField]
        private UnitObjectType[] UnitObjectRelation;

        [SerializeField]
        private UnitObjectType[] UnitPentagramRelation;


        public Dictionary<UnitType, GameObject> GetDictionary()
        {
            Dictionary<UnitType, GameObject> tempDictionary = new Dictionary<UnitType, GameObject>();

            foreach (UnitObjectType relation in UnitObjectRelation)
            {
                tempDictionary[relation.Type] = relation.UnitPrefab;
            }

            return tempDictionary;
        }

        public Dictionary<UnitType, GameObject> GetPentagramDictionary()
        {
            Dictionary<UnitType, GameObject> tempDictionary = new Dictionary<UnitType, GameObject>();

            foreach (UnitObjectType relation in UnitPentagramRelation)
            {
                tempDictionary[relation.Type] = relation.UnitPrefab;
            }

            return tempDictionary;
        }
    }
}
