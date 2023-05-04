using System.Collections.Generic;

public class SaveLoadSystem
{
    private static Dictionary<string, object> savedData = new();

    public static object LoadData(string key, object defaultValue)
    {
        if (savedData.ContainsKey(key))
        {
            return savedData[key];
        }
        else
        {
            return defaultValue;
        }
    }
    public static void SaveData(string key, object data)
    {
        if (savedData.ContainsKey(key))
        {
            savedData[key] = data;
        }
        else
        {
            savedData.Add(key, data);
        }
    }
}
