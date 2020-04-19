using UnityEngine;

public class ControllerEnemyMedium : ControllerEnemy {
    
    /**
     * Function Awake
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Awake function of the ControllerEnemy Class
     */
    protected new void Awake() {
        base.Awake();
    }

    protected void OnEnable() {
        HitPoints = GameManager.enemyHitPoints.mediumHp;
    }

    
    /**
     * Function Start
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Start function of the ControllerEnemy Class
     */
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
                var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Medium_Main");
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
