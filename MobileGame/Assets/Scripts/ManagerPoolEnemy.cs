public class ManagerPoolEnemy : ManagerPool {
    public static ManagerPoolEnemy instance;

    
    /**
     * Function Awake
     *  Runs when the GameObject that this script is attached to is
     *  initialised.
     *  Establishes an Instance for this Class.
     */
    private void Awake() {
        instance = this;
    }
}
