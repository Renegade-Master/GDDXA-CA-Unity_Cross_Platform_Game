using UnityEngine;

public abstract class ControllerCharacter : ControllerGeneric {
    protected Transform _shotSpawn;

    protected void Start() {
        _shotSpawn = transform.Find("ShotSpawn");
    }

    protected abstract void Fire();
}
