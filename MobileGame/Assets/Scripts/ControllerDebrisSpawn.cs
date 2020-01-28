using System.Collections;
using UnityEngine;

public enum SpawnPatternDebris {
    Test,
    Test2,
    Test3
}

public class ControllerDebrisSpawn : ControllerGeneric {
    //private bool _spawnEnemies;
    private float _spawnFreq;
    private Vector3 _moveSpeed;
    private int _direction;
    //private int _enemyCount;
    
    protected void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        _direction = Random.Range(0, 2) * 2 - 1;
    }
    
    public void StartMovement(SpawnPatternDebris pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case SpawnPatternDebris.Test:
                _spawnFreq = 3.0f;
                _moveSpeed = new Vector3(0,0,0.1f);
                break;
            case SpawnPatternDebris.Test2:
                _spawnFreq = 2.0f;
                _moveSpeed = new Vector3(0,0,0.3f);
                break;
            case SpawnPatternDebris.Test3:
                _spawnFreq = 1.0f;
                _moveSpeed = new Vector3(0,0,0.7f);
                break;
            default:
                _spawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        StartCoroutine(Movement());
        StartCoroutine(DebrisSpawn());
    }

    private IEnumerator Movement() {
        while (true) {
            var tempRb = gameObject.GetComponent<Rigidbody>();
            var distanceToEdge = new Vector2(_boundary.zMax - tempRb.position.z, tempRb.position.z - _boundary.zMin);
            
            if (distanceToEdge.x < 3.0f || distanceToEdge.y < 3.0f) {
                //Debug.Log("I'm getting close to the edge.");
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

    private IEnumerator DebrisSpawn() {
        yield return new WaitForSeconds(_spawnFreq);
        
        while (true) {
            var enemy = ManagerPoolDebris.instance.GetPooledObject("Debris_Small");
            if (enemy != null) {
                enemy.transform.position = gameObject.transform.position;
                enemy.transform.rotation = Quaternion.Euler(0,-90,0);
                enemy.GetComponent<Rigidbody>().velocity = Vector3.left * 10;
                enemy.SetActive(true);
            }

            yield return new WaitForSeconds(_spawnFreq);
        }
    }
}