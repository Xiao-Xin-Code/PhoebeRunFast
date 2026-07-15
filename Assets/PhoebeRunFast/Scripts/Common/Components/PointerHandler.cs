using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Event

    event Action<PointerEventData> onPointerDownEvent;
    event Action<PointerEventData> onPointerUpEvent;

    #endregion


    #region Register

    public void RegisterPointerDownEvent(Action<PointerEventData> onPointerDown)
    {
        onPointerDownEvent += onPointerDown;
    }

    public void RegisterPointerUpEvent(Action<PointerEventData> onPointerUp)
    {
        onPointerUpEvent += onPointerUp;
    }

    #endregion

    #region Unregister

    public void UnregisterPointerDownEvent(Action<PointerEventData> onPointerDown)
    {
        onPointerDownEvent -= onPointerDown;
    }

    public void UnregisterPointerUpEvent(Action<PointerEventData> onPointerUp)
    {
        onPointerUpEvent -= onPointerUp;
    }

    #endregion



    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        onPointerDownEvent?.Invoke(eventData);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        onPointerUpEvent?.Invoke(eventData);
    }
}