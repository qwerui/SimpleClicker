using System.IO;
using UnityEngine;

public class JsonParser
{
    public static void Save(string path, object obj, bool isSync = false)
    {
        string directoryPath = path[..path.LastIndexOf('/')];

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = JsonUtility.ToJson(obj);

        if (isSync)
        {
            File.WriteAllText(path, json);
        }
        else
        {
            File.WriteAllTextAsync(path, json);
        }
    }

    public static T Load<T>(string path)
    {
        string json = File.ReadAllText(path);
        
        return JsonUtility.FromJson<T>(json);
    }
}
