using System;
using UnityEngine;

[Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class ManagerBoundary : ManagerGeneric {
    public Boundary enemyBoundary;
    public Boundary pickupBoundary;
    public Boundary playerBoundary;
    public Boundary projectileBoundary;

    // Handle and Object EXITING the collider
    private void OnTriggerExit(Collider other) {
        if (other.tag.Contains("Shot")) {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }

        if (other.tag.Contains("Enemy")) {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }

        if (other.tag.Contains("Debris")) {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
