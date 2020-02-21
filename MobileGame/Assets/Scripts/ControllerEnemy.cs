using UnityEngine;

public abstract class ControllerEnemy : ControllerCharacter {
    protected new void Start() {
        base.Start();
    }

    protected new void Update() {
        base.Update();

        // Has the Enemy been killed?
        if (HitPoints <= 0) gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Contains("Shot") && !other.gameObject.tag.Contains("Enemy")) {
            if (HitPoints > 0) HitPoints--;

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
