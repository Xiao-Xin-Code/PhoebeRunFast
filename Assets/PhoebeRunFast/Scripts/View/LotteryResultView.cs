

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LotteryResultView : MonoBehaviour
{
    public PointerHandler pointerHandler;

    [SerializeField] RectTransform resultParent;
    [SerializeField] RectTransform startRect;

    [SerializeField] Text overText;

    [SerializeField] Button skipBtn;

    [SerializeField] RectTransform[] points;

    public RectTransform ResultParent => resultParent;
    public RectTransform StartRect => startRect;

    public RectTransform[] Points => points;


    public void SetSkipActive(bool active)
    {
        skipBtn.gameObject.SetActive(active);
    }

    public void SetOverActive(bool active)
    {
        overText.gameObject.SetActive(active);
    }

    public bool GetOverActive()
    {
        return overText.gameObject.activeSelf;
    }


    #region Register
    
    public void RegisterSkipPressed(UnityAction action)
    {
        skipBtn.onClick.AddListener(action);
    }

    public void RegisterPointerDownEvent(Action<PointerEventData> onPointerDown)
    {
        pointerHandler.RegisterPointerDownEvent(onPointerDown);
    }

    public void RegisterPointerUpEvent(Action<PointerEventData> onPointerUp)
    {
        pointerHandler.RegisterPointerUpEvent(onPointerUp);
    }

    #endregion

    #region UnRegister

    public void UnRegisterSkipPressed(UnityAction action)
    {
        skipBtn.onClick.RemoveListener(action);
    }

    public void UnRegisterPointerDownEvent(Action<PointerEventData> onPointerDown)
    {
        pointerHandler.UnregisterPointerDownEvent(onPointerDown);
    }

    public void UnRegisterPointerUpEvent(Action<PointerEventData> onPointerUp)
    {
        pointerHandler.UnregisterPointerUpEvent(onPointerUp);
    }

    #endregion
}