using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
	[SerializeField] Button roleBtn;
	[SerializeField] Button backpackBtn;
	[SerializeField] Button battleBtn;
	[SerializeField] Button shopBtn;
	[SerializeField] Button lotteryBtn;


	#region event

	event Action onRolePressed;
	event Action onBackPackPressed;
	event Action onBattlePressed;
	event Action onShopPressed;
	event Action onLotteryPressed;

	#endregion


	#region Regiester

	public void RegisterRolePressed(Action action)
	{
		onRolePressed += action;
	}

	public void RegisterBackPackPressed(Action action)
	{
		onBackPackPressed += action;
	}

	public void RegisterBattlePressed(Action action)
	{
		onBattlePressed += action;
	}

	public void RegisterShopPressed(Action action)
	{
		onShopPressed += action;
	}

	public void RegisterLotteryPressed(Action action)
	{
		onLotteryPressed += action;
	}

	#endregion


	#region UnRegister

	public void UnRegisterRolePressed(Action action)
	{
		onRolePressed -= action;
	}

	public void UnRegisterBackPackPressed(Action action)
	{
		onBackPackPressed -= action;
	}

	public void UnRegisterBattlePressed(Action action)
	{
		onBattlePressed -= action;
	}

	public void UnRegisterShopPressed(Action action)
	{
		onShopPressed -= action;
	}

	public void UnRegisterLotteryPressed(Action action)
	{
		onLotteryPressed -= action;
	}

	#endregion
}
