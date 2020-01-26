public class EnemyManager : PoolManager {
    public static EnemyManager instance;

    private void Awake() {
        instance = this;
    }
}