using UnityEngine;

public abstract class ControllerCharacter : ControllerGeneric {
    protected int    HitPoints;
    protected double LastShootTime;

    public    double    shootChance;
    public    double    shootCoolDown;
    protected Transform ShotSpawn;
    public    float     speed;
    protected Transform Target;

    
    /**
     * Function Start
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Overrides the Start function of the ControllerGeneric Class
     */
    protected new void Start() {
        base.Start();

        ShotSpawn = transform.Find("ShotSpawn");
    }

    protected new void Update() {
        base.Update();
    }

    public abstract void Fire();

    protected abstract void OnCollisionEnter(Collision other);

    public int GetHealth() {
        return HitPoints;
    }

    public void Damage() {
        HitPoints--;
    }

    public void Heal() {
        HitPoints++;
    }

    public bool ReadyToShoot() {
        double timeNow = Time.time;
        if (timeNow - LastShootTime >= shootCoolDown) {
            LastShootTime = timeNow;
            return true;
        }

        return false;
    }
}
