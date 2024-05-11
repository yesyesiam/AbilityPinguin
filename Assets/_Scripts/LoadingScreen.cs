using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] float _showTimer = 0.5f;
    [Header("References")]
    [SerializeField] private Canvas _loadingCanvas;
    [SerializeField] private CanvasGroup _loadingCanvasGroup;
    [SerializeField] private TMP_Text _loadingPercentText;
    [SerializeField] private Image _loadingProgressBar;

    private float _canvasAlphaBlend;

    public void Show()
    {
        _loadingCanvasGroup.alpha = 0.0f;
        _loadingProgressBar.fillAmount = 0.0f;
        _loadingCanvas.enabled = true;

        ChangeCanvasAlphaBlend(1.0f, true);
    }

    public void ChangeLoadingPercent(float percent)
    {
        //_loadingPercentText.text = percent.ToString();
        _loadingProgressBar.fillAmount = percent;
    }

    public void Hide()
    {
        ChangeCanvasAlphaBlend(0.0f, false);
    }

    private void ChangeCanvasAlphaBlend(float andValue, bool isShow)
    {
        DOTween
            .To(() => _canvasAlphaBlend, x => _canvasAlphaBlend = x, andValue, _showTimer)
            .OnUpdate(() =>
            {
                _loadingCanvasGroup.alpha = _canvasAlphaBlend;
            })
            .SetLink(gameObject)
            .OnComplete(() => { _loadingCanvas.enabled = isShow; });


        //_loadingCanvas.enabled = isShow;
    }
}

