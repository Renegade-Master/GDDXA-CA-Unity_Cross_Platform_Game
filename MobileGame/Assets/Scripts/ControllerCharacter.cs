using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ControllerCharacter : ControllerGeneric {
    protected Transform _target;
    protected Transform _shotSpawn;
    protected double lastShootTime;
    
    public double shootChance;
    public double shootCoolDown;
    public float speed;
    
    protected void Start() {
        _shotSpawn = transform.Find("ShotSpawn");
    }

    public abstract void Fire();
    
    protected bool IsPointerOverUIObject() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }

        for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++) {
            Touch touch = Input.GetTouch(touchIndex);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                return true;
            }
        }
 
        return false;
    }
}
