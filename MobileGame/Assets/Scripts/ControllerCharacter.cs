using UnityEngine;

public abstract class ControllerCharacter : ControllerGeneric {
    // Google Analytics Tracking
    public GoogleAnalyticsV4 gaFireTracking;
    public GoogleAnalyticsV4 gaTimesHitTracking;

    protected int    HitPoints;
    protected double LastShootTime;

    public    double    shootChance;
    public    double    shootCoolDown;
    protected Transform ShotSpawn;
    public    float     speed;
    protected Transform Target;

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
