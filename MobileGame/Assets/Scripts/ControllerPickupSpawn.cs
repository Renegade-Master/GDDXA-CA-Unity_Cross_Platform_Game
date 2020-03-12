using System.Collections;
using UnityEngine;

public enum SpawnPatternPickup {
    Test,
    Test2,
    Test3
}

public class ControllerPickupSpawn : ControllerGeneric {
    private int     _direction;
    private float   _healthSpawnFreq;
    private Vector3 _moveSpeed;
    private float   _speedSpawnFreq;

    protected new void Awake() {
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().pickupBoundary;
        _direction = Random.Range(0, 2) * 2 - 1;
    }

    public void StartMovement(SpawnPatternPickup pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case SpawnPatternPickup.Test:
                _healthSpawnFreq = 20.0f;
                _speedSpawnFreq = 30.0f;
                _moveSpeed = new Vector3(0, 0, 0.1f);
                break;
            case SpawnPatternPickup.Test2:
                _healthSpawnFreq = 14.0f;
                _speedSpawnFreq = 32.0f;
                _moveSpeed = new Vector3(0, 0, 0.3f);
                break;
            case SpawnPatternPickup.Test3:
                _healthSpawnFreq = 12.5f;
                _speedSpawnFreq = 20.0f;
                _moveSpeed = new Vector3(0, 0, 0.7f);
                break;
            default:
                _healthSpawnFreq = 0.0f;
                _speedSpawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        StartCoroutine(Movement());
        StartCoroutine(PickupHealthSpawn());
        StartCoroutine(PickupShootSpeedSpawn());
    }

    // Spawn a specified Pickup on request
    public void SpawnPickup(string type) {
        var requestId = $"Pickup_{type}";

        var pickup = ManagerPoolPickup.instance.GetPooledObject(requestId);
        if (pickup != null) {
            pickup.transform.position = gameObject.transform.position;
            pickup.transform.rotation = Quaternion.Euler(0, -90, 0);
            pickup.GetComponent<Rigidbody>().velocity =
                Vector3.left * pickup.GetComponent<ControllerPickup>().speed;
            pickup.SetActive(true);
        }
    }


    private IEnumerator Movement() {
        while (true) {
            var tempRb         = gameObject.GetComponent<Rigidbody>();
            var position       = tempRb.position;
            var distanceToEdge = new Vector2(Boundary.zMax - position.z, position.z - Boundary.zMin);

            if (distanceToEdge.x < 3.0f || distanceToEdge.y < 3.0f) {
                _direction *= -1;
            }

            tempRb.MovePosition(tempRb.position + _moveSpeed * _direction);

            tempRb.position = new Vector3(
                10,
                0,
                Mathf.Clamp(tempRb.position.z, Boundary.zMin, Boundary.zMax)
            );
            yield return new WaitForEndOfFrame();
        }
    }

    // Periodically spawn Health Pickups
    private IEnumerator PickupHealthSpawn() {
        yield return new WaitForSeconds(_healthSpawnFreq);

        while (true) {
            var pickup = ManagerPoolPickup.instance.GetPooledObject("Pickup_Health_Restore");
            if (pickup != null) {
                pickup.transform.position = gameObject.transform.position;
                pickup.transform.rotation = Quaternion.Euler(0, -90, 0);
                pickup.GetComponent<Rigidbody>().velocity =
                    Vector3.left * pickup.GetComponent<ControllerPickup>().speed;
                pickup.SetActive(true);
            }

            yield return new WaitForSeconds(_healthSpawnFreq);
        }
    }

    // Periodically spawn Shoot Speed Pickups
    private IEnumerator PickupShootSpeedSpawn() {
        yield return new WaitForSeconds(_speedSpawnFreq);

        while (true) {
            var pickup = ManagerPoolPickup.instance.GetPooledObject("Pickup_Shoot_Speed");
            if (pickup != null) {
                pickup.transform.position = gameObject.transform.position;
                pickup.transform.rotation = Quaternion.Euler(0, -90, 0);
                pickup.GetComponent<Rigidbody>().velocity =
                    Vector3.left * pickup.GetComponent<ControllerPickup>().speed;
                pickup.SetActive(true);
            }

            yield return new WaitForSeconds(_speedSpawnFreq);
        }
    }
}
