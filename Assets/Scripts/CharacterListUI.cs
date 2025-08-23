using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterListUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform contentParent;
    public Sprite[] characterSprites;

    void Start()
    {
        SaveData saveData = SaveSystem.Load();

        foreach (CharacterData character in saveData.characters)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);

	    switch (character.charId)
	    {
		case 0:
		    row.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>().text = "チャック";
		    break;
		case 1:
		    row.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>().text = "ロッキー";
		    break;
		case 2:
		    row.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>().text = "マッド";
		    break;
		case 3:
		    row.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>().text = "コムギ";
		    break;
		case 4:
		    row.transform.Find("CharacterText").GetComponent<TextMeshProUGUI>().text = "ラッキー";
		    break;
	    }
	    
            row.transform.Find("HPText").GetComponent<TextMeshProUGUI>().text = character.hp.ToString();
            row.transform.Find("MPText").GetComponent<TextMeshProUGUI>().text = character.mp.ToString();
            row.transform.Find("NBattle").GetComponent<TextMeshProUGUI>().text = character.nBattle.ToString();

            Image charImage = row.transform.Find("CharacterImage").GetComponent<Image>();
            if (character.charId >= 0 && character.charId < characterSprites.Length)
            {
                charImage.sprite = characterSprites[character.charId];
            }
        }

	CharacterPointer pointerScript = Object.FindFirstObjectByType<CharacterPointer>();
	pointerScript.Initialize(contentParent);
    }
}
