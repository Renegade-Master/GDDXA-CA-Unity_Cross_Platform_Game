using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public BoundaryManager boundary;

    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;
    private float nextFire;

    private void Awake() {
        boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoundaryManager>();
    }

    void Update() {
        /*if (Input.GetButton ("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }*/
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        
        /*Debug.Log("Horizontal: " + moveHorizontal);
        Debug.Log("Vertical: " + moveVertical);*/

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        // Clamp the Player to within the bounds of the screen
        GetComponent<Rigidbody>().position = new Vector3 (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );
        GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}