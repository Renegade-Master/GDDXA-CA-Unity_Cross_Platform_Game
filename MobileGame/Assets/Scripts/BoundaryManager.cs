using System;
using UnityEngine;

public class BoundaryManager : GenericManager {
    public float xMin, xMax, zMin, zMax;

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
