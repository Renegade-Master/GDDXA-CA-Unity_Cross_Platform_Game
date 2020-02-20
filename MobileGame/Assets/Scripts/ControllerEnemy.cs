using UnityEngine;

public abstract class ControllerEnemy : ControllerCharacter {
    protected new void Start() {
        base.Start();
    }
    
    protected void Update() {
        // Has the Enemy been killed?
        if (HitPoints <= 0) {
            gameObject.SetActive(false);
        }
    }
    
    protected override void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("Shot") && !other.tag.Contains("Enemy")) {
            //Debug.Log("Enemy has been shot");
            Debug.Log("Enemy has been shot by " + other.name);
            
            if (HitPoints > 0) {
                HitPoints--;
            }
            
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(Vector3.zero);
            other.gameObject.SetActive(false);
        }
    }
}
