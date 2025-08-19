using UnityEngine;
using TMPro;

public class PreviousTriangle : MonoBehaviour
{
    public int selectedIndex = 0;
    public GameObject fixedJoystick;

    private TextMeshProUGUI menuText;
    private Color originalColor;

    void Start()
    {
        menuText = GetComponent<TextMeshProUGUI>();

        if (menuText != null)
        {
            originalColor = menuText.color; // 元の色を保存
        }
    }

    void Update()
    {
        selectedIndex = fixedJoystick.GetComponent<CharactorSelection>().selectedIndex;

        if (selectedIndex == 0)
        {
            SetAlpha(0f); // 透明
        }
        else
        {
            SetAlpha(1f); // 不透明
        }
    }

    private void SetAlpha(float alpha)
    {
        if (menuText != null)
        {
            Color c = originalColor;
            c.a = alpha; // α値だけ変更
            menuText.color = c;
        }
    }
}
