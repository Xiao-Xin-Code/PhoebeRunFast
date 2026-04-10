using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] Button begin;

	#region Register

	public void RegisterBeginPressed(UnityAction action)
	{
		begin.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	public void UnRegisterBeginPressed(UnityAction action)
	{
		begin.onClick.RemoveListener(action);
	}

	#endregion
}
