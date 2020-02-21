using System.Collections;
using UnityEngine;

public enum SpawnPatternEnemy {
    Test,
    Test2,
    Test3
}

public class ControllerEnemySpawn : ControllerGeneric {
    private int     _direction;
    private Vector3 _moveSpeed;
    private float   _spawnFreq;

    protected new void Awake() {
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        _direction = Random.Range(0, 2) * 2 - 1;
    }

    public void StartMovement(SpawnPatternEnemy pattern) {
        StopAllCoroutines();
        switch (pattern) {
            case SpawnPatternEnemy.Test:
                _spawnFreq = 5.0f;
                _moveSpeed = new Vector3(0, 0, 0.33f);
                break;
            case SpawnPatternEnemy.Test2:
                _spawnFreq = 3.0f;
                _moveSpeed = new Vector3(0, 0, 0.66f);
                break;
            case SpawnPatternEnemy.Test3:
                _spawnFreq = 1.0f;
                _moveSpeed = new Vector3(0, 0, 1.0f);
                break;
            default:
                _spawnFreq = 0.0f;
                _moveSpeed = Vector3.zero;
                return;
        }

        StartCoroutine(Movement());
        StartCoroutine(EnemySpawn());
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
            var enemy = ManagerPoolEnemy.instance.GetPooledObject("Enemy_Small");
            if (enemy != null) {
                enemy.transform.position = gameObject.transform.position;
                enemy.transform.rotation = Quaternion.Euler(0, -90, 0);

                //enemy.GetComponent<Rigidbody>().AddForce(Vector3.left * 20);
                enemy.SetActive(true);
            }

            yield return new WaitForSeconds(_spawnFreq);
        }
    }
}
