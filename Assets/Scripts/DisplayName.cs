using System.Collections;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;             // 差し替え用のテキスト群
    public int selectedIndex = 0;      // 表示するテキストの番号
    private int lastIndex = -1;
    private TextMeshProUGUI textMesh;
    public GameObject fixedJoystick;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
	textMesh.text = texts[selectedIndex];
    }

    // Update is called once per frame
    void Update()
    {
	selectedIndex = fixedJoystick.GetComponent<CharactorSelection>().selectedIndex;

	if (selectedIndex != lastIndex)
	{
            textMesh.text = texts[selectedIndex];
	    lastIndex = selectedIndex;
	}        
    }
}
