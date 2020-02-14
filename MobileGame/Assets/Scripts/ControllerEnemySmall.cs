using UnityEngine;

public class ControllerEnemySmall : ControllerCharacter {
    protected new void Start() {
        base.Start();
        _boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        _target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    
    // Search for player
    public void Update() {
        double timeNow = Time.time;
        
        transform.LookAt(_target);

        if ((timeNow - lastShootTime) >= shootCoolDown) {
            Fire();
            lastShootTime = timeNow;
        }
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    public override void Fire() {
        var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Small_Main");
        if (bullet != null) {
            bullet.transform.position = _shotSpawn.position;
            bullet.transform.rotation = Quaternion.Euler(Vector3.left);
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.left * bullet.GetComponent<ControllerProjectile>().speed);
            bullet.SetActive(true);
        }
    }
}
