using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : ManagerGeneric {
    private GameObject _playArea;

    private GameObject _player;
    public  int        currentLevel;
    public  GameObject playAreaPrefab;

    public  GameObject       playerPrefab;
    public  List<GameObject> skyBoxes;
    public  List<GameObject> enemySpawnPrefabs;
    public  List<GameObject> debrisSpawnPrefabs;
    private List<GameObject> _enemySpawns = new List<GameObject>();
    private List<GameObject> _debrisSpawns = new List<GameObject>();

    // Start is called before the first frame update
    private void Start() {
        // Place all starting GameObjects
        LoadLevel(currentLevel);
    }

    // Update is called once per frame
    private void Update() {
        
    }

    private void LoadLevel(int level) {
        switch (level) {
            case 0:
                Instantiate(skyBoxes[level], Vector3.zero, Quaternion.Euler(Vector3.zero));
                _playArea = Instantiate(playAreaPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));

                _player = Instantiate(playerPrefab);
                _player.GetComponent<Rigidbody>().transform.position = Vector3.zero;
                _player.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(-90, 0, 90);

                foreach (var obj in enemySpawnPrefabs) {
                    _enemySpawns.Add(Instantiate(obj,
                        new Vector3(60, 0, 0),
                        Quaternion.Euler(Vector3.zero)));
                }

                foreach (var obj in _enemySpawns) {
                    obj.GetComponent<ControllerEnemySpawn>().StartMovement(EnemySpawnPattern.Test);
                }

                // Spawn some Enemies
                // for (var i = 0; i < 4; i++) {
                //     var enemy = ManagerEnemy.instance.GetPooledObject("Enemy_Small");
                //     if (enemy != null) {
                //         Debug.Log("Enemy spawn");
                //         enemy.transform.position = new Vector3(60, 0, -10 + 10 * i);
                //         enemy.GetComponent<Rigidbody>().velocity = Vector3.left * 10;
                //         enemy.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(0, -90, 0);
                //         enemy.SetActive(true);
                //     }
                // }

                break;
            default:
                return;
        }
    }

    private void NextLevel() {
        if (currentLevel >= 1)
            currentLevel = 0;
        else
            currentLevel++;
    }
}
