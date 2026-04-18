using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
	[SerializeField] RectTransform rectTransform;

    [SerializeField] Button tryAgainBtn;
    [SerializeField] Button exitBtn;


	public RectTransform RectTransform => rectTransform;


	#region Register

	public void RegisterTryAgainPressed(UnityAction action)
	{
		tryAgainBtn.onClick.AddListener(action);
	}

	public void RegisterExitPressed(UnityAction action)
	{
		exitBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	public void UnRegisterTryAgainPressed(UnityAction action)
	{
		tryAgainBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterExitPressed(UnityAction action)
	{
		exitBtn.onClick.RemoveListener(action);
	}

	#endregion



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
			sequence.Append(rectTransform.DOScale(0, 0.25f).SetEase(Ease.Linear));
		}
		return sequence;
	}


}
