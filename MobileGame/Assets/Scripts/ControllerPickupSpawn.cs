using System.Collections;
using UnityEngine;

public enum SpawnPatternPickup {
    Test,
    Test2,
    Test3
}

public class ControllerPickupSpawn : ControllerGeneric {
    private int     _direction;
    private Vector3 _moveSpeed;
    private float   _spawnFreq;

    protected new void Awake() {
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().pickupBoundary;
        _direction = Random.Range(0, 2) * 2 - 1;
    }

    public void StartMovement(SpawnPatternPickup pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case SpawnPatternPickup.Test:
                _spawnFreq = 3.0f;
                _moveSpeed = new Vector3(0, 0, 0.1f);
                break;
            case SpawnPatternPickup.Test2:
                _spawnFreq = 2.0f;
                _moveSpeed = new Vector3(0, 0, 0.3f);
                break;
            case SpawnPatternPickup.Test3:
                _spawnFreq = 1.0f;
                _moveSpeed = new Vector3(0, 0, 0.7f);
                break;
            default:
                _spawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        //StartCoroutine(Movement());
        //StartCoroutine(PickupSpawn());
    }

    private IEnumerator Movement() {
        while (true) {
            var tempRb = gameObject.GetComponent<Rigidbody>();
            var position = tempRb.position;
            var distanceToEdge = new Vector2(Boundary.zMax - position.z, position.z - Boundary.zMin);

            if (distanceToEdge.x < 3.0f || distanceToEdge.y < 3.0f)
                _direction *= -1;

            tempRb.MovePosition(tempRb.position + _moveSpeed * _direction);

            tempRb.position = new Vector3(
                60,
                0,
                Mathf.Clamp(tempRb.position.z, Boundary.zMin, Boundary.zMax)
            );
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator PickupSpawn() {
        yield return new WaitForSeconds(_spawnFreq);

        while (true) {
            var debris = ManagerPoolPickup.instance.GetPooledObject("Pickup_Small");
            if (debris != null) {
                debris.transform.position = gameObject.transform.position;
                debris.transform.rotation = Quaternion.Euler(0, -90, 0);
                debris.GetComponent<Rigidbody>().velocity =
                    Vector3.left * debris.GetComponent<ControllerPickup>().speed;
                debris.SetActive(true);
            }

            yield return new WaitForSeconds(_spawnFreq);
        }
    }
}
