using UnityEngine;

public class ControllerEnemyLarge : ControllerEnemy {
    // Google Analytics Tracking
    public GoogleAnalyticsV4 googleAnalytics;
    
    private int     _direction;
    private Vector3 _moveSpeed;

    protected new void Awake() {
        base.Awake();
        _direction = Random.Range(0, 2) * 2 - 1;
        _moveSpeed = new Vector3(0, 0, 0.1f);
    }

    protected void OnEnable() {
        HitPoints = GameManager.enemyHitPoints.largeHp;
    }

    protected new void Start() {
        base.Start();
    }

    // Search for player
    protected new void Update() {
        var position = gameObject.transform.position;
        var pos = position;

        position.Set(
            pos.x,
            0.0f,
            pos.z);

        gameObject.transform.position = position;

        // Has the Enemy been killed?
        if (HitPoints <= 0) gameObject.SetActive(false);

        transform.LookAt(Target);
        if (ReadyToShoot() && InRange()) Fire();
    }

    protected new void FixedUpdate() {
        if (gameObject.GetComponent<Rigidbody>().transform.position.x > 30.0f) {
            base.FixedUpdate();
        } else {
            // Move to the Right Edge of the screen, then hold position moving up/down
            var tempRb = gameObject.GetComponent<Rigidbody>();
            var position = tempRb.position;
            var distanceToEdge = new Vector2(Boundary.zMax - position.z, position.z - Boundary.zMin);

            if (distanceToEdge.x < 3.0f || distanceToEdge.y < 3.0f)
                _direction *= -1;

            tempRb.MovePosition(tempRb.position + _moveSpeed * _direction);

            tempRb.position = new Vector3(
                30,
                0,
                Mathf.Clamp(tempRb.position.z, Boundary.zMin, Boundary.zMax)
            );

            gameObject.transform.position = tempRb.position;
        }
    }

    private bool InRange() {
        if (gameObject.GetComponent<Rigidbody>().transform.position.x < 48.0f
         && gameObject.GetComponent<Rigidbody>().transform.position.x > -34.0f)
            return true;

        return false;
    }

    public override void Fire() {
        foreach (Transform child in gameObject.transform.Find("ShotSpawns"))
            if (child.gameObject.activeSelf) {
                var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Large_Main");
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
