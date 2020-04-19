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


/**
 * <summary>
 * Class <c>ManagerPool</c> 
 * <para>
 * The Generic Object-Pool Managing Class.  Is inherited by all sub-type
 * Object-Poolers.  Inherits from the ManagerGeneric <c>Class</c>
 * </para>
 * </summary>
 */
public abstract class ManagerPool : ManagerGeneric {
    protected static List<GameObject> _pooledObjects = new List<GameObject>();
    public           List<ItemToPool> itemsToPool;

    
    /**
     * <summary>
     * Function <c>Start</c> 
     * <para>
     * Runs when the GameObject that this script is attached to is
     * initialised.  Creates the ObjectPools to be used by inheriting
     * Classes.  Also shuffles the order of the items in the Pool.
     * </para>
     * </summary>
     */
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

    
    /**
     * <summary>
     * Function <c>Shuffle</c> 
     * <para>
     * Randomises the order of items in a given list
     * </para>
     * <param name="list">A list of type T to be randomised</param>
     * <typeparam name="T">The type stored by the list</typeparam>
     * <returns>A list of type T with randomised order</returns>
     * </summary>
     */
    private List<T> Shuffle<T>(List<T> list) {
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

    
    /**
     * <summary>
     * Function <c>GetPooledObject</c>
     * <para>
     * Retrieves a specific family of Object from the Object-Pooler
     * </para>
     * <param name="requestTag">A <c>tag</c> to specify the family of item that
     * should be retrieved from the Pool</param>
     * <returns>A GameObject of the specified family</returns>
     * </summary>
     */
    public GameObject GetPooledObject(string requestTag) {
        if (_pooledObjects == null) return null;

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
    

    // protected void ShrinkPool<T>() {
    //     var activeItems = 0;
    //
    //     foreach (var item in _pooledObjects)
    //         if (item.activeInHierarchy)
    //             activeItems++;
    //
    //     float usedRatio = activeItems / _pooledObjects.Capacity;
    //
    //     if (usedRatio > 0.7f) 
    // }
}
