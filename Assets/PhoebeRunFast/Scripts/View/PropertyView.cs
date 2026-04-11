using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PropertyView : BaseController
{
    [SerializeField] RectTransform rectTransform;


    public Sequence PropertySequence(bool isOpen)
    {
        Sequence sequence = DOTween.Sequence();
        if (isOpen)
        {
			sequence.Append(rectTransform.DOAnchorPosY(0, -300));
		}
        else
        {
			sequence.Append(rectTransform.DOAnchorPosY(0, -1500));
		}
		return sequence;
    }


    public void StateInit()
    {
        rectTransform.anchoredPosition = new Vector2(0, -300);
	}


}
