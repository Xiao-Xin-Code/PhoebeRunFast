using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BirdInputView : MonoBehaviour,
	IPointerDownHandler, IPointerUpHandler
{

	event Action<PointerEventData> onPointerDown;
	event Action<PointerEventData> onPointerUp;



	public void OnPointerDown(PointerEventData eventData)
	{
		onPointerDown?.Invoke(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		onPointerUp?.Invoke(eventData);
	}



	#region Register

	public void RegisterPointerDown(Action<PointerEventData> action)
	{
		onPointerDown += action;
	}

	public void RegisterPointerUp(Action<PointerEventData> action)
	{
		onPointerUp += action;
	}

	#endregion

	#region UnRegister

	public void UnRegisterPointerDown(Action<PointerEventData> action)
	{
		onPointerDown -= action;
	}

	public void UnRegisterPointerUp(Action<PointerEventData> action)
	{
		onPointerUp -= action;
	}

	#endregion

}
