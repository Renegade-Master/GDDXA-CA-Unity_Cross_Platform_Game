using UnityEngine;

public class GameManager : GenericManager {
    public int currentLevel;
    
    public GameObject playerPrefab;
    public GameObject playAreaPrefab;
    public GameObject[] skyBoxes;
    
    private GameObject _player;
    private GameObject _playArea;
    
    // Start is called before the first frame update
    void Start() {
        // Place all starting GameObjects
        LoadLevel(currentLevel);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void LoadLevel(int level) {
        switch (level) {
            case 0:
                Instantiate(skyBoxes[level], Vector3.zero, Quaternion.Euler(Vector3.zero));
                _playArea = Instantiate(playAreaPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
                
                _player = Instantiate(playerPrefab);
                _player.GetComponent<Rigidbody>().transform.position = Vector3.zero;
                _player.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(-90, 0, 90);
                
                // Spawn some initial Enemies
                for (int i = 0; i < 4; i++) {
                    GameObject enemy = EnemyManager.instance.GetPooledObject("Enemy_Small");
                    if (enemy != null) {
                        enemy.transform.position = new Vector3(60,0,-10 + (10 * i));
                        enemy.GetComponent<Rigidbody>().velocity = Vector3.left * 10;
                        enemy.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(0, -90, 0);
                        enemy.SetActive(true);
                    }
                }
                
                break;
            default:
                return;
        }
    }

    void NextLevel() {
        if (currentLevel >= 1) {
            currentLevel = 0;
        } else {
            currentLevel++;
        }
    }
}
