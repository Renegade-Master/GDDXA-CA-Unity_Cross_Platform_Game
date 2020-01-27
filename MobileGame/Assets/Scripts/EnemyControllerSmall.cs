using UnityEngine;

public class EnemyControllerSmall : GenericController {
    protected new void Start() {
        base.Start();
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<BoundaryManager>().enemyBoundary;
    }

    protected override void Fire() {
        var bullet = ShotManager.instance.GetPooledObject("Shot_Enemy_Small_Main");
        if (bullet != null) {
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = _shotSpawn.rotation;
            bullet.GetComponent<Rigidbody>().velocity = Vector3.left * 50;
            bullet.SetActive(true);
        }
    }
}
