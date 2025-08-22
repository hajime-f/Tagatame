using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCharacter : MonoBehaviour
{
    public int selectedIndex = 0;
    private TextMeshProUGUI textMesh;
    public GameObject charactorName;    
    public Image characterImage;  // 表示するUIのImage
    public Sprite chack_0;
    public Sprite lockie_0;
    public Sprite mad_0;
    public Sprite komugi_0;
    public Sprite lucky_0;

    void Start()
    {
	textMesh = charactorName.GetComponent<TextMeshProUGUI>();
	textMesh.text = "";
    }
    
    void Update()
    {
	selectedIndex = CharacterIndex.Instance.c_index;

        switch (selectedIndex)
        {
            case 0:
                characterImage.sprite = chack_0;
		textMesh.text = "チャック";
                break;
            case 1:
                characterImage.sprite = lockie_0;
		textMesh.text = "ロッキー";
                break;
            case 2:
                characterImage.sprite = mad_0;
		textMesh.text = "マッド";		
                break;
            case 3:
                characterImage.sprite = komugi_0;
		textMesh.text = "コムギ";		
                break;
            case 4:
                characterImage.sprite = lucky_0;
		textMesh.text = "ラッキー";		
                break;
            default:
                characterImage.sprite = null; // 該当しない場合は非表示にするならnull
                break;
        }	
    }
}
