using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    [SerializeField] Button continueBtn;
    [SerializeField] Button resetBtn;
    [SerializeField] Button exitBtn;



	#region Register

    public void RegisterContinuePressed(UnityAction action)
    {
        continueBtn.onClick.AddListener(action);
    }

    public void RegisterResetPressed(UnityAction action)
    {
        resetBtn.onClick.AddListener(action);
    }

    public void RegisterExitPressed(UnityAction action)
    {
        exitBtn.onClick.AddListener(action);
    }

    #endregion

    #region UnRegister

    public void UnRegisterContinuePressed(UnityAction action)
    {
        continueBtn.onClick.RemoveListener(action);
    }

    public void UnRegisterResetPressed(UnityAction action)
    {
        resetBtn.onClick.RemoveListener(action);
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
