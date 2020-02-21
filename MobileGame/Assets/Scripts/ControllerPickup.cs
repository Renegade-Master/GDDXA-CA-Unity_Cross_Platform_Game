using UnityEngine;

public class ControllerPickup : ControllerGeneric {
    private string              _pickupType;
    private ControllerPlayer    _player;
    private DisplayPlayerHealth _playerHealth;
    public  float               speed;

    protected new void Awake() {
        base.Awake();

        _pickupType = gameObject.tag;
        _playerHealth = GameObject.FindWithTag("Display_Player_Health_Shield").GetComponent<DisplayPlayerHealth>();
        _player = GameObject.FindWithTag("Player").GetComponent<ControllerPlayer>();
    }

    protected new void Start() {
        base.Start();
    }

    protected new void Update() {
        base.Update();
    }

    protected void OnCollisionEnter(Collision other) {
        // Only trigger for the Player
        if (other.gameObject.CompareTag("Player")) {
            // ToDo: Give the Player some sort of Bonus for the Pickup
            switch (_pickupType) {
                case "Pickup_Health_Restore":
                    Debug.Log("Player activated Health Restore");
                    _playerHealth.AddHealth();
                    break;
                case "Pickup_Shoot_Speed":
                    Debug.Log("Player activated Shoot Speed");
                    _player.shootCoolDown *= 0.75;
                    break;
                case "Pickup_Shoot_Spread":
                    Debug.Log("Player activated Shoot Spread");
                    foreach (Transform child in _player.gameObject.transform.Find("ShotSpawns"))
                        child.gameObject.SetActive(true);
                    break;
                default:
                    Debug.Log("Player activated DEFAULT_PICKUP");
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
