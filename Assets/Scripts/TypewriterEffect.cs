using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public enum State
    {
        Typing,         // タイプライター中
        FullDisplayed,  // 全文表示中
        ConfirmReturn,  // 「タイトルに戻りますか？」表示中
	CharactorConfirmation,  // 「このキャラクターを選択しますか？」表示中
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
    public AudioClip selectSound;

    private AudioSource audioSource;
    private TextMeshProUGUI textMesh;
    private string fullText;
    private Coroutine typingCoroutine;

    public string nextSceneName_01 = "Opening";
    public string nextSceneName_02 = "CharactorCreate02";
    
    public GameObject fixedJoystick;
    public GameObject aButton;
    public GameObject bButton;
    public GameObject menuSelector;

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

	// Aボタンの押下を検知
	var aScript = aButton.GetComponent<OnMouseDownShow_A>();
	bool a_isPressed = aScript.isPressed;
	
        // キーボードやコントローラーの A ボタン入力
        if (a_isPressed)
        {
            OnAButtonPressed();

	    if (a_isPressed)
	    {
		aScript.isPressed = false;
	    }
        }

	// Bボタンの押下を検知
	var bScript = bButton.GetComponent<OnMouseDownShow_B>();
	bool b_isPressed = bScript.isPressed;
	
        // キーボードやコントローラーの B ボタン入力
        if (b_isPressed)
        {
            OnBButtonPressed();

	    if (b_isPressed)
	    {
		bScript.isPressed = false;
	    }
        }
    }

    public void OnAButtonPressed()
    {
	var selector = menuSelector.GetComponent<MenuSelector>();
	int selectedMenu = selector.selectedMenu;

        switch (state)
        {
            case State.Typing:
                SkipToFullText();
                state = State.FullDisplayed;
                break;

            case State.FullDisplayed:
		// 「このキャラクターでいいですか？」表示
		textMesh.text = texts[6];
                state = State.CharactorConfirmation;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
                break;

            case State.ConfirmReturn:
		if (selectedMenu == 0) {
		    // キャンセルして再度タイプライター
		    SetTextByIndex(selectedIndex);
		    StartTypewriter();
		    state = State.Typing;		    
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		else
		{
		    // オープニングに戻る
		    if (selectSound != null)
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		}
                break;

	    case State.CharactorConfirmation:
		if (selectedMenu == 0) {
		    // オープニングに戻る
		    if (selectSound != null)
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_02));
		}
		else
		{
		    // キャンセルして再度タイプライター
		    SetTextByIndex(selectedIndex);
		    StartTypewriter();
		    state = State.Typing;		    
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}				
		break;
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
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
                break;

	    case State.CharactorConfirmation:
                // キャンセルして再度タイプライター
                SetTextByIndex(selectedIndex);
                StartTypewriter();
                state = State.Typing;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
		break;
        }
    }

    private IEnumerator PlaySoundAndLoadScene(AudioClip clip, string sceneName)
    {
	audioSource.PlayOneShot(clip);
	yield return new WaitForSeconds(clip.length); // 音が鳴り終わるまで待機
	SceneManager.LoadScene(sceneName);
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
