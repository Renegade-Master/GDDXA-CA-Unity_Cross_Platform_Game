using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayerFire : ButtonGeneric {
    private ControllerPlayer _playerController;
    private Button _fireButton;
    
    private void Start() {
        GameObject.Find("GameManager").GetComponent<ManagerGame>().GetPlayer(out _playerController);
        if (_playerController == null) {
            Debug.Log("Player is NULL again: " + _playerController);
            throw new NullReferenceException();
        }
        _fireButton = GetComponent<Button>();
        _fireButton.onClick.AddListener(PlayerFire);
    }

    private void PlayerFire() {
        _playerController.Fire();
    }
}
