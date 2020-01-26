using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GameObject shot;
    
    private BoundaryManager _boundary;
    [SerializeField] private Transform _shotSpawn;

    private void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoundaryManager>();
    }

    void Update() {
        GetPlayerInput();
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        // Clamp the Player to within the bounds of the screen
        GetComponent<Rigidbody>().position = new Vector3 (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, _boundary.xMin, _boundary.xMax),
            0,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, _boundary.zMin, _boundary.zMax)
        );
        //GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    void GetPlayerInput() {
        if (Input.GetButtonDown("Fire1")) {
            GameObject bullet = ShotManager.SharedInstance.GetPooledObject("Shot"); 
            if (bullet != null) {
                bullet.transform.position = _shotSpawn.position;
                bullet.transform.rotation = _shotSpawn.rotation;
                bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
                bullet.SetActive(true);
            }
        }
    }
}