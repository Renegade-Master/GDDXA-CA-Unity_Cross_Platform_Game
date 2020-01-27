public class ManagerEnemy : ManagerPool {
    public static ManagerEnemy instance;

    private void Awake() {
        instance = this;
    }
}
