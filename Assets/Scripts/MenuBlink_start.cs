using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuBlink_start : MonoBehaviour
{
    public GameObject aButton;
    public GameObject triangle;
    public float interval = 0.1f;
    public int blinkCount = 7;
    public string nextSceneName = "Charactorselection";
    
    private Coroutine blinkingCoroutine;
    private TextMeshProUGUI menuText;
    private Color originalColor;

    void Start()
    {
        menuText = GetComponent<TextMeshProUGUI>();
        if (menuText != null)
        {
            originalColor = menuText.color;
        }
    }

    void Update()
    {
        bool isSelected = aButton.GetComponent<OnMouseDownShow_A>().isSelected;
	int idx = triangle.GetComponent<Selector>().selectedIndex;

        if (isSelected && blinkingCoroutine == null && idx == 1)
        {
            blinkingCoroutine = StartCoroutine(BlinkRoutine());
        }
        else if (!isSelected && blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = null;
            menuText.color = originalColor; // 元の色に戻す
        }
    }

    private IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetAlpha(0f);
            yield return new WaitForSeconds(interval);

            SetAlpha(1f);
            yield return new WaitForSeconds(interval);
        }

	SceneManager.LoadScene(nextSceneName);
    }

    private void SetAlpha(float alpha)
    {
        if (menuText != null)
        {
            Color c = originalColor;
            c.a = alpha;
            menuText.color = c;
        }
    }
}
