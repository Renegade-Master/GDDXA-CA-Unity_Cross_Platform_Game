using GameAnalyticsSDK;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[Serializable]
public class EnemyHitPoints {
    public int smallHp, mediumHp, largeHp;
}


/**
 * <summary>
 * Class <c>ManagerGame</c> 
 * <para>
 * The Class for managing the game.  Contains functions and variables
 * for tracking the progress of the game.
 * Inherits from the <c>ManagerGeneric</c> <c>Class</c>
 * </para>
 * <seealso cref="ManagerGeneric"/>
 * </summary>
 */
public class ManagerGame : ManagerGeneric {
    private readonly List<GameObject> _debrisSpawns = new List<GameObject>();
    private readonly List<GameObject> _enemySpawns  = new List<GameObject>();

    // Lock to ensure that Spawn Patterns are only executed once per stage
    private readonly bool[]           _gameManagerLock = {true, false, false, false};
    private readonly List<GameObject> _pickupSpawns    = new List<GameObject>();
    private          GameObject       _boss;
    private          ManagerGeneric   _debrisPoolManager;
    private          int              _enemiesDefeated;
    private          ManagerGeneric   _enemyPoolManager;
    private          ManagerGeneric   _pickupPoolManager;

    private GameObject _playArea;

    private GameObject     _player;
    private ManagerGeneric _shotPoolManager;

    // Used to track when to enter the next game stage
    private double _timeElapsed;

    // {SpawnBoss, FightingBoss}
    public bool[][] bossFight = {
        new[] {false, false}, // Boss 01
        new[] {false, false}  // Boss 02
    };

    public List<GameObject> debrisSpawnPrefabs;
    public EnemyHitPoints   enemyHitPoints;
    public List<GameObject> enemySpawnPrefabs;
    public List<GameObject> pickupSpawnPrefabs;
    public int              playerMaxHealth;

