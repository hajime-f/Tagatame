using UnityEngine;
using TMPro;

public class MenuSelection : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;
    private TextMeshProUGUI textMesh;
    public GameObject bButton;
    public int selectedIndex = 0;
    
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        var bScript = bButton.GetComponent<OnMouseDownShow_B>();
	if (bScript.isReturned)
	{
            textMesh.text = texts[selectedIndex];
	} 
	if (bScript.isCanceled_return) {
	    textMesh.text = "";
	    bScript.isCanceled_return = false;
	}
    }
}
