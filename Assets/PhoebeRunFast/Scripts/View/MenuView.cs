using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
	[SerializeField] Button roleBtn;
	[SerializeField] Button backpackBtn;
	[SerializeField] Button battleBtn;
	[SerializeField] Button shopBtn;
	[SerializeField] Button lotteryBtn;


	#region Regiester

	public void RegisterRolePressed(UnityAction action)
	{
		roleBtn.onClick.AddListener(action);
	}

	public void RegisterBackPackPressed(UnityAction action)
	{
		backpackBtn.onClick.AddListener(action);
	}

	public void RegisterBattlePressed(UnityAction action)
	{
		battleBtn.onClick.AddListener(action);
	}

	public void RegisterShopPressed(UnityAction action)
	{
		shopBtn.onClick.AddListener(action);
	}

	public void RegisterLotteryPressed(UnityAction action)
	{
		lotteryBtn.onClick.AddListener(action);
	}

	#endregion


	#region UnRegister

	public void UnRegisterRolePressed(UnityAction action)
	{
		roleBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterBackPackPressed(UnityAction action)
	{
		backpackBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterBattlePressed(UnityAction action)
	{
		battleBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterShopPressed(UnityAction action)
	{
		shopBtn.onClick.RemoveListener(action);
	}

	public void UnRegisterLotteryPressed(UnityAction action)
	{
		lotteryBtn.onClick.RemoveListener(action);
	}

	#endregion
}
