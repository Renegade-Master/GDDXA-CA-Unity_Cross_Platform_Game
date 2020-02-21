using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : HudGeneric {
    private ManagerGame     _gameManager;
    private List<Transform> _health;
    private int             _healthPointer;

    private double           _lastHitTime;
    private ControllerPlayer _playerController;
    private Slider           _shield;
    public  double           _shieldCoolDown;

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

    private void Update() {
        var playerHealth = _playerController.GetHealth();

        // Replenish the Shield after a cooldown period
        if (Time.time - _lastHitTime >= _shieldCoolDown) _shield.value = _shield.minValue;
    }

    // ToDo: Make both of these functions dependant on passed in INT to make removing multiple HP at once possible.
    public void AddHealth() {
        // Player cannot increase health beyond set limit
        if (_healthPointer >= _gameManager.GetPlayerMaxHealth()) return;

        // Heal the Player
        _health[++_healthPointer].gameObject.SetActive(true);
        _playerController.Heal();
    }

    // Reduce Shield when hit, or reduce Health if Shield is empty
    // Shield is funky and works backwards to what one might expect.  MAX is empty, MIN is full
    public void RemoveHealth(int damage) {
        _lastHitTime = Time.time;

        // Check if shield is already at 10
        if (_shield.value >= _shield.maxValue) {
            // Check if health-pointer is already at 0 (should not really happen, game should have ended)
            if (_healthPointer < 0) return;

            //Debug.Log("Shields Empty.  Decrementing Health");
            for (var i = 0; i < damage; i++) {
                _health[_healthPointer--].gameObject.SetActive(false);
                _playerController.Damage();
            }
        }

        // Reduce shields
        //Debug.Log("Decrementing Shields");
        for (var i = 0; i < damage; i++) _shield.value++;
    }
}
