using UnityEngine;
using TMPro;

public class CharacterPointer : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public TextMeshProUGUI pointer;
    public Transform contentParent;

    private int selectedIndex = 0;
    private int maxIndex;
    private bool isStickMoved = false;

    public void Initialize(Transform content)
    {
	contentParent = content;
	selectedIndex = 0;
	maxIndex = contentParent.childCount - 1;
	UpdatePointerPosition();
    }    

    void Update()
    {
        float vertical = this.fixedJoystick.Vertical;

        if (vertical > 0.5f && !this.isStickMoved)
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
	    this.isStickMoved = true;
            UpdatePointerPosition();
        }
        else if (vertical < -0.5f && !this.isStickMoved)
        {
            selectedIndex = Mathf.Min(maxIndex, selectedIndex + 1);
	    this.isStickMoved = true;
            UpdatePointerPosition();
        }

	if (Mathf.Abs(vertical) < 0.2f)
	{
	    this.isStickMoved = false;
	}
    }

    void UpdatePointerPosition()
    {
	RectTransform targetRow = contentParent.GetChild(selectedIndex).GetComponent<RectTransform>();
	RectTransform pointerRect = pointer.rectTransform;
	
	// pointer を Content の子にしている前提
	Vector3 localPos = pointerRect.localPosition;
	localPos.y = targetRow.localPosition.y; // Y座標だけ合わせる
	pointerRect.localPosition = localPos;
    }
        
    void SelectCharacter(int index)
    {
        Debug.Log("Selected character index: " + index);
        // 必要ならここでデータを取得
    }
}
