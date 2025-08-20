using System.Collections;
using UnityEngine;

public class SceneBGMController : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private void OnEnable()
    {
        StartCoroutine(PlayBGMWhenReady());
    }

    private IEnumerator PlayBGMWhenReady()
    {
        // BGMManager が生成されるまで待機
        while (BGMManager.Instance == null)
        {
            yield return null;
        }
        BGMManager.Instance.PlayBGM(bgm);
    }
}
