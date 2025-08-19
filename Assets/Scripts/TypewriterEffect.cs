using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public enum State
    {
        Typing,         // タイプライター中
        FullDisplayed,  // 全文表示中
        ConfirmReturn,  // 「タイトルに戻りますか？」表示中
        Idle            // 何もしていない
    }

    public State state = State.Idle;

    [TextArea(3, 10)]
    public string[] texts;             // 差し替え用のテキスト群
    public int selectedIndex = 0;      // 表示するテキストの番号
    private int lastSelectedIndex = -1;
    public float delay = 0.05f;        // 1文字表示の間隔
    public AudioClip typeSound;        // タイプ音
    public AudioClip cancelSound;

    private AudioSource audioSource;
    private TextMeshProUGUI textMesh;
    private string fullText;
    private Coroutine typingCoroutine;

    public GameObject fixedJoystick;
    public GameObject bButton;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        audioSource = gameObject.AddComponent<AudioSource>();
	audioSource.volume = 0.3f;
	
        SetTextByIndex(selectedIndex);
        StartTypewriter();
    }

    void Update()
    {
        // selectedIndex の変更を反映
        selectedIndex = fixedJoystick.GetComponent<CharactorSelection>().selectedIndex;

	if (selectedIndex != lastSelectedIndex)
	{
	    SetTextByIndex(selectedIndex);
	    StartTypewriter();
	    lastSelectedIndex = selectedIndex;
	}
	
	// Bボタンの押下を検知
	var bScript = bButton.GetComponent<OnMouseDownShow_B>();
	bool isPressed = bScript.isPressed;

        // キーボードやコントローラーの B ボタン入力
        if (Input.GetButtonDown("Submit") || isPressed)
        {
            OnBButtonPressed();

	    if (isPressed)
	    {
		bScript.isPressed = false;
	    }
        }
    }

    public void OnBButtonPressed()
    {
        switch (state)
        {
            case State.Typing:
                SkipToFullText();
                state = State.FullDisplayed;
                break;

            case State.FullDisplayed:
                // 「タイトルに戻りますか？」表示
                textMesh.text = texts[5]; // texts[5] は確認用テキスト
                state = State.ConfirmReturn;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
                break;

            case State.ConfirmReturn:
                // キャンセルして再度タイプライター
                SetTextByIndex(selectedIndex);
                StartTypewriter();
                state = State.Typing;
                break;
        }
    }

    private void SetTextByIndex(int index)
    {
        if (texts != null && texts.Length > index)
        {
            fullText = texts[index];
            textMesh.text = "";
        }
        else
        {
            Debug.LogWarning("指定されたインデックスのテキストが存在しません。");
        }
    }

    private void StartTypewriter()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
        state = State.Typing;
    }

    private IEnumerator TypeText()
    {
        textMesh.text = "";

        foreach (char c in fullText)
        {
            textMesh.text += c;

            if (typeSound != null && !char.IsWhiteSpace(c))
                audioSource.PlayOneShot(typeSound);

            yield return new WaitForSeconds(delay);
        }

        state = State.FullDisplayed;
    }

    private void SkipToFullText()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textMesh.text = fullText;
    }
}
