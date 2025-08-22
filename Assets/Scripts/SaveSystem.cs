using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    // データを保存
    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // 見やすい整形JSON
        File.WriteAllText(savePath, json);
    }

    // データを読み込み
    public static SaveData Load()
    {
        if (!File.Exists(savePath))
        {
            return new SaveData(); // ファイルが無ければ空のデータ返す
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
