using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;



    public void StateInit()
    {
        rectTransform.localScale = Vector3.zero;
    }


    public Sequence ActiveSequence(bool isOpen)
    {
        Sequence sequence = DOTween.Sequence();
        if (isOpen)
        {
			sequence.Append(rectTransform.DOScale(1, 0.25f).SetEase(Ease.OutBack));
		}
        else
        {
			sequence.Append(rectTransform.DOScale(0, 0.25f).SetEase(Ease.OutBack));
		}
        return sequence;
    }
}
