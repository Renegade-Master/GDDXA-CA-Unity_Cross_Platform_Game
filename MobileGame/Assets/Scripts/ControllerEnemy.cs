using UnityEngine;

public abstract class ControllerEnemy : ControllerCharacter {
    protected new void Start() {
        base.Start();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    protected new void Update() {
        base.Update();

        // Has the Enemy been killed?
        if (HitPoints <= 0) gameObject.SetActive(false);

        transform.LookAt(Target);
        if (ReadyToShoot()) Fire();
    }

    protected void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    protected override void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Contains("Shot")) {
            if (other.gameObject.tag.Contains("Enemy")) {
                other.gameObject.SetActive(false);
                return;
            }

            if (HitPoints > 0) HitPoints--;

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
