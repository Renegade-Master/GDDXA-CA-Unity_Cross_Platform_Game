using UnityEngine;

public abstract class ControllerCharacter : ControllerGeneric {
    protected Transform Target;
    protected Transform ShotSpawn;
    protected double LastShootTime;
    protected int HitPoints;
    
    public double shootChance;
    public double shootCoolDown;
    public float speed;
    
    protected new void Start() {
        base.Start();
        
        ShotSpawn = transform.Find("ShotSpawn");
    }

    public abstract void Fire();

    protected abstract void OnTriggerEnter(Collider other);

    public int GetHealth() {
        return HitPoints;
    }

    public bool ReadyToShoot() {
        double timeNow = Time.time;
        if ((timeNow - LastShootTime) >= shootCoolDown) {
            LastShootTime = timeNow;
            return true;
        }

        return false;
    }
}
