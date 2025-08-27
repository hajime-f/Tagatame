using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    private List<CharacterData> party;
    public TextMeshProUGUI navigationMessage;

    private Coroutine typingCoroutine;
    private AudioSource audioSource;
    public AudioClip typeSound;
    private float delay = 0.05f;
    
    private int turnIndex = 0;
    private bool battleEnded = false;

    void Start()
    {
	audioSource = gameObject.AddComponent<AudioSource>();
	audioSource.volume = 0.3f;
	
        // 前シーンから持ち越したキャラリストをコピー
        party = new List<CharacterData>(CharacterDataLoader.party);

        // 戦闘開始
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop()
    {
        while (!battleEnded)
        {
            yield return StartCoroutine(TakeTurn(party[turnIndex]));

            // 次のキャラへ
            turnIndex = (turnIndex + 1) % party.Count;

            // 勝敗判定
            CheckBattleEnd();
        }

        Debug.Log("戦闘終了！");
    }

    IEnumerator TakeTurn(CharacterData character)
    {
        if (character.hp <= 0)
        {
            yield break; // 戦闘不能ならターンを飛ばす
        }

        // Debug.Log($"{character.name} のターン！");

        // ここで攻撃対象を決める（例: ランダムで他キャラを攻撃）
        CharacterData target = GetRandomTarget(character);

        if (target != null)
        {
            int damage = Random.Range(5, 15);
            target.hp -= damage;

            // Debug.Log($"{character.name} が {target.name} に {damage} ダメージ！ 残りHP: {target.hp}");
        }

        // 演出時間などを確保（0.5秒待機）
        yield return new WaitForSeconds(0.5f);
    }

    CharacterData GetRandomTarget(CharacterData attacker)
    {
        List<CharacterData> candidates = party.FindAll(c => c.hp > 0 && c != attacker);
        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    void CheckBattleEnd()
    {
        // 生存キャラが1人以下なら終了
        int aliveCount = party.FindAll(c => c.hp > 0).Count;
        if (aliveCount <= 1)
        {
            battleEnded = true;
        }
    }

    private void StartTypewriter(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string fullText)
    {
        navigationMessage.text = "";

        foreach (char c in fullText)
        {
            navigationMessage.text += c;

            if (typeSound != null && !char.IsWhiteSpace(c))
                audioSource.PlayOneShot(typeSound);

            yield return new WaitForSeconds(delay);
        }
    }    
}
