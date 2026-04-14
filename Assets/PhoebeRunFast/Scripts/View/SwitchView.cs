using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchView : MonoBehaviour
{
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;


	#region Register

	public void RegisterLeftPressed(UnityAction action)
	{
		leftBtn.onClick.AddListener(action);
	}

	public void RegisterRightPressed(UnityAction action)
	{
		rightBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	public void UnRegisterLeftPressed(UnityAction action)
	{
		leftBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterRightPressed(UnityAction action)
	{
		rightBtn.onClick.RemoveListener(action);
	}

	#endregion



	public Sequence SwitchSequence(bool isOpen)
	{
		Sequence sequence = DOTween.Sequence();
		if (isOpen)
		{
			sequence.Join(leftBtn.GetComponent<RectTransform>().DOAnchorPosX(50, 0.25f));
			sequence.Join(rightBtn.GetComponent<RectTransform>().DOAnchorPosX(-50, 0.25f));
		}
		else
		{
			sequence.Join(leftBtn.GetComponent<RectTransform>().DOAnchorPosX(-150, 0.25f));
			sequence.Join(rightBtn.GetComponent<RectTransform>().DOAnchorPosX(150, 0.25f));
		}
		return sequence;
	}


	public void StateInit()
	{
		leftBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
		rightBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
	}


}
