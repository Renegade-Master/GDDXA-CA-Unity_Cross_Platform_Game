public class ManagerPoolDebris : ManagerPool {
    public static ManagerPoolDebris instance;

    private void Awake() {
        instance = this;
    }
}
