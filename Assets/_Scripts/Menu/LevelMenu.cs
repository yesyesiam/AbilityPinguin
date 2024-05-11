using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    private IPersistentData _persistentPlayerData;
    private GameInstance _instance;

    public void Initialize(IPersistentData persistentPlayerData)
    {
        _persistentPlayerData = persistentPlayerData;
        _instance = GameInstance._instance;

        UnlockLevels();
    }

    private void UnlockLevels()
    {
        foreach (var b in _buttons)
        {
            b.interactable = false;
        }
        for (int i = 0; i < _persistentPlayerData.PlayerData.UnlockedLevelsCount; i++)
        {
            _buttons[i].interactable = true;
        }
    }

    public void LoadLevel(LevelInfo levelInfo)
    {
        Debug.Log("kekos " + levelInfo.Title);
        _instance.LoadLevel(levelInfo.SceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}