using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerHealth : HudGeneric {
    public GameObject indicatorPrefab;
    private List<GameObject> _health;

    private void Awake() {
        _health = new List<GameObject>();
        for (int i = 0; i < 5; i++) {
            //_health.Add(Instantiate(indicatorPrefab));
        }
    }

    public void AddHealth(int num) {
        _health.Add(indicatorPrefab);
    }
    
    public void RemoveHealth(int num) {
        for (int i = 0; i < num; i++) {
            if (_health.Count > 0) {
                _health.Remove(indicatorPrefab);
            } else {
                // ToDo: Signal Game Over
            }
        }
    }
}
