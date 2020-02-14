using UnityEngine;

public class ControllerProjectile : ControllerGeneric {
    public float speed;
    
    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }
}
