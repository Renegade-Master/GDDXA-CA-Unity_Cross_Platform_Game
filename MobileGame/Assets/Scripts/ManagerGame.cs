using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class EnemyHitPoints {
    public int smallHp, mediumHp, largeHp, bossHp;
}

public class ManagerGame : ManagerGeneric {
    private readonly List<GameObject> _debrisSpawns = new List<GameObject>();
    private readonly List<GameObject> _enemySpawns  = new List<GameObject>();

    // Lock to ensure that Spawn Patterns are only executed once per stage
    private readonly bool[]           _gameManagerLock = {true, false, false};
    
    // {SpawnBoss, FightingBoss}
    public bool[][] bossFight = {
        new [] {false, false}, // Level 01 Boss
        new [] {false, false}, // Level 02 Boss (currently not used)
        new [] {false, false}, // Level 03 Boss
    };
    private readonly List<GameObject> _pickupSpawns    = new List<GameObject>();
    private          ManagerGeneric   _debrisPoolManager;
    private          ManagerGeneric   _enemyPoolManager;
    private          ManagerGeneric   _pickupPoolManager;

    private GameObject _playArea;

    private GameObject     _player;
    private ManagerGeneric _shotPoolManager;
    private GameObject _boss;

    // Used to track when to enter the next game stage
    private double           _timeElapsed;
    public  List<GameObject> debrisSpawnPrefabs;
    public  EnemyHitPoints   enemyHitPoints;
    public  List<GameObject> enemySpawnPrefabs;
    public  List<GameObject> pickupSpawnPrefabs;
    public  int              playerMaxHealth;

    public List<GameObject> skyBoxes;
    public double           timeForStage01;
    public double           timeForStage02;
    public double           timeForStage03;
    public double timeForStage04;

    // Start is called before the first frame update
    private void Start() {
        // Initialise the Game Clock
        _timeElapsed = 0.0;

        // Place all starting GameObjects
        LoadLevel();
    }

    // Initialise the Game World ready for a new level
    private void LoadLevel() {
        _debrisPoolManager = gameObject.GetComponent<ManagerPoolDebris>();
        _enemyPoolManager = gameObject.GetComponent<ManagerPoolEnemy>();
        _pickupPoolManager = gameObject.GetComponent<ManagerPoolPickup>();
        _shotPoolManager = gameObject.GetComponent<ManagerPoolShot>();

        Instantiate(skyBoxes[0], Vector3.zero, Quaternion.Euler(Vector3.zero));
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

        foreach (var obj in pickupSpawnPrefabs)
            _pickupSpawns.Add(Instantiate(obj,
                new Vector3(20, 0, 0),
                Quaternion.Euler(Vector3.zero)));

        // Start a coroutine to track how much time has elapsed
        StartCoroutine(GameClock());
    }

    // Returns the Maximum permitted Player Health
    public int GetPlayerMaxHealth() {
        return playerMaxHealth;
    }

    // End the Game
    public void GameOver() {
        Debug.Log("The game has ended.");

        //Stop playing the scene
        Application.Quit();
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;
#endif
    }

    // Keep track of how much time has elapsed in the Game
    // Also handles the different game stages
    private IEnumerator GameClock() {
        while (true) {
            // Handle different Game Stages
            if (_timeElapsed < timeForStage01) {
                if (_gameManagerLock[0]) {
                    Debug.Log("Entering Stage 01");
                    
                    foreach (var obj in _enemySpawns)
                        obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test);

                    foreach (var obj in _debrisSpawns)
                        obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test);

                    foreach (var obj in _pickupSpawns)
                        obj.GetComponent<ControllerPickupSpawn>().StartMovement(SpawnPatternPickup.Test);

                    _gameManagerLock[0] = false;
                    bossFight[0][0] = true; // Allow Boss_01 to be spawned
                    bossFight[0][1] = false; // Player is not currently fighting Boss_01
                }
            } else if (_timeElapsed < timeForStage02) {
                // If Boss_01 has not yet been set up, and Player is not currently fighting Boss_01
                if (bossFight[0][0] && !bossFight[0][1]) {
                    Debug.Log("Spawning Boss 01");
                    
                    // Spawn Boss_01, and stop everything else until it dies
                    _boss = _enemySpawns[0].GetComponent<ControllerEnemySpawn>().SpawnBoss("Large_01");
                    
                    bossFight[0][0] = false; // Prevent Boss_01 setup from running again
                    bossFight[0][1] = true; // Player is now fighting Boss_01
                }

                // If Boss_01 is alive
                if (_boss.activeInHierarchy) {
                    Debug.Log("Player Fighting Boss 01");
                    
                } else if(bossFight[0][1]) {
                    // Player was fighting Boss, but just killed it
                    
                    Debug.Log("Boss 01 has been killed");

                    // Boss_01 has been killed
                    bossFight[0][1] = false; // Player is not currently fighting Boss_01
                    bossFight[2][0] = true; // Allow Boss_02 setup to run
                    _gameManagerLock[1] = true; // Allow progression to Stage 02
                }
                
                // Spawn Stage 02 enemies
                if (_gameManagerLock[1]) {
                    Debug.Log("Entering Stage 02");
                    
                    foreach (var obj in _enemySpawns)
                        obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test2);

                    foreach (var obj in _debrisSpawns)
                        obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test2);

                    foreach (var obj in _pickupSpawns)
                        obj.GetComponent<ControllerPickupSpawn>().StartMovement(SpawnPatternPickup.Test2);

                    _gameManagerLock[1] = false;
                    _gameManagerLock[2] = true;
                }
            } else if (_timeElapsed < timeForStage03) {
                Debug.Log("Entering Stage 03");
                
                if (_gameManagerLock[2]) {
                    foreach (var obj in _enemySpawns)
                        obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test3);

                    foreach (var obj in _debrisSpawns)
                        obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test3);

                    foreach (var obj in _pickupSpawns) {
                        obj.GetComponent<ControllerPickupSpawn>().StartMovement(SpawnPatternPickup.Test3);
                        obj.GetComponent<ControllerPickupSpawn>().SpawnPickup("Shoot_Spread");
                    }

                    _gameManagerLock[2] = false;
                    _gameManagerLock[3] = true;
                }
            } else if (_timeElapsed < timeForStage04) {
                
            }

            // If Player is not currently fighting any Boss, increment timer
            if(!bossFight[0][1] && !bossFight[1][1] && !bossFight[2][1]) _timeElapsed += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Shrink the Object Poolers if they are holding more items than necessary
    // private IEnumerator PoolShrinkScheduler() {
    //     yield return new WaitForSeconds(5);
    //
    //     while (gameObject.activeInHierarchy) {
    //         if 
    //     }
    // }
}
