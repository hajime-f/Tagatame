using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterPointer : MonoBehaviour
{    
    public FixedJoystick fixedJoystick;
    public TextMeshProUGUI pointer;
    public Transform contentParent;
    public int visibleCount = 5; // 画面に表示可能な行数

    public enum State
    {
        ListDisplayed,  // リスト表示中
        ConfirmReturn,  // 「タイトル画面に戻りますか？」表示中
	ConfirmRemove,  // 「キャラクターを削除していいですか？」表示中
	ConfirmProceed, // 「このキャラクターで戦いますか？」表示中
    }
    public State state = State.ListDisplayed;

    [TextArea(3, 10)]
    public string[] texts;
    
    private TextMeshProUGUI textMesh;
    public GameObject messageBox;
    public GameObject menuSelector;
    public GameObject navigationMessage;
    public GameObject aButton;
    public GameObject bButton;
    
    private AudioSource audioSource;
    public AudioClip cancelSound;
    public AudioClip selectSound;
    public AudioClip cursorSound;
    
    private int selectedIndex = 0;
    private int maxIndex;
    private int topIndex = 0;
    private bool isStickMoved = false;
    private float rowHeight = 190;

    public string nextSceneName_01 = "Opening";
    
    void Start()
    {
	textMesh = navigationMessage.GetComponent<TextMeshProUGUI>();
	if (messageBox.activeSelf)
	    messageBox.SetActive(false);
	if (menuSelector.activeSelf)
	    menuSelector.SetActive(false);
	if (navigationMessage.activeSelf) {
	    textMesh.text = "";
	    navigationMessage.SetActive(false);
	}
	
        audioSource = gameObject.AddComponent<AudioSource>();
	// audioSource.volume = 0.3f;	
    }    
    
    public void Initialize(Transform content)
    {
        contentParent = content;
        selectedIndex = 0;
        topIndex = 0;

        Canvas.ForceUpdateCanvases();

        maxIndex = contentParent.childCount - 1;

        UpdatePointerAndContent();
        UpdateVisibleRows();
    }

    void Update()
    {
        float vertical = fixedJoystick.Vertical;

	if (state == State.ListDisplayed) {
	    if (vertical > 0.5f && !isStickMoved && selectedIndex > 0)
	    {
		selectedIndex--;
		isStickMoved = true;
		if (selectedIndex < topIndex) topIndex--;
		audioSource.PlayOneShot(cursorSound);
		UpdatePointerAndContent();
		UpdateVisibleRows();
	    }
	    else if (vertical < -0.5f && !isStickMoved && selectedIndex < maxIndex)
	    {
		selectedIndex++;
		isStickMoved = true;
		if (selectedIndex >= topIndex + visibleCount) topIndex++;
		audioSource.PlayOneShot(cursorSound);
		UpdatePointerAndContent();
		UpdateVisibleRows();
	    }
	    
	    if (Mathf.Abs(vertical) < 0.2f) isStickMoved = false;
	}
	
	// Aボタンの押下を検知
	var aScript = aButton.GetComponent<OnMouseDownShow_A>();
	bool a_isPressed = aScript.isPressed;
	
        if (a_isPressed)
        {
            OnAButtonPressed();
	    if (a_isPressed)
		aScript.isPressed = false;
        }

	// Bボタンの押下を検知
	var bScript = bButton.GetComponent<OnMouseDownShow_B>();
	bool b_isPressed = bScript.isPressed;
	
        if (b_isPressed)
        {
            OnBButtonPressed();
	    if (b_isPressed)
		bScript.isPressed = false;
        }        	
    }

    void UpdatePointerAndContent()
    {
	int pointerPosInView = selectedIndex - topIndex;
	RectTransform pointerRect = pointer.rectTransform;
	
	float yOffset = -pointerPosInView * rowHeight + 380;
	
	pointerRect.localPosition = new Vector3(
	    pointerRect.localPosition.x,
	    yOffset,
	    pointerRect.localPosition.z
	);
    }

    void UpdateVisibleRows()
    {
        // topIndex から visibleCount までの行だけアクティブ
        for (int i = 0; i <= maxIndex; i++)
        {
            contentParent.GetChild(i).gameObject.SetActive(i >= topIndex && i < topIndex + visibleCount);
        }
    }

    public void OnAButtonPressed()
    {
	var selector = menuSelector.GetComponent<MenuSelector_3>();
	int selectedMenu = selector.selectedMenu;

	switch (state)
	{
	    case State.ListDisplayed:
		break;
	    
	    case State.ConfirmReturn:
		if (selectedMenu == 0)
		{
		    HideUI();
		    state = State.ListDisplayed;
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		else
		{
		    if (selectSound != null)
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		}
		break;
	}
	
    }

    public void OnBButtonPressed()
    {
	switch (state)
	{
	    case State.ListDisplayed:
		ShowUI(texts[0]);
		state = State.ConfirmReturn;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
		break;

	    case State.ConfirmReturn:
		HideUI();
		state = State.ListDisplayed;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
		break;
	}
    }

    private void HideUI()
    {
	if (messageBox.activeSelf)
	    messageBox.SetActive(false);
	if (menuSelector.activeSelf)
	    menuSelector.SetActive(false);
	if (navigationMessage.activeSelf) {
	    textMesh.text = "";
	    navigationMessage.SetActive(false);
	}
    }    

    private void ShowUI(string text)
    {
	if (!messageBox.activeSelf)
	    messageBox.SetActive(true);
	if (!menuSelector.activeSelf)
	    menuSelector.SetActive(true);
	if (!navigationMessage.activeSelf) {
	    textMesh.text = text;
	    navigationMessage.SetActive(true);
	}
    }            

    private IEnumerator PlaySoundAndLoadScene(AudioClip clip, string sceneName)
    {
	audioSource.PlayOneShot(clip);
	yield return new WaitForSeconds(clip.length); // 音が鳴り終わるまで待機
	SceneManager.LoadScene(sceneName);
    }        
}
