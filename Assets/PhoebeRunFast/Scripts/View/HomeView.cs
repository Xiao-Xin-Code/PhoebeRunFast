using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] Button begin;
	[SerializeField] Button setBtn;

	#region Register

	public void RegisterBeginPressed(UnityAction action)
	{
		begin.onClick.AddListener(action);
	}

	public void RegisterSetPressed(UnityAction action)
	{
		setBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	public void UnRegisterBeginPressed(UnityAction action)
	{
		begin.onClick.RemoveListener(action);
	}

	public void UnRegisterSetPressed(UnityAction action)
	{
		setBtn.onClick.RemoveListener(action);
	}

	#endregion
}
