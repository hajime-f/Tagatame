using UnityEngine;

public class CharacterIndex : MonoBehaviour
{
    public static CharacterIndex Instance { get; private set; }

    public int c_index = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既に存在していれば破棄
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンを跨いでも破棄されない
    }
}
