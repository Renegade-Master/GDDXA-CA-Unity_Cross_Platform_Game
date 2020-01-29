using UnityEngine;

public class ControllerPlayer : ControllerCharacter {
    private Vector3 movement;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private Vector2 _touchOrigin = -Vector2.one;
#endif

    protected void Awake() {
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().playerBoundary;
    }

    protected new void Start() {
        base.Start();
    }

    private void Update() {
        GetPlayerInput(out var moveHorizontal, out var moveVertical);

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
    }

    private void FixedUpdate() {
        var tempRb = GetComponent<Rigidbody>();

        tempRb.MovePosition(tempRb.position + movement);

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

	        //Check if the phase of that touch equals Began
	        if (myTouch.phase == TouchPhase.Began) {
		        //If so, set touchOrigin to the position of that touch
		        _touchOrigin = myTouch.position;
	        }

	        //If the touch phase is not Began, and instead is equal to Ended and the x of _touchOrigin is greater or equal to zero:
	        else if (myTouch.phase == TouchPhase.Ended && _touchOrigin.x >= 0) {
		        //Set touchEnd to equal the position of this touch
		        var touchEnd = myTouch.position;

		        //Calculate the difference between the beginning and end of the touch on the x axis.
		        var x = touchEnd.x - _touchOrigin.x;

		        //Calculate the difference between the beginning and end of the touch on the y axis.
		        var y = touchEnd.y - _touchOrigin.y;

		        //Set _touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
		        _touchOrigin.x = -1;

		        //Check if the difference along the x axis is greater than the difference along the y axis.
		        if (Mathf.Abs(x) > Mathf.Abs(y))

			        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
			        mvH = x > 0 ? 1 : -1;
		        else

			        //If y is greater than zero, set horizontal to 1, otherwise set it to -1
			        mvV = y > 0 ? 1 : -1;
	        }
        }
#endif // End Device specific code

        if (Input.GetButton("Fire1")) Fire();
    }

    protected override void Fire() {
        var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Player_Main");
        if (bullet != null) {
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = _shotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.right * 50;
            bullet.SetActive(true);
        }
    }
}
