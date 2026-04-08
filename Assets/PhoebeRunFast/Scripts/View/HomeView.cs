using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] Button begin;


    event Action onBeginPressed;


	#region Register

	public void RegisterBeginPressed(Action action)
	{
		onBeginPressed += action;
	}

	#endregion



	#region UnRegister

	public void UnRegisterBeginPressed(Action action)
	{
		onBeginPressed -= action;
	}

	#endregion

}
