public class ManagerShot : ManagerPool {
    public static ManagerShot instance;

    private void Awake() {
        instance = this;
    }
}
