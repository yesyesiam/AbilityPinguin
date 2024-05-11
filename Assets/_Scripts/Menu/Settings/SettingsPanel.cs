using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Slider _volumeSlider;

    private SettingsManager _settingsManager;
    public void Initialize(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;

        _dropdown.value = _settingsManager.GetQualityLevel;
        _volumeSlider.value = _settingsManager.GetVolume;

        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        _saveButton.onClick.AddListener(OnSave);
        _volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        Debug.Log("selected index: " + index);
        _settingsManager.ChangeQuality(index);
    }

    private void OnVolumeSliderChanged(float value)
    {
        _settingsManager.SetMasterVolume(value);
    }

    private void OnSave()
    {
        gameObject.SetActive(false);
        _settingsManager.SaveSettings();
    }

    private void OnDestroy()
    {
        _dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        _saveButton.onClick.RemoveListener(OnSave);
        _volumeSlider.onValueChanged.RemoveListener(OnVolumeSliderChanged);
    }
}
