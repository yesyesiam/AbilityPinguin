using Newtonsoft.Json;
using UnityEngine;

public class DataLocalProvider : IDataProvider
{
    private const string PlayerDataKey = "PlayerGameData";

    private IPersistentData _persistentData;

    public DataLocalProvider(IPersistentData persistentData) => _persistentData = persistentData;

    //private string SavePath => Application.persistentDataPath;
    //private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");

    public bool TryLoad()
    {
        if (IsDataAlreadyExist(PlayerDataKey) == false)
            return false;
        try
        {
            string saveString = PlayerPrefs.GetString(PlayerDataKey);
            _persistentData.PlayerData = JsonConvert.DeserializeObject<PlayerData>(saveString);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }

    public void Save()
    {
        var jsonString = JsonConvert.SerializeObject(_persistentData.PlayerData, Formatting.None, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        PlayerPrefs.SetString(PlayerDataKey, jsonString);
    }

    private bool IsDataAlreadyExist(string saveKey) => PlayerPrefs.HasKey(saveKey);
}
