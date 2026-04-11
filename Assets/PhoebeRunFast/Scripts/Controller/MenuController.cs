using QMVC;
using UnityEngine;

public class MenuController : BaseController
{
	[SerializeField] MenuView _view;


	BindableProperty<Menu> Menu = new BindableProperty<Menu>(global::Menu.Battle);


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

		Menu.RegisterWithOldValue(OnMenuChanged);

	}

	private void OnRolePressed()
	{
		//TODO: 开启Role界面
		Menu.Value = global::Menu.Role;
	}


	private void OnBackPackPressed()
	{
		//TODO: 开启背包界面
		Menu.Value = global::Menu.BackPack;
	}

	private void OnBattlePressed()
	{
		//TODO: 开启战斗界面
		//this.SendCommand(new OpenTransitionCommand(() => _gameModel.Stage.Value = Stage.Game));
		Menu.Value = global::Menu.Battle;
	}

	private void OnShopPressed()
	{
		//TODO: 开启商店界面
		Menu.Value = global::Menu.Shop;
	}

	private void OnLotteryPressed()
	{
		//TODO: 开启抽奖界面
		Menu.Value = global::Menu.Lottery;
	}




	private void OnMenuChanged(Menu oldmenu, Menu newmenu)
	{
		switch (oldmenu)
		{
			case global::Menu.Role:
				this.SendCommand(new RoleMenuActiveCommand(false));
				break;
			case global::Menu.BackPack:
				this.SendCommand(new BackPackActiveCommand(false));
				break;
			case global::Menu.Battle:
				this.SendCommand(new BattleActiveCommand(false));
				break;
			case global::Menu.Shop:
				this.SendCommand(new ShopActiveCommand(false));
				break;
			case global::Menu.Lottery:
				this.SendCommand(new LotteryActiveCommand(false));
				break;
			default:
				break;
		}

		switch (newmenu)
		{
			case global::Menu.Role:
				this.SendCommand(new RoleMenuActiveCommand(true));
				break;
			case global::Menu.BackPack:
				this.SendCommand(new BackPackActiveCommand(true));
				break;
			case global::Menu.Battle:
				this.SendCommand(new BattleActiveCommand(true));
				break;
			case global::Menu.Shop:
				this.SendCommand(new ShopActiveCommand(true));
				break;
			case global::Menu.Lottery:
				this.SendCommand(new LotteryActiveCommand(true));
				break;
			default:
				break;
		}
	}




}
