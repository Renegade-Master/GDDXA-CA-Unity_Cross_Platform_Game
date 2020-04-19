using UnityEngine;
using UnityEngine.SocialPlatforms;


/**
 * Class ControllerPickup inherits from ControllerGeneric
 *  A Class for managing the game.  Contains functions and variables for
 *  tracking the progress of the game
 */
public class ControllerPickup : ControllerGeneric {
    private string              _pickupType;
    private ControllerPlayer    _player;
    private DisplayPlayerHealth _playerHealth;
    public  float               speed;

    
    /**
     * Function Awake
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     */
    protected new void Awake() {
        base.Awake();

        _pickupType = gameObject.tag;
        _playerHealth = GameObject.FindWithTag("Display_Player_Health_Shield").GetComponent<DisplayPlayerHealth>();
        _player = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
    }

    
    /**
     * Function Start
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Start function from the ControllerGeneric Class
     */
    protected new void Start() {
        base.Start();
    }

    
    /**
     * Function Update
     *  Runs midway through the current frame after Physics calculations
     *  when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Update function from the ControllerGeneric Class
     */
    protected new void Update() {
        base.Update();
    }

    
    /**
     * Function OnCollisionEnter
     *  Activates when the Collision mesh of "this" Object intersects
     *  the Collision mesh of "that" Object.
     *
     * @param Collision - The Collision mesh for the "other" Object
     */
    protected void OnCollisionEnter(Collision other) {
        // Only trigger for the Player
        if (other.gameObject.CompareTag("Player")) {
            // ToDo: Give the Player some sort of Bonus for the Pickup
            switch (_pickupType) {
                case "Pickup_Health_Restore":

                    _playerHealth.AddHealth();
                    break;
                case "Pickup_Shoot_Speed":

                    _player.shootCoolDown *= 0.75;

                    if (Social.localUser.authenticated)
                        Social.ReportProgress(GPGSIds.achievement_more_faster, 100.0f,
                            success => {
                                Debug.Log(success ? "Shot Speed Pickup Success" : "Shot Speed Pickup Fail");
                            });
                    break;
                case "Pickup_Shoot_Spread":

                    foreach (Transform child in _player.gameObject.transform.Find("ShotSpawns"))
                        child.gameObject.SetActive(true);

                    if (Social.localUser.authenticated)
                        Social.ReportProgress(GPGSIds.achievement_more_wider, 100.0f,
                            success => {
                                Debug.Log(success ? "Shot Spread Pickup Success" : "Shot Spread Pickup Fail");
                            });
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
