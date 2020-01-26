using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Boundary _boundary;
    private Transform _shotSpawn;

    private void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoundaryManager>().playerBoundary;
        _shotSpawn = transform.Find("ShotSpawn");
    }

    void Update() {
        GetPlayerInput();
    }

    void FixedUpdate() {
        Rigidbody tempRb = GetComponent<Rigidbody>();
        
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //tempRb.velocity = movement * speed;
        tempRb.MovePosition(tempRb.position + movement);

        // Clamp the Player Position to within the bounds of the screen
        tempRb.position = new Vector3 (
            Mathf.Clamp(tempRb.position.x, _boundary.xMin, _boundary.xMax),
            0,
            Mathf.Clamp(tempRb.position.z, _boundary.zMin, _boundary.zMax)
        );

        // Clamp the Player Rotation to within the bounds of the screen
        tempRb.rotation = Quaternion.Euler (
            Mathf.Clamp(tempRb.rotation.x, -95, -85),
            Mathf.Clamp(tempRb.rotation.y, -5, 5),
            Mathf.Clamp(tempRb.rotation.z, 85, 95)
        );

        GetComponent<Rigidbody>().position = tempRb.position;
    }

    void GetPlayerInput() {
        if (Input.GetButton("Fire1")) {
            GameObject bullet = ShotManager.SharedInstance.GetPooledObject("Shot_Enemy_Small_Main"); 
            if (bullet != null) {
                bullet.transform.position = _shotSpawn.position;
                bullet.transform.rotation = _shotSpawn.rotation;
                bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
                bullet.SetActive(true);
            }
        }
    }
}