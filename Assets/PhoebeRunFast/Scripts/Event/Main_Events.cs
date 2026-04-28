
/// <summary>
/// 角色菜单激活事件
/// </summary>
public class RoleMenuActiveEvent
{
	public bool isActive;

	public RoleMenuActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}
}

/// <summary>
/// 背包激活事件
/// </summary>
public class BackPackActiveEvent
{
	public bool isActive;

	public BackPackActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}
}

/// <summary>
/// 战斗激活事件
/// </summary>
public class BattleActiveEvent
{
	public bool isActive;

	public BattleActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}
}

/// <summary>
/// 商店激活事件
/// </summary>
public class ShopActiveEvent
{
	public bool isActive;

	public ShopActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}

}

/// <summary>
/// 抽奖激活事件
/// </summary>
public class LotteryActiveEvent
{
	public bool isActive;

	public LotteryActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}
}

/// <summary>
/// 切换到左侧角色事件
/// </summary>
public class ToLeftRoleEvent
{

}

/// <summary>
/// 切换到右侧角色事件
/// </summary>
public class ToRightRoleEvent
{

}


public class ShowLevelEvent
{
	public string levelType;
	public int[] levels;
	
	public ShowLevelEvent(string levelType, params int[] levels)
	{
		this.levelType = levelType;
		this.levels = levels;
	}

}

public class UpGradeLevelEvent
{
	public string level;

	public UpGradeLevelEvent(string level)
	{
		this.level = level;
	}

}

