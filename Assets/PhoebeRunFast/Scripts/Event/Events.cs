

using System.Collections;
using DG.Tweening;

/// <summary>
/// 打开转场事件
/// </summary>
public class OpenTransitionEvent
{
	public TweenCallback action;

	public OpenTransitionEvent(TweenCallback action)
	{
		this.action = action;
	}
}

/// <summary>
/// 关闭转场事件
/// </summary>
public class CloseTransitionEvent
{
	public TweenCallback action;

	public CloseTransitionEvent(TweenCallback action)
	{
		this.action = action;
	}
}

/// <summary>
/// 更新进度事件
/// </summary>
public class UpdateProgressEvent
{
	public float progress;

	public UpdateProgressEvent(float progress)
	{
		this.progress = progress;
	}
}

/// <summary>
/// 更新进度增量事件
/// </summary>
public class UpdateProgressDeltaEvent
{
	public float delta;

	public UpdateProgressDeltaEvent(float delta)
	{
		this.delta = delta;
	}
}

/// <summary>
/// 加载主页事件
/// </summary>
public class LoadHomeEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载主页事件
/// </summary>
public class UnLoadHomeEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 加载主界面事件
/// </summary>
public class LoadMainEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载主界面事件
/// </summary>
public class UnLoadMainEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 加载游戏事件
/// </summary>
public class LoadGameEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载游戏事件
/// </summary>
public class UnLoadGameEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 主页初始化完成事件
/// </summary>
public class HomeInitByTransitionOverEvent
{

}

/// <summary>
/// 主界面初始化完成事件
/// </summary>
public class MainInitByTransitionOverEvent
{

}

/// <summary>
/// 游戏初始化完成事件
/// </summary>
public class GameInitByTransitionOverEvent
{

}

/// <summary>
/// 设置激活事件
/// </summary>
public class SettingActiveEvent
{
	public bool isActive;

	public SettingActiveEvent(bool isActive)
	{
		this.isActive = isActive;
	}
}

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

/// <summary>
/// 小鸟上升事件
/// </summary>
public class BirdUpEvent
{

}