    public List<GameObject> skyBoxes;
    public double           timeForStage01;
    public double           timeForStage02;
    public double           timeForStage03;
    public double           timeForStage04;

    
    /**
     * <summary>
     * Function <c>Start</c> 
     * <para>
     * Runs when the GameObject that this script is attached to is
     * initialised.  Creates the data required to run the game.  Also
     * initialises the connections to the PlayStore and GameAnalytics.
     * </para>
     * </summary>
     */
    private void Start() {
        // Initialise the Game Clock
        _timeElapsed = 0.0;
        _enemiesDefeated = 0;

        #region InitialiseGameAnalytics

        GameAnalytics.Initialize();

        #endregion //InitialiseGameAnalytics

        #region InitialiseGooglePlayGames

        // Create client configuration
        var config = new
                PlayGamesClientConfiguration.Builder()
            .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        // Log the User into their Local Google Account
        Social.localUser.Authenticate(success => { Debug.Log(success ? "Login Success" : "Login Fail"); });

        #endregion //InitialiseGooglePlayGames

        // Place all starting GameObjects
        LoadLevel();
    }
    
    
    /**
     * <summary>
     * Function <c>LoadLevel</c>
     * <para>
     * Load up the data required for the Level
     * </para>
     * </summary>
     */
    private void LoadLevel() {
        GameAnalytics.StartSession();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level_01");

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

    
    /**
     * <summary>
     * Function <c>GetPlayerMaxHealth</c>
     * <para>
     * Return the Maximum permitted amount of Player health.
     * </para>
     * <returns>The maximum amount of Player health.</returns>
     * </summary>
     */
    public int GetPlayerMaxHealth() {
        return playerMaxHealth;
    }

    
    /**
     * <summary>
     * Function <c>IncrementEnemiesDefeated</c>
     * <para>
     * Increment the local counter for the amount of enemies defeated
     * </para>
     * <param name="counter">The number to increment the counter by.
     * The default value is <c>1</c>.</param>
     * </summary>
     */
    public void IncrementEnemiesDefeated(int counter = 1) {
        _enemiesDefeated += counter;

        if (Social.localUser.authenticated) {
            PlayGamesPlatform.Instance.Events.IncrementEvent(GPGSIds.event_enemies_defeated, 1);

            Social.ReportProgress(GPGSIds.achievement_defeated_an_enemy, 100.0f,
                success => {
                    Debug.Log(success
                        ? "Defeated Enemy Success"
                        : "Defeated Enemy Fail");
                });

            PlayGamesPlatform.Instance.IncrementAchievement(
                GPGSIds.achievement_5_enemies_defeated, 1,
                success => {
                    Debug.Log(success
                        ? "Update 5 Enemies Defeated Achievement Success"
                        : "Update 5 Enemies Defeated Achievement Fail");
                });

            PlayGamesPlatform.Instance.IncrementAchievement(
                GPGSIds.achievement_10_enemies_defeated, 1,
                success => {
                    Debug.Log(success
                        ? "Update 10 Enemies Defeated Achievement Success"
                        : "Update 10 Enemies Defeated Achievement Fail");
                });

            PlayGamesPlatform.Instance.IncrementAchievement(
                GPGSIds.achievement_25_enemies_defeated, 1,
                success => {
                    Debug.Log(success
                        ? "Update 25 Enemies Defeated Achievement Success"
                        : "Update 25 Enemies Defeated Achievement Fail");
                });
        }
    }

    
    /**
     * <summary>
     * Function <c>PlayerWins</c>
     * <para>
     * For handling functionality in the event that the User has won the
     * game.
     * </para>
     * </summary>
     */
    private void PlayerWins() {
        // Store the Elapsed time as the User's Score in the Leaderboard
        if (Social.localUser.authenticated) {
            PlayGamesPlatform.Instance.Events.IncrementEvent(GPGSIds.event_total_wins, 1);

            Social.ReportScore(_enemiesDefeated, GPGSIds.leaderboard_enemies_defeated,
                success => {
                    Debug.Log(success
                        ? "Update Enemies Defeated Leaderboard Score Success"
                        : "Update Enemies Defeated Leaderboard Score Fail");
                });
            Social.ReportScore((long) _timeElapsed, GPGSIds.leaderboard_level_complete_time,
                success => {
                    Debug.Log(success
                        ? "Update Time Leaderboard Score Success"
                        : "Update Time Leaderboard Score Fail");
                });
        }
        
        StartCoroutine(WaitTime());

        GameOver();
    }

    
    /**
     * <summary>
     * Function <c>GameOver</c>
     * <para>
     * End the current game session
     * </para>
     * </summary>
     */
    public void GameOver() {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level_01");

        // Show the Achievements and Leaderboards UI
        //Social.ShowAchievementsUI();
        PlayGamesPlatform.Instance.ShowAchievementsUI();
        
        // Wait some time to give the User a chance to look at the Achievements
        StartCoroutine(WaitTime());
        
        //Social.ShowLeaderboardUI();
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
        
        // Wait some time to give the User a chance to look at the Leaderboard
        StartCoroutine(WaitTime());
        
        GameAnalytics.EndSession();

        //Stop playing the scene
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    
    /**
     * <summary>
     * Coroutine <c>GameClock</c>
     * <para>
     * Keep track of how much time has elapsed in the Game.  Also
     * handles transition between the different game stages.
     * </para>
     * <returns>An <c>IEnumerator</c> to re-enter the function.</returns>
     * </summary>
     */
    private IEnumerator GameClock() {
        while (true) {
            // Handle different Game Stages
            if (_timeElapsed < timeForStage01) {
                if (_gameManagerLock[0]) {
                    foreach (var obj in _enemySpawns)
                        obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test);

                    foreach (var obj in _debrisSpawns)
                        obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test);

                    foreach (var obj in _pickupSpawns)
                        obj.GetComponent<ControllerPickupSpawn>().StartMovement(SpawnPatternPickup.Test);

                    _gameManagerLock[0] = false;
                    bossFight[0][0] = true;  // Allow Boss_01 to be spawned
                    bossFight[0][1] = false; // Player is not currently fighting Boss_01
                }
            } else if (_timeElapsed < timeForStage02) {
                // START BOSS FIGHT 01
                // If Boss_01 has not yet been set up, and Player is not currently fighting Boss_01
                if (bossFight[0][0] && !bossFight[0][1]) {
                    // Spawn Boss_01, and stop everything else until it dies
                    _boss = _enemySpawns[0].GetComponent<ControllerEnemySpawn>().SpawnBoss("Large_01");

                    bossFight[0][0] = false; // Prevent Boss_01 setup from running again
                    bossFight[0][1] = true;  // Player is now fighting Boss_01
                }

                // If Boss_01 is alive
                if (_boss.activeInHierarchy) {
                } else if (bossFight[0][1]) {
                    // Player was fighting Boss_01, but just killed it


                    // Boss_01 has been killed
                    bossFight[0][1] = false;    // Player is not currently fighting Boss_01
                    bossFight[1][0] = true;     // Allow Boss_02 setup to run
                    _gameManagerLock[1] = true; // Allow progression to Stage 02

                    if (Social.localUser.authenticated)
                        Social.ReportProgress(GPGSIds.achievement_first_boss_defeated, 100.0f,
                            success => {
                                Debug.Log(success
                                    ? "Player Defeated First Boss Success"
                                    : "Player Defeated First Boss Fail");
                            });
                }

                // END BOSS FIGHT 01

                // Spawn Stage 02 enemies
                if (_gameManagerLock[1]) {
                    foreach (var obj in _enemySpawns)
                        obj.GetComponent<ControllerEnemySpawn>().StartMovement(SpawnPatternEnemy.Test2);

                    foreach (var obj in _debrisSpawns)
                        obj.GetComponent<ControllerDebrisSpawn>().StartMovement(SpawnPatternDebris.Test2);

                    foreach (var obj in _pickupSpawns)
                        obj.GetComponent<ControllerPickupSpawn>().StartMovement(SpawnPatternPickup.Test2);

                    _gameManagerLock[1] = false;
                    _gameManagerLock[2] = true; // Allow progression to Stage 03
                }
            } else if (_timeElapsed < timeForStage03) {
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
                    bossFight[1][0] = true;  // Allow Boss_02 to be spawned
                    bossFight[1][1] = false; // Player is not currently fighting Boss_02

                    _gameManagerLock[3] = true; // Allow progression to Stage 04
                }
            } else if (_timeElapsed < timeForStage04) {
                // START BOSS FIGHT 02
                // If Boss_02 has not yet been set up, and Player is not currently fighting Boss_02
                if (bossFight[1][0] && !bossFight[1][1]) {
                    // Spawn Boss_01, and stop everything else until it dies
                    _boss = _enemySpawns[0].GetComponent<ControllerEnemySpawn>().SpawnBoss("Large_01");

                    bossFight[1][0] = false; // Prevent Boss_02 setup from running again
                    bossFight[1][1] = true;  // Player is now fighting Boss_02
                }

                // If Boss_02 is alive
                if (_boss.activeInHierarchy) {
                } else if (bossFight[1][1]) {
                    // Player was fighting Boss_02, but just killed it

                    // Boss_02 has been killed
                    bossFight[1][1] = false; // Player is not currently fighting Boss_01
                    bossFight[1][0] = true;  // Allow Boss_02 setup to run

                    if (Social.localUser.authenticated)
                        Social.ReportProgress(GPGSIds.achievement_second_boss_defeated, 100.0f,
                            success => {
                                Debug.Log(success
                                    ? "Player Defeated Second Boss Success"
                                    : "Player Defeated Second Boss Fail");
                            });

                    StartCoroutine(WaitTime());

                    // Player has won the game
                    PlayerWins();
                }

                // END BOSS FIGHT 02
            }

            // If Player is not currently fighting any Boss, increment timer
            if (!bossFight[0][1] && !bossFight[1][1]) _timeElapsed += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    
    /**
     * <summary>
     * Coroutine <c>WaitTime</c>
     * <para>
     * Wait a specified amount of time.
     * </para>
     * <param name="timeToWait">The time in <c>seconds</c> to wait.</param>
     * <returns>An <c>IEnumerator</c> to re-enter the function.</returns>
     * </summary>
     */
    public IEnumerator WaitTime(float timeToWait = 5.0f) {
        bool waited = false;

        while (!waited) {
            yield return new WaitForSeconds(timeToWait);
            waited = true;
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
