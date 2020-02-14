using UnityEngine;

public class ControllerPlayer : ControllerCharacter {
    private Vector3 _movement;
    private Camera _mainCam;

    public float springForce;
    public DisplayPlayerHealth healthDisplayObject;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private Vector2 _touchOrigin = -Vector2.one;
    private SpringJoint _spring;
    public GameObject touchTarget;
#endif

    // For when the GameObject is Woken after being set to sleep, or after first activation.
    protected void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().playerBoundary;
        _mainCam = Camera.main;
        _spring = gameObject.GetComponent<SpringJoint>();
        
        _spring.spring = 0.0f;
    }

    // For when the GameObject is instantiated at the game start.
    protected new void Start() {
        base.Start();
    }

    // Called BEFORE THE START of every frame to get the Player's intentions for this frame.
    private void Update() {
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
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, _boundary.xMin, _boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, _boundary.zMin, _boundary.zMax)
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
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = _shotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
            bullet.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("Shot")) {
            Debug.Log("Player has been shot");
            
            healthDisplayObject.RemoveHealth(other.GetComponent<ControllerProjectile>().power);
        }
    }

    // Normalise a value to a different value between a given MAX and MIN.
    private float Normalise(float x, float min, float max) {
	    return (max - min) * ((x - min) / (max - min)) + min;
    }
    
    // Convert Screen Coordinates into GameWorld Coordinates
    private Vector3 ScreenToWorldCoord(Vector2 touchPos) {
        var temp = new Vector3(
            touchPos.x,
            touchPos.y,
            _mainCam.transform.position.y
        );
        var temp2 = _mainCam.ScreenToWorldPoint(temp);

        return temp2;
    }
}
