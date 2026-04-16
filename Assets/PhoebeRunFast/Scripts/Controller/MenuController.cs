using QMVC;
using UnityEngine;

/// <summary>
/// 菜单控制器
/// </summary>
public class MenuController : BaseController
{
	[SerializeField] MenuView _view;

	/// <summary>
	/// 当前菜单状态
	/// </summary>
	BindableProperty<Menu> Menu = new BindableProperty<Menu>(global::Menu.Battle);

	BootModel _bootModel;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_bootModel = this.GetModel<BootModel>();

		// 注册视图事件
		_view.RegisterRolePressed(OnRolePressed);
		_view.RegisterBackPackPressed(OnBackPackPressed);
		_view.RegisterBattlePressed(OnBattlePressed);
		_view.RegisterShopPressed(OnShopPressed);
		_view.RegisterLotteryPressed(OnLotteryPressed);

		// 注册菜单变化事件
		Menu.RegisterWithOldValue(OnMenuChanged);

	}

	/// <summary>
	/// 角色按钮点击事件
	/// </summary>
	private void OnRolePressed()
	{
		Menu.Value = global::Menu.Role;
	}

	/// <summary>
	/// 背包按钮点击事件
	/// </summary>
	private void OnBackPackPressed()
	{
		Menu.Value = global::Menu.BackPack;
	}

	/// <summary>
	/// 战斗按钮点击事件
	/// </summary>
	private void OnBattlePressed()
	{
		if(Menu.Value == global::Menu.Battle)
		{
			// 如果当前是战斗菜单，进入游戏场景
			this.SendCommand(new OpenTransitionCommand(() => _bootModel.Stage.Value = Stage.Game));
		}
		else
		{
			// 否则切换到战斗菜单
			Menu.Value = global::Menu.Battle;
		}
	}

	/// <summary>
	/// 商店按钮点击事件
	/// </summary>
	private void OnShopPressed()
	{
		Menu.Value = global::Menu.Shop;
	}

	/// <summary>
	/// 抽奖按钮点击事件
	/// </summary>
	private void OnLotteryPressed()
	{
		Menu.Value = global::Menu.Lottery;
	}

	/// <summary>
	/// 菜单变化事件
	/// </summary>
	/// <param name="oldmenu">旧菜单</param>
	/// <param name="newmenu">新菜单</param>
	private void OnMenuChanged(Menu oldmenu, Menu newmenu)
	{
		// 关闭旧菜单
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

		// 打开新菜单
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
