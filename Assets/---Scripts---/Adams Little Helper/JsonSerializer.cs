using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using File = System.IO.File;

public static class JsonSerializer
{
    /// <summary>
    /// Loads A Single Item Of Data From a given file path
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="Filepath">The location to load the data from(full directory path)</param>

    public static T LoadSerializedData<T>(string Filepath)
    {
        T DataToReturn = default;

        if (File.Exists(Filepath))
        {
            string DataInFile = File.ReadAllText(Filepath);
            DataToReturn = JsonUtility.FromJson<T>(DataInFile);
        }

        return DataToReturn;
    }

    /// <summary>
    /// Stores A Single Item Of Data
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="DataToStore">A Peice Of Data To Store</param>
    /// <param name="Filepath">The location to store the data (full directory path)</param>
    public static void StoreData<T>(T DataToStore, string Filepath)
    {
        string DataInJson = JsonUtility.ToJson(DataToStore);
        File.WriteAllText(Filepath,DataInJson);
    }

    /// <summary>
    /// Stores A List Of Data
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="DataToStore">A Collection Of Data To Store In A Single Json File</param>
    /// <param name="filepath">The location to store the data (full directory path)</param>
    public static void StoreArrayOfData<T>(T[] DataToStore, string filepath)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var data in DataToStore)
        {
            sb.Append(DataToStore);
        }
        File.WriteAllText(filepath,sb.ToString());
    }
}
