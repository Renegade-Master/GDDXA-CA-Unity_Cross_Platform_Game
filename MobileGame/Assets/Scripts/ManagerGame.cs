using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : ManagerGeneric {
    private GameObject _playArea;
    private ManagerGeneric _enemyPoolManager;
    private ManagerGeneric _debrisPoolManager;
    private ManagerGeneric _shotPoolManager;

    private GameObject _player;
    public  int        currentLevel;
    
    public  List<GameObject> skyBoxes;
    public  List<GameObject> enemySpawnPrefabs;
    public  List<GameObject> debrisSpawnPrefabs;
    private List<GameObject> _enemySpawns  = new List<GameObject>();
    private List<GameObject> _debrisSpawns = new List<GameObject>();

    // Start is called before the first frame update
    private void Start() {
        // Place all starting GameObjects
        LoadLevel(currentLevel);
        
    }

    private void LoadLevel(int level) {
        switch (level) {
            case 0:
                _enemyPoolManager = gameObject.GetComponent<ManagerPoolEnemy>();
                _debrisPoolManager = gameObject.GetComponent<ManagerPoolDebris>();
                _shotPoolManager = gameObject.GetComponent<ManagerPoolShot>();
                
                Instantiate(skyBoxes[level], Vector3.zero, Quaternion.Euler(Vector3.zero));
                _playArea = GameObject.FindWithTag("PlayArea");

                _player = GameObject.FindWithTag("Player");
                _player.GetComponent<Rigidbody>().transform.position = Vector3.zero;
                _player.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(-90, 0, 90);

                foreach (var obj in enemySpawnPrefabs)
                    _enemySpawns.Add(Instantiate(obj,
                        new Vector3(60, 0, 0),
                        Quaternion.Euler(Vector3.zero)));

                foreach (var obj in debrisSpawnPrefabs)
                    _debrisSpawns.Add(Instantiate(obj,
                        new Vector3(60, 0, 0),
                        Quaternion.Euler(Vector3.zero)));

                foreach (var obj in _enemySpawns)
                    obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test);

                foreach (var obj in _debrisSpawns)
                    obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test);
                
                // Start the coroutine to regularly skrink the Pooled GameObjects
                //StartCoroutine(PoolShrinkScheduler());

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

    // private IEnumerator PoolShrinkScheduler() {
    //     yield return new WaitForSeconds(5);
    //
    //     while (gameObject.activeInHierarchy) {
    //         if 
    //     }
    // }
}
