using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameDataManager
{
    private static string _path { get; set;  }

    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SetPath();
        FileStream file;

        if (File.Exists(_path)) file = File.OpenWrite(_path);
        else file = File.Create(_path);

        formatter.Serialize(file, gameData);
        file.Close();
    }

    public static GameData LoadGameData()
    {
        SetPath();
        FileStream file;
        GameData gameData = new GameData(10, DateTime.UtcNow);

        if (File.Exists(_path))
        {
            file = File.OpenRead(_path);
            BinaryFormatter formatter = new BinaryFormatter();
            gameData = (GameData)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            SaveGameData(gameData);
            LoadGameData();
        }

        return gameData;
    }

    private static void SetPath()
    {
        _path = Application.persistentDataPath + "/gd.ps";
    }
}
