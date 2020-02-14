using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayerFire : ButtonGeneric {
    private ControllerPlayer _playerController;
    private Button _fireButton;
    
    private void Start() {
        //_playerController = GameObject.Find("GameManager").GetComponent<ManagerGame>().GetPlayer();
        _playerController = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
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
