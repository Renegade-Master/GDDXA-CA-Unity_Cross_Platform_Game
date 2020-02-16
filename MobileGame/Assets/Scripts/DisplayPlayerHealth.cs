using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerHealth : HudGeneric {
    private ControllerPlayer _playerController;
    private ManagerGame _gameManager;
    private List<Transform> _health;
    private int _healthPointer;

    private void Awake() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<ManagerGame>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
        _health = new List<Transform>();
        _healthPointer = -1;
        
        foreach (Transform child in transform) {
            print("Foreach loop: " + child);
            _health.Add(child);
            _healthPointer++;
        }
    }

    private void Update() {
        int playerHealth = _playerController.GetHealth();
        Debug.Log("Player has " + playerHealth + " health.");
    }
    
    // ToDo: Make both of these functions dependant on passed in INT to make removing multiple HP at once possible.
    public void AddHealth() {
        _health[_healthPointer++].gameObject.SetActive(true);
    }
    
    public void RemoveHealth() {
        _health[_healthPointer--].gameObject.SetActive(false);
    }
}
