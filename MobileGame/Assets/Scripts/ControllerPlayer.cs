using UnityEngine;

public class ControllerPlayer : ControllerCharacter {
    private Vector3 _movement;
    private ManagerGame _gameManager;
    private DisplayPlayerHealth _healthDisplay;

    public float springForce;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private Vector2 _touchOrigin = -Vector2.one;
    private SpringJoint _spring;
    public GameObject touchTarget;
#endif

    // For when the GameObject is Woken after being set to sleep, or after first activation.
    protected void Awake() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<ManagerGame>();
        _healthDisplay = GameObject.FindWithTag("Display_Health").GetComponent<DisplayPlayerHealth>();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().playerBoundary;
        MainCam = Camera.main;
        _spring = gameObject.GetComponent<SpringJoint>();
        HitPoints = _gameManager.getPlayerMaxHealth();
        
        _spring.spring = 0.0f;
    }

    // For when the GameObject is instantiated at the game start.
    protected new void Start() {
        base.Start();
    }

    // Called BEFORE THE START of every frame to get the Player's intentions for this frame.
    private void Update() {
        // Has the Player lost the game?
        if (HitPoints <= 0) {
            _gameManager.GameOver();
        }
        
        GetPlayerInput(out var moveHorizontal, out var moveVertical);
        
        _movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    }
    
    // Called just BEFORE THE END of every frame to deal with the physics engine changes, ready for the next frame.
    private void FixedUpdate() {
        var tempRb = GetComponent<Rigidbody>();
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        tempRb.MovePosition(tempRb.position + _movement);
#endif
        // Clamp the Player Position to within the bounds of the screen
        tempRb.position = new Vector3(
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, Boundary.xMin, Boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, Boundary.zMin, Boundary.zMax)
        );
      
        // Clamp the Player Rotation to within reasonable bounds
        tempRb.rotation = Quaternion.Euler(
            Mathf.Clamp(GetComponent<Rigidbody>().rotation.x, -95, -85),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().rotation.z, 85, 95)
        );
        
        GetComponent<Rigidbody>().position = tempRb.position;
    }

    // Called by Update to get input from the Player.
    private void GetPlayerInput(out float mvH, out float mvV) {
        mvH = 0.0f;
        mvV = 0.0f;
        
        // Is the Player using a standard input device
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        mvH = 0.0f;
        mvV = 0.0f;
        
        mvH = Input.GetAxis("Horizontal");
        mvV = Input.GetAxis("Vertical");

        // If the Player is using a touchscreen input device
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE || UNITY_EDITOR
        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0) {
	        //Store the first touch detected.
	        var myTouch = Input.touches[0];

            if (!IsPointerOverUIObject()) {
                //Check if the phase of that touch equals Began
                if (myTouch.phase == TouchPhase.Began) {
                    touchTarget.transform.position = ScreenToWorldCoord(myTouch.position);
                    _spring.spring = springForce;
                } else if (myTouch.phase == TouchPhase.Moved) {
                    touchTarget.transform.position = ScreenToWorldCoord(myTouch.position);
                }

                //If the touch phase is not Began, and instead is equal to Ended
                else if (myTouch.phase == TouchPhase.Ended) {
                    _spring.spring = 0.0f;
                }
            }
        }
#endif // End Device specific code
    }

    // Called by GetPlayerInput if the Player is requesting to shoot.  Handles Player attacks.
    public override void Fire() {
        var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Player_Main");
        if (bullet != null) {
            bullet.transform.position = ShotSpawn.position;
            bullet.transform.rotation = ShotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
            bullet.SetActive(true);
        }
    }
    
    protected override void OnTriggerEnter(Collider other) {
        // If the Player has been shot, but not by themselves.
        if (other.tag.Contains("Shot") && !other.tag.Contains("Player")) {
            Debug.Log("Player has been shot");

            //HitPoints -= other.GetComponent<ControllerProjectile>().power;
            HitPoints -= 1;
            
            // ToDo: Only subtract health if Shields are at 0.
            _healthDisplay.RemoveHealth();
        }
        
        if (other.tag.Contains("Pickup_Health")) {
            Debug.Log("Player has collected a health pickup");

            HitPoints += 1;
            _healthDisplay.AddHealth();
        }
    }
}
