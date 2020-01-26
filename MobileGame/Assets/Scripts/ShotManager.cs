using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemToPool {
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}

public class ShotManager : GenericManager {
    public static ShotManager SharedInstance;
    public List<GameObject> pooledObjects;
    public List<ItemToPool> itemsToPool;

    private void Awake() {
        SharedInstance = this;
    }

    void Start () {
        pooledObjects = new List<GameObject>();
        foreach (ItemToPool item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    
    public GameObject GetPooledObject(string requestTag) {
        foreach (GameObject pObj in pooledObjects)
        {
            if (!pObj.activeInHierarchy && pObj.CompareTag(requestTag)) {
                return pObj;
            }
        }
        foreach (ItemToPool item in itemsToPool) {
            if (item.objectToPool.CompareTag(requestTag)) {
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }


}