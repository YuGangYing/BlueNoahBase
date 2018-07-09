﻿using UnityEngine;
using System.Collections.Generic;

namespace BlueNoah.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("BlueNoah/UI/PoolObject")]
    public class PoolObject : MonoBehaviour
    {
        public string poolName;
        public bool isPooled;
    }

    public enum PoolInflationType
    {
        /// When a dynamic pool inflates, add one to the pool.
        INCREMENT,
        /// When a dynamic pool inflates, double the size of the pool
        DOUBLE
    }

    class Pool
    {
        private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

        //the root obj for unused obj
        private GameObject rootObj;
        private PoolInflationType inflationType;
        private string poolName;
        private int objectsInUse = 0;

        public Pool(string poolName, GameObject poolObjectPrefab, GameObject rootPoolObj, int initialCount, PoolInflationType type)
        {
            if (poolObjectPrefab == null)
            {
#if UNITY_EDITOR
                Debug.LogError("[ObjPoolManager] null pool object prefab !");
#endif
                return;
            }
            this.poolName = poolName;
            this.inflationType = type;
            this.rootObj = new GameObject(poolName + "Pool");
            this.rootObj.transform.SetParent(rootPoolObj.transform, false);

            // In case the origin one is Destroyed, we should keep at least one
            GameObject go = Object.Instantiate(poolObjectPrefab);
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                po = go.AddComponent<PoolObject>();
            }
            po.poolName = poolName;
            AddObjectToPool(po);

            //populate the pool
            populatePool(Mathf.Max(initialCount, 1));
        }

        //o(1)
        private void AddObjectToPool(PoolObject po)
        {
            //add to pool
            po.gameObject.SetActive(false);
            po.gameObject.name = poolName;
            availableObjStack.Push(po);
            po.isPooled = true;
            //add to a root obj
            po.gameObject.transform.SetParent(rootObj.transform, false);
        }

        private void populatePool(int initialCount)
        {
            for (int index = 0; index < initialCount; index++)
            {
                PoolObject po = Object.Instantiate(availableObjStack.Peek());
                AddObjectToPool(po);
            }
        }

        //o(1)
        public GameObject NextAvailableObject(bool autoActive)
        {
            PoolObject po = null;
            if (availableObjStack.Count > 1)
            {
                po = availableObjStack.Pop();
            }
            else
            {
                int increaseSize = 0;
                //increment size var, this is for info purpose only
                if (inflationType == PoolInflationType.INCREMENT)
                {
                    increaseSize = 1;
                }
                else if (inflationType == PoolInflationType.DOUBLE)
                {
                    increaseSize = availableObjStack.Count + Mathf.Max(objectsInUse, 0);
                }
                if (increaseSize > 0)
                {
                    populatePool(increaseSize);
                    po = availableObjStack.Pop();
                }
            }

            GameObject result = null;
            if (po != null)
            {
                objectsInUse++;
                po.isPooled = false;
                result = po.gameObject;
                if (autoActive)
                {
                    result.SetActive(true);
                }
            }

            return result;
        }

        //o(1)
        public void ReturnObjectToPool(PoolObject po)
        {
            if (poolName.Equals(po.poolName))
            {
                objectsInUse--;
                /* we could have used availableObjStack.Contains(po) to check if this object is in pool.
                 * While that would have been more robust, it would have made this method O(n) 
                 */
                if (po.isPooled)
                {
#if UNITY_EDITOR
                    Debug.LogWarning(po.gameObject.name + " is already in pool. Why are you trying to return it again? Check usage.");
#endif
                }
                else
                {
                    AddObjectToPool(po);
                }
            }
            else
            {
                Debug.LogError(string.Format("Trying to add object to incorrect pool {0} {1}", po.poolName, poolName));
            }
        }
    }
}