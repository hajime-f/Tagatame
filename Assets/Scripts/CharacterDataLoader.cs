using System.Collections.Generic;
using UnityEngine;

public class CharacterDataLoader : MonoBehaviour
{
    public static List<CharacterData> party;
    
    void Start() {
	// セーブデータ全体をロード
	SaveData data = SaveSystem.Load();
	
	// 選択されたキャラのリストを取得
	 party = new List<CharacterData>();

	foreach (int idx in SelectedCharacters.Instance.Indices) {
	    CharacterData charData = data.characters[idx];
	    party.Add(charData);
	}
    }    
}
