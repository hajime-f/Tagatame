using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<CharacterData> characters = new List<CharacterData>();

    public SaveData()
    {
        if (characters == null) characters = new List<CharacterData>();
    }
}
