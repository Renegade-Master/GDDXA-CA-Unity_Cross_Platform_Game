using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerGeneric : MonoBehaviour {
    protected Boundary    Boundary;
    protected ManagerGame GameManager;
    protected Camera      MainCam;

    protected void Start() {
        GameManager = GameObject.FindWithTag("GameController").GetComponent<ManagerGame>();
    }

    protected void Awake() {
        Start();
    }

    protected void Update() {
        var position = gameObject.transform.position;
        var pos      = position;

        position.Set(
            pos.x,
            0.0f,
            pos.z);

        gameObject.transform.position = position;
    }

    // Normalise a value to a different value between a given MAX and MIN.
    protected float Normalise(float x, float min, float max) {
        return (max - min) * ((x - min) / (max - min)) + min;
    }

    protected bool IsPointerOverUIObject() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        for (var touchIndex = 0; touchIndex < Input.touchCount; touchIndex++) {
            var touch = Input.GetTouch(touchIndex);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                return true;
            }
        }

        return false;
    }

    // Convert Screen Coordinates into GameWorld Coordinates
    protected Vector3 ScreenToWorldCoord(Vector2 touchPos) {
        var temp = new Vector3(
            touchPos.x,
            touchPos.y,
            MainCam.transform.position.y
        );
        var temp2 = MainCam.ScreenToWorldPoint(temp);

        return temp2;
    }
}
