using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayerFire : HudGeneric {
    private Button           _fireButton;
    private ControllerPlayer _playerController;

    private void Start() {
        _playerController = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
        if (_playerController == null) throw new NullReferenceException();

        _fireButton = GetComponent<Button>();
        _fireButton.onClick.AddListener(PlayerFire);

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        _fireButton.gameObject.SetActive(false);
#endif
    }

    private void PlayerFire() {
        if (!_playerController.ReadyToShoot()) return;

        // Increment the User's Shot Count Achievements 
        if (Social.localUser.authenticated) {
            Social.ReportProgress(GPGSIds.achievement_10_shots_fired, 1.0,
                success => { Debug.Log(success ? "Update 10 Shots Fired Success" : "Update 10 Shots Fired Fail"); });
            Social.ReportProgress(GPGSIds.achievement_25_shots_fired, 1.0,
                success => { Debug.Log(success ? "Update 20 Shots Fired Success" : "Update 20 Shots Fired Fail"); });
            Social.ReportProgress(GPGSIds.achievement_50_shots_fired, 1.0,
                success => { Debug.Log(success ? "Update 50 Shots Fired Success" : "Update 50 Shots Fired Fail"); });
        }

        _playerController.Fire();
    }
}
