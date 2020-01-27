using System.Collections;
using UnityEngine;

public enum EnemySpawnPattern {
    Test,
    Test2,
    Test3
}

public class ControllerEnemySpawn : ControllerGeneric {
    private bool _spawnEnemies;
    private float _spawnFreq;
    private Vector3 _moveSpeed;
    private int _direction = 1;
    private int _enemyCount;
    
    protected void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
    }
    
    public void StartMovement(EnemySpawnPattern pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case EnemySpawnPattern.Test:
                _spawnFreq = 1.0f;
                _moveSpeed = new Vector3(0,0,0.33f);
                break;
            case EnemySpawnPattern.Test2:
                _spawnFreq = 2.0f;
                _moveSpeed = new Vector3(0,0,0.66f);
                break;
            case EnemySpawnPattern.Test3:
                _spawnFreq = 3.0f;
                _moveSpeed = new Vector3(0,0,1);
                break;
            default:
                _spawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        StartCoroutine(Movement());
    }

    private IEnumerator Movement() {
        while (true) {
            //Debug.Log("Movement Method Executing");
            var tempRb = gameObject.GetComponent<Rigidbody>();
            var distanceToEdge = new Vector2(_boundary.zMax - tempRb.position.z, tempRb.position.z - _boundary.zMin);
            Debug.Log("Distance: " + distanceToEdge);

            if (distanceToEdge.x < 3.0f || distanceToEdge.y < 3.0f) {
                Debug.Log("I'm getting close to the edge.");
                _direction *= -1;
            }

            tempRb.MovePosition(tempRb.position + (_moveSpeed * _direction));

            tempRb.position = new Vector3(
                60,
                0,
                Mathf.Clamp(tempRb.position.z, _boundary.zMin, _boundary.zMax)
            );
            yield return new WaitForEndOfFrame();
        }
    } 
}
