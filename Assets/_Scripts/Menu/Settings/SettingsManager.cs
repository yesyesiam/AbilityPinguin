using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager
{
    private const string MixerName = "volume";

    private AudioMixer _audioMixer;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentPlayerData;

    public static SettingsManager instance;

    public SettingsManager(IDataProvider dataProvider, IPersistentData persistentPlayerData, 
        AudioMixer audioMixer)
    {
        _dataProvider = dataProvider;
        _persistentPlayerData = persistentPlayerData;
        _audioMixer = audioMixer;

        InitQuality();
    }

    public void InitMasterVolume()
    {
        SetMasterVolume(_persistentPlayerData.PlayerData.Volume);
    }

    private void InitQuality()
    {
        var level = _persistentPlayerData.PlayerData.QualityLevel;
        if (QualitySettings.GetQualityLevel()!= level)
        {
            QualitySettings.SetQualityLevel(level, true);
        } 
    }

    public void ChangeQuality(int qualityLevel)
    {
        if (QualitySettings.GetQualityLevel() != qualityLevel)
        {
            _persistentPlayerData.PlayerData.QualityLevel = qualityLevel;
            QualitySettings.SetQualityLevel(_persistentPlayerData.PlayerData.QualityLevel);
        } 
    }

    public void SetMasterVolume(float volume)
    {
        _persistentPlayerData.PlayerData.Volume = volume;
        _audioMixer.SetFloat(MixerName, Mathf.Log10(volume) * 20);
    }

    public void SaveSettings()
    {
        _dataProvider.Save();
    }

    public int GetQualityLevel => _persistentPlayerData.PlayerData.QualityLevel;
    public float GetVolume => _persistentPlayerData.PlayerData.Volume;
}
