using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class ItemToPool {
    public int        amountToPool;
    public GameObject objectToPool;
    public bool       shouldExpand;
}

public abstract class ManagerPool : ManagerGeneric {
    protected static List<GameObject> _pooledObjects = new List<GameObject>();
    public           List<ItemToPool> itemsToPool;

    protected void Start() {
        foreach (var item in itemsToPool)
            for (var i = 0; i < item.amountToPool; i++) {
                var obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }

        // Shuffle the Items
        _pooledObjects = Shuffle(_pooledObjects);
    }

    // protected void ShrinkList<T>() {
    //     int activeItems = 0;
    //
    //     foreach (var item in itemsToPool) {
    //         if
    //     }
    // }

    protected List<T> Shuffle<T>(List<T> list) {
        var rng = new Random();
        var n = list.Count;

        while (n > 1) {
            n--;
            var k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    public GameObject GetPooledObject(string requestTag) {
        if (_pooledObjects == null) {
            Debug.Log("Pool is null for request String: " + requestTag);

            return null;
        }

        foreach (var pObj in _pooledObjects)
            if (!pObj.activeInHierarchy && pObj.CompareTag(requestTag))
                return pObj;
        foreach (var item in itemsToPool)
            if (item.objectToPool.CompareTag(requestTag))
                if (item.shouldExpand) {
                    var obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    _pooledObjects.Add(obj);
                    return obj;
                }

        return null;
    }
}
