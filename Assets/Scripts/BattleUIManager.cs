using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleUIManager : MonoBehaviour
{
    public Transform partyContainer;   // PartyContainerをインスペクタでセット
    public GameObject characterUIPrefab; // CharacterUIプレハブをインスペクタでセット
    public Sprite[] characterSprites;

    void Start()
    {
        foreach (var character in CharacterDataLoader.party)
        {
            GameObject ui = Instantiate(characterUIPrefab, partyContainer);

	    Image charImage = ui.transform.Find("CharacterImage").GetComponent<Image>();
            if (character.charId >= 0 && character.charId < characterSprites.Length)
            {
                charImage.sprite = characterSprites[character.charId];
            }	    
        }
    }
}
