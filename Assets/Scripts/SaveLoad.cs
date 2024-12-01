using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    public static int HighScore = 0;

    public static void Save(int highScore)
    {
        HighScore = Math.Max(HighScore, highScore);
        var bf = new BinaryFormatter();
        var file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, HighScore);
        file.Close();
    }

    public static int Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            HighScore = (int) bf.Deserialize(file);
            file.Close();
        }

        return HighScore;
    }
}