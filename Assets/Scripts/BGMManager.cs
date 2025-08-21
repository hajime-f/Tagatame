using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // シーンを跨いで参照するための公開シングルトン
    public static BGMManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        // シングルトン確立
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 2個目以降は破棄
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource 準備
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.35f;
    }

    public void PlayBGM(AudioClip clip, bool restartIfSame = false)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlayBGM: clip is null");
            return;
        }

        // 同じ曲を再生中ならスキップ（再スタートしたいときは restartIfSame = true）
        if (!restartIfSame && audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying) audioSource.Stop();
    }
}
