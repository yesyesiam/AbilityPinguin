using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmCastPanel : MonoBehaviour
{
    public UnityEvent onApplyButtonClicked;
    public UnityEvent onCancelButtonClicked;

    [SerializeField] private Button btnYes;
    [SerializeField] private Button btnNo;

    public void Init()
    {
        btnYes.interactable = false;
        
        btnYes.onClick.AddListener(ApplyButtonClicked);
        btnNo.onClick.AddListener(CancelButtonClicked);
        HidePanel();
    }

    private void ApplyButtonClicked()
    {
        onApplyButtonClicked?.Invoke();
        HidePanel();
    }
    private void CancelButtonClicked()
    {
        onCancelButtonClicked?.Invoke();
        HidePanel();
    }

    public void ShowPanel(bool interactable)
    {
        btnYes.interactable = interactable;
        gameObject.SetActive(true);
    }

    private void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btnNo.onClick.RemoveListener(CancelButtonClicked);
        btnYes.onClick.RemoveListener(ApplyButtonClicked);
    }
}
