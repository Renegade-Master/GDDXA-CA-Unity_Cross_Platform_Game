using System;
using UnityEngine;

public class ControllerEnemySmall : ControllerCharacter {
    protected new void Start() {
        base.Start();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    
    // Search for player
    public void Update() {
        transform.LookAt(Target);

        if (ReadyToShoot()) {
            Fire();
        }
    }

    public void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    public override void Fire() {
        var bullet = ManagerPoolShot.instance.GetPooledObject("Shot_Enemy_Small_Main");
        if (bullet != null) {
            bullet.transform.position = ShotSpawn.position;
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bullet.GetComponent<ControllerProjectile>().speed);
            bullet.SetActive(true);
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        throw new NotImplementedException();
    }
}
