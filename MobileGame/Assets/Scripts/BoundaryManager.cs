using System;
using UnityEngine;

public class BoundaryManager : MonoBehaviour {
    public float xMin, xMax, zMin, zMax;

    // Handle an Object ENTERING the collider
    private void OnTriggerEnter(Collider other) {
        throw new NotImplementedException();
    }

    // Handle and Object EXITING the collider
    private void OnTriggerExit(Collider other) {
        throw new NotImplementedException();
    }
}
