using System;
using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class MenuController : BaseController
{
	[SerializeField] MenuView _view;


	GameModel _gameModel;

	protected override void OnInit()
	{
		base.OnInit();

		_gameModel = this.GetModel<GameModel>();

		_view.RegisterRolePressed(OnRolePressed);
		_view.RegisterBackPackPressed(OnBackPackPressed);
		_view.RegisterBattlePressed(OnBattlePressed);
		_view.RegisterShopPressed(OnShopPressed);
		_view.RegisterLotteryPressed(OnLotteryPressed);

	}





	private void OnRolePressed()
	{
		//TODO: 开启Role界面
	}


	private void OnBackPackPressed()
	{
		//TODO: 开启背包界面
	}

	private void OnBattlePressed()
	{
		//TODO: 开启战斗界面
		this.SendCommand(new OpenTransitionCommand(() => _gameModel.Stage.Value = Stage.Game));
	}

	private void OnShopPressed()
	{
		//TODO: 开启商店界面
	}

	private void OnLotteryPressed()
	{
		//TODO: 开启抽奖界面
	}

}
