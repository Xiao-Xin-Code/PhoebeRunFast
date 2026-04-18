using QMVC;

/// <summary>
/// 角色菜单激活命令
/// </summary>
public class RoleMenuActiveCommand : AbstractCommand
{
	bool isActive;

	public RoleMenuActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new RoleMenuActiveEvent(isActive));
	}
}

/// <summary>
/// 背包激活命令
/// </summary>
public class BackPackActiveCommand : AbstractCommand
{
	bool isActive;

	public BackPackActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new BackPackActiveEvent(isActive));
	}
}

/// <summary>
/// 战斗激活命令
/// </summary>
public class BattleActiveCommand : AbstractCommand
{
	bool isActive;

	public BattleActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new BattleActiveEvent(isActive));
	}
}

/// <summary>
/// 商店激活命令
/// </summary>
public class ShopActiveCommand : AbstractCommand
{
	bool isActive;

	public ShopActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new ShopActiveEvent(isActive));
	}
}

/// <summary>
/// 抽奖激活命令
/// </summary>
public class LotteryActiveCommand : AbstractCommand
{
	bool isActive;

	public LotteryActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	/// <summary>
	/// 执行命令
	/// </summary>
	protected override void OnExecute()
	{
		this.SendEvent(new LotteryActiveEvent(isActive));
	}
}

/// <summary>
/// 切换到左侧角色命令
/// </summary>
public class ToLeftRoleCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<ToLeftRoleEvent>();
	}
}

/// <summary>
/// 切换到右侧角色命令
/// </summary>
public class ToRightRoleCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<ToRightRoleEvent>();
	}
}