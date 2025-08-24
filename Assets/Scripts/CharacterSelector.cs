using System.Collections.Generic;
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
	NoCharacter,    // 「キャラクターがいません」表示中
	ConfirmProceed, // 「このキャラクターで戦いますか？」表示中
    }
    public State state = State.ListDisplayed;

    [TextArea(3, 10)]
    public string[] texts;
    
    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI textMesh_2;
    public GameObject messageBox;
    public GameObject menuSelector;
    public GameObject navigationMessage;
    public GameObject messageBox_2;
    public GameObject navigationMessage_2;
    public GameObject aButton;
    public GameObject bButton;
    
    private AudioSource audioSource;
    public AudioClip cancelSound;
    public AudioClip selectSound;
    public AudioClip cursorSound;
    public AudioClip readySound;
    
    private int selectedIndex = 0;
    private int maxIndex;
    private int topIndex = 0;
    private bool isStickMoved = false;
    private float rowHeight = 190;

    private List<int> selectedIndices = new List<int>();
    private const int MaxSelectable = 5;
    
    public string nextSceneName_01 = "Opening";
    public string nextSceneName_02 = "Battle";
    private bool isTurning = false;
    
    void Start()
    {
	textMesh = navigationMessage.GetComponent<TextMeshProUGUI>();
	textMesh_2 = navigationMessage_2.GetComponent<TextMeshProUGUI>();
	if (messageBox.activeSelf)
	    messageBox.SetActive(false);
	if (menuSelector.activeSelf)
	    menuSelector.SetActive(false);
	if (navigationMessage.activeSelf) {
	    textMesh.text = "";
	    navigationMessage.SetActive(false);
	}
	if (messageBox_2.activeSelf)
	    messageBox_2.SetActive(false);
	if (navigationMessage_2.activeSelf) {
	    textMesh_2.text = "";
	    navigationMessage_2.SetActive(false);
	}
	
        audioSource = gameObject.AddComponent<AudioSource>();
	audioSource.volume = 0.3f;	
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
	RecalcBounds();

	if (contentParent.childCount == 0)
	{
	    if (!messageBox.activeSelf)
		messageBox.SetActive(true);
	    if (!navigationMessage.activeSelf) {
		textMesh.text = texts[2];
		navigationMessage.SetActive(true);
	    }
	    state = State.NoCharacter;
	    pointer.gameObject.SetActive(false);
	}
	
        float vertical = fixedJoystick.Vertical;
	float horizontal = fixedJoystick.Horizontal;

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

	    if (horizontal > 0.5f && !isStickMoved)
	    {
		if (selectedIndices.Count >= 2)
		{
		    ShowUI(texts[3]);
		    state = State.ConfirmProceed;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
		}
	    }
	    else if (horizontal < -0.5f && !isStickMoved)
	    {
		ShowUI(texts[1]);
		state = State.ConfirmRemove;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);		
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
	if (contentParent == null) return;
	int count = contentParent.childCount;
	if (count == 0)
	{
	    if (pointer != null) pointer.gameObject.SetActive(false);
	    return;
	}
	
	// ビュー内の相対位置
	int pointerPosInView = selectedIndex - topIndex;
	
	RectTransform pointerRect = pointer.rectTransform;
	float yOffset = -pointerPosInView * rowHeight + 380f; // ここはUIに合わせて調整値
	
	pointerRect.localPosition = new Vector3(
	    pointerRect.localPosition.x,
	    yOffset,
	    pointerRect.localPosition.z
	);
    }

    void UpdateVisibleRows()
    {
	if (contentParent == null) return;
	
	int count = contentParent.childCount;
	for (int i = 0; i < count; i++)  // ← i <= maxIndex は使わない
	{
	    bool visible = (i >= topIndex && i < topIndex + visibleCount);
	    var child = contentParent.GetChild(i);
	    if (child != null) child.gameObject.SetActive(visible);
	}
	
	// 行が0ならポインタも隠す
	if (pointer != null) pointer.gameObject.SetActive(count > 0);
    }
    
    public void OnAButtonPressed()
    {
	if (isTurning) return;
	
	var selector = menuSelector.GetComponent<MenuSelector_3>();
	int selectedMenu = selector.selectedMenu;

	switch (state)
	{
	    case State.ListDisplayed:
		ToggleCharacterSelection(selectedIndex);
		if (selectSound != null)
		    audioSource.PlayOneShot(readySound);
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
		    {
			isTurning = true;
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		    }
		}
		break;

	    case State.ConfirmRemove:
		if (selectedMenu == 0)
		{
		    HideUI();
		    state = State.ListDisplayed;
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		else
		{
                    HideUI();
		    RemoveCharacter(selectedIndex);
		    state = State.ListDisplayed;
		    if (selectSound != null)
			audioSource.PlayOneShot(selectSound);
		}
		break;

	    case State.NoCharacter:
		if (selectSound != null)
		{
		    isTurning = true;
		    StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		}
		break;

	    case State.ConfirmProceed:
		if (selectedMenu == 0)
		{
                    HideUI();
		    if (!messageBox_2.activeSelf)
			messageBox_2.SetActive(true);
		    if (!navigationMessage_2.activeSelf) {
			textMesh_2.text = texts[4];
			navigationMessage_2.SetActive(true);
		    }
		    if (selectSound != null)
		    {
			isTurning = true;
			StartCoroutine(FadeOutAndLoadScene(selectSound, nextSceneName_02));
		    }
		}
		else
		{
		    HideUI();
		    state = State.ListDisplayed;
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		break;		
	}
	
    }

    public void OnBButtonPressed()
    {
	if (isTurning) return;
	
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

	    case State.ConfirmRemove:
		HideUI();
		state = State.ListDisplayed;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);
		break;
		
	    case State.NoCharacter:
		if (selectSound != null)
		    StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		break;

	    case State.ConfirmProceed:
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

    private void ToggleCharacterSelection(int index)
    {
	Transform row = contentParent.GetChild(index);
	TextMeshProUGUI text1 = row.Find("CharacterText").GetComponent<TextMeshProUGUI>();
	TextMeshProUGUI text2 = row.Find("HPText").GetComponent<TextMeshProUGUI>();
	TextMeshProUGUI text3 = row.Find("MPText").GetComponent<TextMeshProUGUI>();
	TextMeshProUGUI text4 = row.Find("NBattle").GetComponent<TextMeshProUGUI>();

	if (selectedIndices.Contains(index))
	{
	    // 選択解除
	    selectedIndices.Remove(index);
	    text1.color = Color.white;
	    text2.color = Color.white;
	    text3.color = Color.white;
	    text4.color = Color.white;
	}
	else
	{
	    if (selectedIndices.Count < MaxSelectable)
	    {
		// 新規選択
		selectedIndices.Add(index);
		text1.color = Color.yellow;
		text2.color = Color.yellow;
		text3.color = Color.yellow;
		text4.color = Color.yellow;
	    }
	}
    }

    public void RemoveCharacter(int indexToRemove)
    {
	// セーブデータから削除
	var data = SaveSystem.Load();
	if (data != null && data.characters != null &&
	    indexToRemove >= 0 && indexToRemove < data.characters.Count)
	{
	    data.characters.RemoveAt(indexToRemove);
	    SaveSystem.Save(data);
	}
	
	// 行（UI）を削除
	if (contentParent != null &&
	    indexToRemove >= 0 && indexToRemove < contentParent.childCount)
	{
	    Destroy(contentParent.GetChild(indexToRemove).gameObject);
	}
	
	// インデックス類を再計算して表示更新
	RecalcBounds();
	UpdateVisibleRows();
	UpdatePointerAndContent();
    }
    
    private void RecalcBounds()
    {
	int count = contentParent != null ? contentParent.childCount : 0;
	
	maxIndex = Mathf.Max(0, count - 1);
	selectedIndex = Mathf.Clamp(selectedIndex, 0, maxIndex);
	
	int maxTop = Mathf.Max(0, count - visibleCount);
	topIndex = Mathf.Clamp(topIndex, 0, maxTop);
    }

    // BGM フェードアウト用
    private IEnumerator FadeOutAndLoadScene(AudioClip clip, string sceneName, float fadeTime = 5f)
    {
	// 効果音（Ready / Select）を鳴らす
	if (clip != null)
	    audioSource.PlayOneShot(clip);
	
	// BGM を探す（例：シングルトンの BGMManager から取得しても良い）
	AudioSource bgm = GameObject.Find("BGMManager").GetComponent<AudioSource>();
	if (bgm != null)
	{
	    float startVolume = bgm.volume;
	    
	    // 徐々に音量を下げる
	    float t = 0;
	    while (t < fadeTime)
	    {
		t += Time.deltaTime;
		bgm.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
		yield return null;
	    }
	    
	    bgm.Stop();
	    bgm.volume = startVolume; // 次のシーン用に戻しておく
	}
	
	// 効果音が鳴り終わるまで待機
	if (clip != null)
	    yield return new WaitForSeconds(clip.length);
	
	// シーン遷移
	SceneManager.LoadScene(sceneName);
    }
}
