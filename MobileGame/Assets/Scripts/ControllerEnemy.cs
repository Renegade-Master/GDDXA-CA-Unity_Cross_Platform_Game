using UnityEngine;

public abstract class ControllerEnemy : ControllerCharacter {
    
    /**
     * Function Start
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Start function of the ControllerCharacter Class
     */
    protected new void Start() {
        base.Start();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    protected new void Update() {
        base.Update();

        // Has the Enemy been killed?
        if (HitPoints <= 0) {
            // Increment the Enemies Defeated Counter
            GameManager.IncrementEnemiesDefeated();

            gameObject.SetActive(false);
        }

        transform.LookAt(Target);
        if (ReadyToShoot() && InRange()) Fire();
    }

    protected void FixedUpdate() {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * speed);
    }

    private bool InRange() {
        if (gameObject.GetComponent<Rigidbody>().transform.position.x < 37.0f
         && gameObject.GetComponent<Rigidbody>().transform.position.x > -34.0f)
            return true;

        return false;
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
