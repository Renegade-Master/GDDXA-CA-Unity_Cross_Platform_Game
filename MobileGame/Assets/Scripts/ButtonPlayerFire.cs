using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayerFire : ButtonGeneric {
    private GameObject _player;
    private ControllerPlayer _playerController;
    private Button _fireButton;
    
    private void Start() {
        GameObject.Find("GameManager").GetComponent<ManagerGame>().GetPlayer(out _playerController);
        if (_player == null) {
            Debug.Log("Player is NULL again: " + _player);
            throw new NullReferenceException();
        }
        _fireButton = GetComponent<Button>();
        _fireButton.onClick.AddListener(PlayerFire);
    }

    private void PlayerFire() {
        _playerController.Fire();
    }
}
