using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacters : MonoBehaviour
{
    public static SelectedCharacters Instance { get; private set; }

    public List<int> Indices = new List<int>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
