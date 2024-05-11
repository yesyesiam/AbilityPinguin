using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> OnClick;

    [SerializeField] private Image _displayImage;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private TMP_Text _cooldownText;

    private Ability _ability;
    private int _abilityIndex;
    public void SetDisplayImage(Sprite displayImage) => _displayImage.sprite = displayImage;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click on ability: "+_abilityIndex);
        OnClick?.Invoke(_abilityIndex);
    }

    public void Init(Ability ability, int index)
    {
        _abilityIndex = index;
        _ability = ability;
        SetDisplayImage(_ability.DisplayImage);

        _ability.EventChangeCooldownTimer += SetCooldownText;
    }

    public void SetCooldownText(int current, int max)
    {
        if (current <= 0.0f)
        {
            _cooldownText.text = string.Empty;
            _cooldownImage.fillAmount = 0.0f;
        }
        else
        {
            _cooldownText.text = current.ToString();
            _cooldownImage.fillAmount = (float)current / max;
        }
    }

    private void OnDestroy()
    {
        _ability.EventChangeCooldownTimer -= SetCooldownText;
    }
}
