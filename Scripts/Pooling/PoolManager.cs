

/*
*      @Author: Carlos Miguel Aquino
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Note: PoolManager.cs was created by Willy Campos as part of an assignment within VFS Unity 3 Class. We have received permission to use this script.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.ObjectPooling
{
    ///<summary>
    /// 
    ///</summary>
    public static class PoolManager
    {
        private static readonly int INITIAL_POOL_SIZE = 5;

        private static Dictionary<int, List<PoolableObject>> PoolsDictionary = new Dictionary<int, List<PoolableObject>>();
        private static Dictionary<int, int> IndexesCollection = new Dictionary<int, int>();

        private static PoolableObject AddObjectToPool(PoolableObject prefab, int id)
        {
            var clone = GameObject.Instantiate(prefab);
            clone.gameObject.SetActive(false);
            PoolsDictionary[id].Add(clone);
            return clone;
        }

        public static void CreatePool(PoolableObject prefab,int poolSize)
        {
            var id = prefab.GetInstanceID();

            PoolsDictionary[id] = new List<PoolableObject>(poolSize);
            IndexesCollection[id] = 0;

            for (int i = 0; i < poolSize; i++)
            {
                AddObjectToPool(prefab, id);
            }
        }

        public static PoolableObject GetNext(PoolableObject prefab, Vector3 position, Quaternion rotation, bool isActive = true)
        {
            var clone = GetNext(prefab);
            
            clone.transform.position = position;
            clone.transform.rotation = rotation;

            clone.gameObject.SetActive(isActive);

            return clone;

        }

        public static PoolableObject GetNext(PoolableObject prefab)
        {
            var id = prefab.GetInstanceID();

            if(PoolsDictionary.ContainsKey(id) == false)
            {
                CreatePool(prefab, INITIAL_POOL_SIZE);
            }
            
            for (int i = 0; i < PoolsDictionary[id].Count; i++)
            {
                IndexesCollection[id] = (IndexesCollection[id] + 1) % PoolsDictionary[id].Count;

                if(PoolsDictionary[id][IndexesCollection[id]].gameObject.activeInHierarchy == false)
                {
                    return PoolsDictionary[id][IndexesCollection[id]];
                }
                
            }

            return AddObjectToPool(prefab, id);
            
        }
    }
}