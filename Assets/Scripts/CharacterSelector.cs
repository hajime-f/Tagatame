using UnityEngine;
using TMPro;

public class CharacterPointer : MonoBehaviour
{
    public FixedJoystick fixedJoystick;
    public TextMeshProUGUI pointer;
    public Transform contentParent;
    public int visibleCount = 5; // 画面に表示可能な行数

    private AudioSource audioSource;
    public AudioClip cancelSound;
    public AudioClip selectSound;
    public AudioClip cursorSound;
    
    private int selectedIndex = 0;
    private int maxIndex;
    private int topIndex = 0;
    private bool isStickMoved = false;
    private float rowHeight = 190;

    void Start()
    {
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

    void SelectCharacter(int index)
    {
        Debug.Log("Selected character index: " + index);
        // 必要ならここでデータを取得
    }
}
