using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * Class DisplayPlayerHealth inherits from HudGeneric
 *  A Class for interacting with the Player's Health.  Contains
 *  functions for decrementing and incrementing Shields and HitPoints
 */
public class DisplayPlayerHealth : HudGeneric {
    private ManagerGame     _gameManager;
    private List<Transform> _health;
    private int             _healthPointer;

    private double           _lastHitTime;
    private ControllerPlayer _playerController;
    private Slider           _shield;
    public  double           _shieldCoolDown;

    
    /**
     * Function Awake
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     */
    private void Awake() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<ManagerGame>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();

        // Find the Shield Display Object
        _shield = gameObject.transform.Find("PlayerShieldDisplay").transform.Find("Bar_PlayerShield")
                            .GetComponent<Slider>();

        // Prepare and find the Health Display Object(s)
        _health = new List<Transform>();
        _healthPointer = -1;

        foreach (Transform child in gameObject.transform.Find("PlayerHealthDisplay")) {
            _health.Add(child);
            _healthPointer++;
        }
    }

    
    /**
     * Function Update
     *  Runs midway through the current frame after Physics calculations
     *  when the GameObject that this script is attached to is
     *  initialised.
     */
    private void Update() {
        // Replenish the Shield after a cooldown period
        if (Time.time - _lastHitTime >= _shieldCoolDown) _shield.value = _shield.minValue;
    }

    
    /**
     * Function AddHealth
     *  Restore a Health Point to the Player.  Do not increment beyond
     *  the Maximum limit.
     */
    public void AddHealth() {
        // Player cannot increase health beyond set limit
        if (_healthPointer >= _gameManager.GetPlayerMaxHealth() - 1) return;

        // Heal the Player
        _health[++_healthPointer].gameObject.SetActive(true);
        _playerController.Heal();
    }


    /**
     * Function RemoveHealth
     *  Reduce Shield when hit, or reduce Health if Shield is empty.
     *  Shield is funky and works backwards to what one might expect.
     *  MAX is empty, MIN is full
     *
     * @param int - The damage to be taken from the Player Health
     */
    public void RemoveHealth(int damage) {
        _lastHitTime = Time.time;

        // Check if shield is already at 10
        if (_shield.value >= _shield.maxValue) {
            // Check if health-pointer is already at 0 (should not really happen, game should have ended)
            if (_healthPointer < 0) return;


            for (var i = 0; i < damage; i++) {
                _health[_healthPointer--].gameObject.SetActive(false);
                _playerController.Damage();
            }
        }

        // Reduce shields
        for (var i = 0; i < damage; i++) _shield.value++;
    }
}
