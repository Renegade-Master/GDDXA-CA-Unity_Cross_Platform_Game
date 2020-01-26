public class ShotManager : PoolManager {
    public static ShotManager instance;

    private void Awake() {
        instance = this;
    }
}