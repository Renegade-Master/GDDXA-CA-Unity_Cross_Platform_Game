using UnityEngine;

public class ControllerDebris : ControllerGeneric {
    public float speed;

    protected new void Update() {
        base.Update();
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    // Repel other GameObjects away from this Object
    private void OnCollisionEnter(Collision other) {
        // Destroy a Shot if it hits a Debris
        if (other.gameObject.tag.Contains("Shot")) {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
