using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;             // 差し替え用のテキスト群
    public int selectedIndex = 0;      // 表示するテキストの番号
    private int lastIndex = -1;

    public float delay = 0.05f;        // 1文字表示の間隔
    public AudioClip typeSound;        // タイプ音

    private AudioSource audioSource;   // 再生用
    private TextMeshProUGUI textMesh;
    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public GameObject fixedJoystick;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        audioSource = gameObject.AddComponent<AudioSource>();

        // 初期テキストを設定
        SetTextByIndex(selectedIndex);
    }

    void OnEnable()
    {
        StartTypewriter();
    }

    void Update()
    {
	selectedIndex = fixedJoystick.GetComponent<CharactorSelection>().selectedIndex;

	if (selectedIndex != lastIndex)
	{
	    SetTextByIndex(selectedIndex);
	    StartTypewriter();
	    lastIndex = selectedIndex;
	}
	
        // Aボタンでスキップ（SubmitはデフォルトでキーボードEnter/Space、コントローラーのAに割り当てられることが多い）
        if (isTyping && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.JoystickButton0)))
        {
            SkipToFullText();
        }
    }

    public void SetTextByIndex(int index)
    {
		
        if (texts != null && texts.Length > 0 && index >= 0 && index < texts.Length)
        {
            fullText = texts[selectedIndex];
            textMesh.text = "";
        }
        else
        {
            Debug.LogWarning("指定されたインデックスのテキストが存在しません。");
        }
    }
    
    public void StartTypewriter()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        textMesh.text = "";

        foreach (char c in fullText)
        {
            textMesh.text += c;

            // 文字が空白や改行でなければ音を鳴らす
            if (typeSound != null && !char.IsWhiteSpace(c))
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    private void SkipToFullText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textMesh.text = fullText;
        isTyping = false;
    }
}
