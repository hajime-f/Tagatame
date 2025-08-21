using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CancelCreateConfirmation : MonoBehaviour
{
    public enum State
    {
        FullDisplayed,  // 全文表示中
        ConfirmReturn,  // 「１つ前のシーンに戻りますか？」表示中
	CharactorConfirmation,  // 「キャラクター制作を完了しますか？」表示中
        Idle            // 何もしていない
    }
    public State state = State.FullDisplayed;

    [TextArea(3, 10)]
    public string[] texts;
    private TextMeshProUGUI textMesh;
    public GameObject messageBox;
    public GameObject navigationMessage;
    public GameObject parameterSelector;
    public GameObject menuSelector;
    public GameObject aButton;
    public GameObject bButton;

    private AudioSource audioSource;
    public AudioClip cancelSound;
    public AudioClip selectSound;    

    public string nextSceneName_01 = "CharactorCreate01";
    public string nextSceneName_02 = "Opening";
    
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
	audioSource.volume = 0.3f;	
    }

    void Update()
    {
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

    public void OnAButtonPressed()
    {
	var selector = menuSelector.GetComponent<MenuSelector_2>();
	int selectedMenu = selector.selectedMenu;

	switch (state)
	{
	    case State.FullDisplayed:
		if (!messageBox.activeSelf)
		    messageBox.SetActive(true);
		if (!menuSelector.activeSelf)
		    menuSelector.SetActive(true);
		if (!navigationMessage.activeSelf) {
		    textMesh.text = texts[1];
		    navigationMessage.SetActive(true);
		}
		state = State.CharactorConfirmation;
		parameterSelector.GetComponent<ParameterSelector>().active = false;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);		
		break;

	    case State.ConfirmReturn:
		if (selectedMenu == 0)
		{
		    if (messageBox.activeSelf)
			messageBox.SetActive(false);
		    if (menuSelector.activeSelf)
			menuSelector.SetActive(false);
		    if (navigationMessage.activeSelf) {
			textMesh.text = "";
			navigationMessage.SetActive(false);
		    }
		    parameterSelector.GetComponent<ParameterSelector>().active = true;
		    state = State.FullDisplayed;
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		else
		{
		    if (selectSound != null)
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_01));
		}
		break;

	    case State.CharactorConfirmation:
		if (selectedMenu == 0)
		{
		    if (messageBox.activeSelf)
			messageBox.SetActive(false);
		    if (menuSelector.activeSelf)
			menuSelector.SetActive(false);
		    if (navigationMessage.activeSelf) {
			textMesh.text = "";
			navigationMessage.SetActive(false);
		    }
		    parameterSelector.GetComponent<ParameterSelector>().active = true;
		    state = State.FullDisplayed;
		    if (cancelSound != null)
			audioSource.PlayOneShot(cancelSound);
		}
		else
		{
		    if (selectSound != null)
			StartCoroutine(PlaySoundAndLoadScene(selectSound, nextSceneName_02));
		}
		break;
	}
    }

    public void OnBButtonPressed()
    {
        switch (state)
	{
	    case State.FullDisplayed:
		if (!messageBox.activeSelf)
		    messageBox.SetActive(true);
		if (!menuSelector.activeSelf)
		    menuSelector.SetActive(true);
		if (!navigationMessage.activeSelf) {
		    textMesh.text = texts[0];
		    navigationMessage.SetActive(true);
		}
		state = State.ConfirmReturn;
		parameterSelector.GetComponent<ParameterSelector>().active = false;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);		
		break;

	    case State.ConfirmReturn:
		if (messageBox.activeSelf)
		    messageBox.SetActive(false);
		if (menuSelector.activeSelf)
		    menuSelector.SetActive(false);
		if (navigationMessage.activeSelf) {
		    textMesh.text = "";
		    navigationMessage.SetActive(false);
		}
		parameterSelector.GetComponent<ParameterSelector>().active = true;
		state = State.FullDisplayed;
		if (cancelSound != null)
		    audioSource.PlayOneShot(cancelSound);		
		break;

	    case State.CharactorConfirmation:
		if (messageBox.activeSelf)
		    messageBox.SetActive(false);
		if (menuSelector.activeSelf)
		    menuSelector.SetActive(false);
		if (navigationMessage.activeSelf) {
		    textMesh.text = "";
		    navigationMessage.SetActive(false);
		}
		parameterSelector.GetComponent<ParameterSelector>().active = true;
		state = State.FullDisplayed;
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
}


