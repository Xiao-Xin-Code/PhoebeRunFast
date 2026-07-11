using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    [SerializeField] Button closeBtn;


    public void StateInit()
    {
        rectTransform.localScale = Vector3.zero;
    }


    public Sequence ActiveSequence(bool isOpen)
    {
        Sequence sequence = DOTween.Sequence();
        if (isOpen)
        {
            sequence.OnStart(() => { gameObject.SetActive(true); });
			sequence.Append(rectTransform.DOScale(1, 0.25f).SetEase(Ease.OutBack));
		}
        else
        {
			sequence.Append(rectTransform.DOScale(0, 0.25f).SetEase(Ease.OutBack));
            sequence.OnComplete(() => { gameObject.SetActive(false); });
		}
        return sequence;
    }


    #region Register

    public void RegisterClosePressed(UnityAction action)
    {
        closeBtn.onClick.AddListener(action);
    }

    #endregion

    #region UnRegister

    public void UnRegisterClosePressed(UnityAction action)
    {
        closeBtn.onClick.RemoveListener(action);
    }

    #endregion
}
