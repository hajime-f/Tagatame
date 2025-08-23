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

	Canvas.ForceUpdateCanvases();
	
	maxIndex = contentParent.childCount - 1;
	UpdatePointerPosition();
    }    

    void Update()
    {
        float vertical = this.fixedJoystick.Vertical;

        if (vertical > 0.5f && !this.isStickMoved && selectedIndex > 0)
        {
            selectedIndex--;
	    this.isStickMoved = true;
            UpdatePointerPosition();
        }
        else if (vertical < -0.5f && !this.isStickMoved && selectedIndex < maxIndex)
        {
            selectedIndex++;
	    this.isStickMoved = true;
            UpdatePointerPosition();
        }

	if (Mathf.Abs(vertical) < 0.2f)
	{
	    this.isStickMoved = false;
	}
    }

    public int visibleRowCount = 5;   // 画面に表示できる行数
    private int topIndex = 0;         // 現在表示している最上段の行インデックス
    
    void UpdatePointerPosition()
    {
	// もし選択行が表示範囲を超えたらスクロール
	if (selectedIndex < topIndex)
	{
	    topIndex = selectedIndex;
	}
	else if (selectedIndex > topIndex + visibleRowCount - 1)
	{
	    topIndex = selectedIndex - visibleRowCount + 1;
	}
	
	// スクロールの反映: contentParent の位置を調整
	float rowHeight = 200f; // 行の高さ。環境に合わせて調整
	Vector3 contentPos = contentParent.localPosition;
	contentPos.y = topIndex * rowHeight;
	contentParent.localPosition = contentPos;
	
	// ポインタの位置を「表示領域の中の相対インデックス」に合わせる
	int relativeIndex = selectedIndex - topIndex;
	RectTransform pointerRect = pointer.rectTransform;
	
	Vector3 localPos = pointerRect.localPosition;
	localPos.y = -relativeIndex * rowHeight - 150;
	pointerRect.localPosition = localPos;
    }
    
    
    void SelectCharacter(int index)
    {
        Debug.Log("Selected character index: " + index);
        // 必要ならここでデータを取得
    }
}
