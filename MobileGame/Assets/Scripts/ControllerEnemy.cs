using UnityEngine;
using Event = GooglePlayGames.BasicApi.Events.Event;

public abstract class ControllerEnemy : ControllerCharacter {
    protected new void Start() {
        base.Start();
        Boundary = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<ManagerBoundary>().enemyBoundary;
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    protected new void Update() {
        base.Update();

        // Has the Enemy been killed?
        if (HitPoints <= 0) {
            if (Social.localUser.authenticated) {
                Social.ReportProgress(GPGSIds.achievement_defeated_an_enemy, 100.0,
                    success => { Debug.Log(success ? "Defeated Enemy Success" : "Defeated Enemy Fail"); });
                Social.ReportProgress(GPGSIds.achievement_5_enemies_defeated, 100.0,
                    success => {
                        Debug.Log(success ? "Update 5 Enemies Defeated Success" : "Update 5 Enemies Defeated Fail");
                    });
                Social.ReportProgress(GPGSIds.achievement_10_enemies_defeated, 100.0,
                    success => {
                        Debug.Log(success ? "Update 10 Enemies Defeated Success" : "Update 10 Enemies Defeated Fail");
                    });
                Social.ReportProgress(GPGSIds.achievement_25_enemies_defeated, 100.0,
                    success => {
                        Debug.Log(success ? "Update 25 Enemies Defeated Success" : "Update 25 Enemies Defeated Fail");
                    });
            }

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
