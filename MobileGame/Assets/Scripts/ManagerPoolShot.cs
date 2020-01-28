public class ManagerPoolShot : ManagerPool {
    public static ManagerPoolShot instance;

    private void Awake() {
        instance = this;
    }
}
