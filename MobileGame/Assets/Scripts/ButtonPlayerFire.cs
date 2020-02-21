using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayerFire : HudGeneric {
    private Button           _fireButton;
    private ControllerPlayer _playerController;

    private void Start() {
        _playerController = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
        if (_playerController == null) {
            Debug.Log("Player is NULL again: " + _playerController);
            throw new NullReferenceException();
        }

        _fireButton = GetComponent<Button>();
        _fireButton.onClick.AddListener(PlayerFire);
    }

    private void PlayerFire() {
        if (!_playerController.ReadyToShoot()) return;

        _playerController.Fire();
    }
}
