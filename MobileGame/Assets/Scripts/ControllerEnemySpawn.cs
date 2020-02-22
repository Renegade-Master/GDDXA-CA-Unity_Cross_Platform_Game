using System.Collections;
using System.Reflection;
using UnityEngine;

public enum SpawnPatternEnemy {
    Test,
    Test2,
    Test3
}

public class ControllerEnemySpawn : ControllerGeneric {
    private int     _direction;
    private string  _enemyToSpawn;
    private Vector3 _moveSpeed;
    private float   _spawnFreq = 5.0f;

    protected new void Awake() {
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        _direction = Random.Range(0, 2) * 2 - 1;
    }

    public void StartMovement(SpawnPatternEnemy pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case SpawnPatternEnemy.Test:
                _enemyToSpawn = "Enemy_Small";
                _moveSpeed = new Vector3(0, 0, 0.33f);
                break;
            case SpawnPatternEnemy.Test2:
                _enemyToSpawn = "Enemy_Medium";
                _moveSpeed = new Vector3(0, 0, 0.5f);
                break;
            case SpawnPatternEnemy.Test3:
                _spawnFreq = 3.5f;
                _enemyToSpawn = "Enemy_Medium";
                _moveSpeed = new Vector3(0, 0, 0.75f);
                break;
            default:
                //_spawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        StartCoroutine(Movement());
        StartCoroutine(EnemySpawn());
    }

    // Spawn a specified Boss on request
    public GameObject SpawnBoss(string type) {
        StopAllCoroutines();
        
        var requestId = $"Enemy_{type}";

        var enemy = ManagerPoolEnemy.instance.GetPooledObject(requestId);
        if (enemy != null) {
            var spawnPos = new Vector3(
                150.0f,
                0.0f,
                gameObject.transform.position.z
            );
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.Euler(0, -90, 0);
            enemy.GetComponent<Rigidbody>().velocity =
                Vector3.left * enemy.GetComponent<ControllerEnemy>().speed;
            enemy.SetActive(true);
            return enemy;
        }
        
        // Something went wrong
        //throw new TargetException();
        Debug.Log("PROBLEMS!");
        return null;
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

    private IEnumerator EnemySpawn() {
        yield return new WaitForSeconds(_spawnFreq);

        while (true) {
            //if (!GameManager.bossLevel) {
                var enemy = ManagerPoolEnemy.instance.GetPooledObject(_enemyToSpawn);
                if (enemy != null) {
                    enemy.transform.position = gameObject.transform.position;
                    enemy.transform.rotation = Quaternion.Euler(0, -90, 0);
                    enemy.SetActive(true);
                }
            //}
            yield return new WaitForSeconds(_spawnFreq);
        }
    }
}
