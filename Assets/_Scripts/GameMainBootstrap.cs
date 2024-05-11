using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMainBootstrap : MonoBehaviour
{
    private IDataProvider _dataProvider;
    private IPersistentData _persistentPlayerData;

    [SerializeField] private SettingsManager _settingsManager;
    [SerializeField] private LevelMenu _levelMenu;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private SettingsPanel _settingsPanel;

    private void Awake()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        _persistentPlayerData = new PersistentData();
        _dataProvider = new DataLocalProvider(_persistentPlayerData);

        LoadDataOrInit();

        InitializeSettings();

        _levelMenu.Initialize(_persistentPlayerData);
    }

    private void Start()
    {
        _settingsManager.InitMasterVolume();
    }

    private void InitializeSettings()
    {
        _settingsManager = new SettingsManager(_dataProvider, _persistentPlayerData, _audioMixer);
        _settingsPanel.Initialize(_settingsManager);
    }

    private void LoadDataOrInit()
    {
        if (_dataProvider.TryLoad() == false)
            _persistentPlayerData.PlayerData = new PlayerData();
    }
}
