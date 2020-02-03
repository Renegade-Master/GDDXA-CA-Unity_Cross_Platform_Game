using System;
using UnityEngine;

public class ControllerPlayer : ControllerCharacter {
    private Vector3 _movement;
    private Camera _mainCam;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private Vector2 _touchOrigin = -Vector2.one;
    [SerializeField] private GameObject _touchTarget;
#endif

    // For when the GameObejct is Woken after being set to sleep, or after first activation.
    protected void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().playerBoundary;
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _touchTarget.SetActive(false);
    }

    // For when the GameObejct is instantiated at the game start.
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

        Debug.Log("Moving by: " + _movement);

        tempRb.MovePosition(tempRb.position + _movement);

        // Clamp the Player Position to within the bounds of the screen
        tempRb.position = new Vector3(
            Mathf.Clamp(tempRb.position.x, _boundary.xMin, _boundary.xMax),
            0,
            Mathf.Clamp(tempRb.position.z, _boundary.zMin, _boundary.zMax)
        );

        // Clamp the Player Rotation to within the bounds of the screen
        tempRb.rotation = Quaternion.Euler(
            Mathf.Clamp(tempRb.rotation.x, -95, -85),
            Mathf.Clamp(tempRb.rotation.y, -5, 5),
            Mathf.Clamp(tempRb.rotation.z, 85, 95)
        );

        GetComponent<Rigidbody>().position = tempRb.position;
    }

    // Called by Update to get input from the Player.
    private void GetPlayerInput(out float mvH, out float mvV) {
        mvH = 0.0f;
        mvV = 0.0f;

        // Is the Player using a standard input device
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        mvH = Input.GetAxis("Horizontal");
        mvV = Input.GetAxis("Vertical");

        // If the Player is using a touchscreen input device
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0) {
	        //Store the first touch detected.
	        var myTouch = Input.touches[0];

            /* ToDO:
             *    This isn't working yet.
             *    The idea is to place a Target at the touch location
             *    If the touch moves, move the target too
             *    If the touch is released, set the Target to INACTIVE
             *    While the Target is ACTIVE, pull the Player towards the Target location
             */
            
	        //Check if the phase of that touch equals Began
	        if (myTouch.phase == TouchPhase.Began) {
		        //If so, set touchOrigin to the position of that touch
		        _touchOrigin = myTouch.position;
                
                _touchTarget.GetComponent<Rigidbody>().position = _mainCam.ScreenToWorldPoint(myTouch.position);
                _touchTarget.SetActive(true);
            }
	        
	        else if (myTouch.phase == TouchPhase.Moved) {
                _touchTarget.GetComponent<Rigidbody>().position = _mainCam.ScreenToWorldPoint(myTouch.position);
	        }

	        //If the touch phase is not Began, and instead is equal to Ended and the x of _touchOrigin is greater or equal to zero:
	        else if (myTouch.phase == TouchPhase.Ended/* && _touchOrigin.x >= 0*/) {
		        _touchTarget.SetActive(false);
	        }
        }
#endif // End Device specific code

        if (Input.GetButton("Fire1")) Fire();
    }

    // Called by GetPlayerInput if the Player is requesting to shoot.  Handles Player attacks.
    protected override void Fire() {
        var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Player_Main");
        if (bullet != null) {
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = _shotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
            bullet.SetActive(true);
        }
    }

    // Normalise a value to a different value between a given MAX and MIN.
    private float normalise(float x, float min, float max) {
	    return (max - min) * ((x - min) / (max - min)) + min;
    }
}
