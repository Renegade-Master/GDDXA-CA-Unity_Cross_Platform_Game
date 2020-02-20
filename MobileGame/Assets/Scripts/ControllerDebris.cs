using UnityEngine;

public class ControllerDebris : ControllerGeneric {
    public float speed;
    public float repelForce;

    protected new void Update() {
        base.Update();
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    // Repel other GameObjects away from this Object
    private void OnCollisionEnter(Collision other) {
        Debug.Log("Debris Colliding with " + other.gameObject.name);

        // Destroy a Shot if it hits a Debris
        if (other.gameObject.tag.Contains("Shot")) {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
            return;
        }

        // If it's something else
        Debug.Log("Debris repelling away from " + other.gameObject.name);
        
        // Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
        // Vector3 awayFromOrigin = other.gameObject.transform.position - transform.position;
        //
        // otherRb.AddForce(awayFromOrigin * repelForce, ForceMode.Impulse);
    }
}
