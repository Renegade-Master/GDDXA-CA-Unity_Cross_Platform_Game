public class ManagerPoolEnemy : ManagerPool {
    public static ManagerPoolEnemy instance;

    private void Awake() {
        instance = this;
    }
}
