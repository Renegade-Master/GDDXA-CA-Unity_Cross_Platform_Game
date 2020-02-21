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

    // Handle an Object ENTERING the collider
    private void OnTriggerEnter(Collider other) {
        // throw new NotImplementedException();
    }

    // Handle and Object EXITING the collider
    private void OnTriggerExit(Collider other) {
        //Debug.Log("Something left: " + other.name);

        if (other.tag.Contains("Shot")) {
            //Debug.Log("Destroying Shot");
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }

        if (other.tag.Contains("Enemy")) {
            //Debug.Log("Destroying Enemy");
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }

        if (other.tag.Contains("Debris")) {
            //Debug.Log("Destroying Debris");
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
