using System;
using UnityEngine;

[Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class BoundaryManager : GenericManager {
    public Boundary playerBoundary;
    public Boundary enemyBoundary;
    public Boundary projectileBoundary;

    // Handle an Object ENTERING the collider
    private void OnTriggerEnter(Collider other) {
        throw new NotImplementedException();
    }

    // Handle and Object EXITING the collider
    private void OnTriggerExit(Collider other) {
        Debug.Log("Something left: " + other.name);

        if (other.CompareTag("Shot")) {
            Debug.Log("Destroying Shot");
            other.gameObject.SetActive(false);
        }
    }
}
