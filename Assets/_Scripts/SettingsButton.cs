using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Button _btn;
    private void OnEnable() => _btn.onClick.AddListener(LoadMenu);
    private void OnDisable() => _btn.onClick.RemoveListener(LoadMenu);

    private void LoadMenu()
    {
        GameInstance._instance.LoadMenu();
    }
}
