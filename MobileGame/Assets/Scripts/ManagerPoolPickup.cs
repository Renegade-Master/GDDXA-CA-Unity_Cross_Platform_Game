public class ManagerPoolPickup : ManagerPool {
    public static ManagerPoolPickup instance;

    private void Awake() {
        instance = this;
    }
}
