using System.Collections;
using DG.Tweening;
using QMVC;

/// <summary>
/// 打开转场命令
/// </summary>
public class OpenTransitionCommand : AbstractCommand
{
	private TweenCallback action;

	public OpenTransitionCommand(TweenCallback action)
	{
		this.action = action;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new OpenTransitionEvent(action));
	}
}

/// <summary>
/// 关闭转场命令
/// </summary>
public class CloseTransitionCommand : AbstractCommand
{
	private TweenCallback action;

	public CloseTransitionCommand(TweenCallback action)
	{
		this.action = action;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new CloseTransitionEvent(action));
	}
}

/// <summary>
/// 更新进度命令
/// </summary>
public class UpdateProgressCommand : AbstractCommand
{
	private float progress;

	public UpdateProgressCommand(float progress)
	{
		this.progress = progress;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateProgressEvent(progress));
	}
}

/// <summary>
/// 更新进度增量命令
/// </summary>
public class UpdateProgressDeltaCommand : AbstractCommand
{
	private float delta;

	public UpdateProgressDeltaCommand(float delta)
	{
		this.delta = delta;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateProgressDeltaEvent(delta));
	}
}

/// <summary>
/// 加载主页命令
/// </summary>
public class LoadHomeCommand : AbstractCommand<IEnumerator>
{
	protected override IEnumerator OnExecute()
	{
		LoadHomeEvent loadHomeEvent = new LoadHomeEvent();
		this.SendEvent<LoadHomeEvent>();
		yield return loadHomeEvent.enumerator;
	}
}

/// <summary>
/// 卸载主页命令
/// </summary>
public class UnLoadHomeCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadHomeEvent>();
	}
}

/// <summary>
/// 加载主界面命令
/// </summary>
public class LoadMainCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<LoadMainEvent>();
	}
}

/// <summary>
/// 卸载主界面命令
/// </summary>
public class UnLoadMainCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadMainEvent>();
	}
}

/// <summary>
/// 加载游戏命令
/// </summary>
public class LoadGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<LoadGameEvent>();
	}
}

/// <summary>
/// 卸载游戏命令
/// </summary>
public class UnLoadGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<UnLoadGameEvent>();
	}
}

/// <summary>
/// 设置激活命令
/// </summary>
public class SettingActiveCommand : AbstractCommand
{
	bool isActive;

	public SettingActiveCommand(bool isActive)
	{
		this.isActive = isActive;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SettingActiveEvent(isActive));
	}
}

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

/// <summary>
/// 小鸟上升命令
/// </summary>
public class BirdUpCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<BirdUpEvent>();
	}
}