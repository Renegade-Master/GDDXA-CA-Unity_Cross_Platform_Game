using UnityEngine;

public class ControllerProjectile : ControllerGeneric {
    public float speed;
    public int power;

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
    }
}
