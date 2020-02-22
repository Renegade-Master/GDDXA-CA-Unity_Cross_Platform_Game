using UnityEngine;

public class ControllerEnemySmall : ControllerEnemy {
    protected new void Awake() {
        base.Awake();
    }

    protected void OnEnable() {
        HitPoints = GameManager.enemyHitPoints.smallHp;
    }

    protected new void Start() {
        base.Start();
    }

    // Search for player
    protected new void Update() {
        base.Update();
    }

    protected new void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Fire() {
        foreach (Transform child in gameObject.transform.Find("ShotSpawns"))
            if (child.gameObject.activeSelf) {
                var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Small_Main");
                if (bullet != null) {
                    bullet.transform.position = child.position;
                    bullet.transform.rotation = child.rotation;
                    bullet.GetComponent<Rigidbody>().velocity =
                        child.transform.forward * bullet.GetComponent<ControllerProjectile>().speed;
                    bullet.SetActive(true);
                }
            }
    }
}
