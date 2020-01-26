using System.Collections;
using UnityEngine;

public class GameManager : GenericManager {
    public int currentLevel;
    
    public GameObject playerPrefab;
    public GameObject playAreaPrefab;
    
    public GameObject[] skyBoxes;
    public GameObject player;
    public GameObject playArea;
    
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
                playArea = Instantiate(playAreaPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
                player = Instantiate(playerPrefab);
                
                player.GetComponent<Rigidbody>().transform.position = Vector3.zero;
                player.GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(-90, 0, 90);
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
