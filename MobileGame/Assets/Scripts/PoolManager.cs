using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemToPool {
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}

public abstract class PoolManager : GenericManager {
    public List<ItemToPool> itemsToPool;
    
    private List<GameObject> _pooledObjects;

    void Start () {
        _pooledObjects = new List<GameObject>();
        foreach (ItemToPool item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }
    }
    
    public GameObject GetPooledObject(string requestTag) {
        foreach (GameObject pObj in _pooledObjects) {
            if (!pObj.activeInHierarchy && pObj.CompareTag(requestTag)) {
                return pObj;
            }
        }
        foreach (ItemToPool item in itemsToPool) {
            if (item.objectToPool.CompareTag(requestTag)) {
                if (item.shouldExpand) {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}