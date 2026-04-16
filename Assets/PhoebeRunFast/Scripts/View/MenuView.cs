using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 菜单视图
/// </summary>
public class MenuView : MonoBehaviour
{
	[SerializeField] Button roleBtn;
	[SerializeField] Button backpackBtn;
	[SerializeField] Button battleBtn;
	[SerializeField] Button shopBtn;
	[SerializeField] Button lotteryBtn;

	#region Regiester

	/// <summary>
	/// 注册角色按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterRolePressed(UnityAction action)
	{
		roleBtn.onClick.AddListener(action);
	}

	/// <summary>
	/// 注册背包按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterBackPackPressed(UnityAction action)
	{
		backpackBtn.onClick.AddListener(action);
	}

	/// <summary>
	/// 注册战斗按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterBattlePressed(UnityAction action)
	{
		battleBtn.onClick.AddListener(action);
	}

	/// <summary>
	/// 注册商店按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterShopPressed(UnityAction action)
	{
		shopBtn.onClick.AddListener(action);
	}

	/// <summary>
	/// 注册抽奖按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterLotteryPressed(UnityAction action)
	{
		lotteryBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	/// <summary>
	/// 注销角色按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterRolePressed(UnityAction action)
	{
		roleBtn.onClick.RemoveListener(action);
	}

	/// <summary>
	/// 注销背包按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterBackPackPressed(UnityAction action)
	{
		backpackBtn.onClick.RemoveListener(action);
	}

	/// <summary>
	/// 注销战斗按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterBattlePressed(UnityAction action)
	{
		battleBtn.onClick.RemoveListener(action);
	}

	/// <summary>
	/// 注销商店按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterShopPressed(UnityAction action)
	{
		shopBtn.onClick.RemoveListener(action);
	}

	/// <summary>
	/// 注销抽奖按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterLotteryPressed(UnityAction action)
	{
		lotteryBtn.onClick.RemoveListener(action);
	}

	#endregion
}
