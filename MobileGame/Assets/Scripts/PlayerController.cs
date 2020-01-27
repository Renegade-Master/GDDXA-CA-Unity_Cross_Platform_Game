using UnityEngine;

public class PlayerController : GenericController {
    protected new void Start() {
        base.Start();
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoundaryManager>().playerBoundary;
    }

    private void Update() {
        GetPlayerInput();
    }

    private void FixedUpdate() {
        var tempRb = GetComponent<Rigidbody>();

        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        tempRb.MovePosition(tempRb.position + movement);

        // Clamp the Player Position to within the bounds of the screen
        tempRb.position = new Vector3(
            Mathf.Clamp(tempRb.position.x, _boundary.xMin, _boundary.xMax),
            0,
            Mathf.Clamp(tempRb.position.z, _boundary.zMin, _boundary.zMax)
        );

        // Clamp the Player Rotation to within the bounds of the screen
        tempRb.rotation = Quaternion.Euler(
            Mathf.Clamp(tempRb.rotation.x, -95, -85),
            Mathf.Clamp(tempRb.rotation.y, -5, 5),
            Mathf.Clamp(tempRb.rotation.z, 85, 95)
        );

        GetComponent<Rigidbody>().position = tempRb.position;
    }

    private void GetPlayerInput() {
        if (Input.GetButton("Fire1")) Fire();
    }

    protected override void Fire() {
        var bullet = ShotManager.instance.GetPooledObject("Shot_Player_Main");
        if (bullet != null) {
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = _shotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
            bullet.SetActive(true);
        }
    }
}
