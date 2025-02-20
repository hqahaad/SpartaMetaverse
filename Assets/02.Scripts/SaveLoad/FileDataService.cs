using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileDataService : IDataService
{
    private ISerializer serializer;
    private string dataPath = Application.persistentDataPath;
    private string fileExtension = "json";

    public FileDataService(ISerializer serializer)
    {
        this.serializer = serializer;
    }

    private string GetPathToFile(string fileName)
    {
        return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
    }

    public void Save(GameData data, bool overwrite = true)
    {
        string fileLocation = GetPathToFile(data.name);

        if (!overwrite && File.Exists(fileLocation))
        {
            return;
        }

        File.WriteAllText(fileLocation, serializer.Serialize(data));
    }

    public GameData Load(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (!File.Exists(fileLocation))
        {
            //¼öÁ¤
            GameData newData = new();
            newData.name = name;

            Save(newData, true);

            return newData;
        }

        return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
    }

    public void Remove(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
    }

    public void RemoveAll()
    {
        foreach(var iter in  Directory.GetFiles(dataPath))
        {
            File.Delete(iter);
        }
    }
}
