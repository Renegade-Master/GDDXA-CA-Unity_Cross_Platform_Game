using UnityEngine;

public class ControllerProjectile : ControllerGeneric {
    public float speed;
    public int power;

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }
}
