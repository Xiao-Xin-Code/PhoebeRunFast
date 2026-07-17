using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertyView : BaseController
{
    [SerializeField] RectTransform rectTransform;

    [SerializeField] Button outButton;

    [SerializeField] Text outText;


    public Sequence PropertySequence(bool isOpen)
    {
        Sequence sequence = DOTween.Sequence();
        if (isOpen)
        {
            sequence.Append(rectTransform.DOAnchorPosY(-400, 0.25f));
        }
        else
        {
            sequence.Append(rectTransform.DOAnchorPosY(-1500, 0.25f));
        }
        return sequence;
    }


    public void StateInit()
    {
        rectTransform.anchoredPosition = new Vector2(0, -400);
    }


    public void SetOutState(bool isOut)
    {
        outText.text = isOut ? "已出站" : "出战";
        outButton.interactable = !isOut;
    }


    #region Register

    public void RegisterOutPressed(UnityAction onPressed)
    {
        outButton.onClick.AddListener(onPressed);
    }

    #endregion

    #region UnRegister

    public void UnRegisterOutPressed(UnityAction onPressed)
    {
        outButton.onClick.RemoveListener(onPressed);
    }

    #endregion

}
