using UnityEngine;

/**
 * Rotation Notes
 *     Forward = Blue Axis
 *     Right   = Red Axis
 *     Up      = Green Axis
 */

public class ControllerProjectile : ControllerGeneric {
    public float speed;
    public int power;

    protected new void Update() {
        base.Update();
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
    }
}
