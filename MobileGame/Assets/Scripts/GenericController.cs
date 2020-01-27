using UnityEngine;

public abstract class GenericController : MonoBehaviour {
    protected Boundary  _boundary;
    protected Transform _shotSpawn;

    protected void Start() {
        _shotSpawn = transform.Find("ShotSpawn");
    }

    protected abstract void Fire();
}
