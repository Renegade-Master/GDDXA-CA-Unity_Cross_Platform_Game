using UnityEngine;

public class ControllerEnemyMedium : ControllerEnemy {
    protected new void Awake() {
        base.Awake();
    }

    protected void OnEnable() {
        HitPoints = GameManager.enemyHitPoints.mediumHp;
    }

    protected new void Start() {
        base.Start();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Search for player
    public new void Update() {
        transform.LookAt(Target);

        base.Update();

        if (ReadyToShoot()) Fire();
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    public override void Fire() {
        foreach (Transform child in gameObject.transform.Find("ShotSpawns"))
            if (child.gameObject.activeSelf) {
                var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Medium_Main");
                if (bullet != null) {
                    bullet.transform.position = child.position;
                    bullet.transform.rotation = child.rotation;
                    bullet.GetComponent<Rigidbody>().velocity = child.transform.forward * bullet.GetComponent<ControllerProjectile>().speed;
                    bullet.SetActive(true);
                }
            }
    }
}
