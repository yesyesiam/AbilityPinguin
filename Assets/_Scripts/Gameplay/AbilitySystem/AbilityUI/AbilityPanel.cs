using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    public event Action<int> OnClickAbility;

    [SerializeField] private RectTransform _widgetContainer;
    [SerializeField] private AbilitySlot _slotWidget;

    private List<AbilitySlot> widgets = new List<AbilitySlot>();

    public void Init(Ability[] abilities)
    {
        for (int i = 0; i < abilities.Length; ++i)
        {
            AbilitySlot widget = Instantiate(_slotWidget, _widgetContainer);

            widget.Init(abilities[i], i);
            widget.OnClick += OnClick;

            widgets.Add(widget);   
        }
    }

    private void OnClick(int index)
    {
        OnClickAbility?.Invoke(index);
    }

    private void OnDestroy()
    {
        foreach (var widget in widgets)
        {
            widget.OnClick -= OnClick;
            Destroy(widget.gameObject);
        }
    }
}
