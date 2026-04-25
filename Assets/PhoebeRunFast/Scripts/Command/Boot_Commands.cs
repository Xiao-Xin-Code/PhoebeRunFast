using DG.Tweening;
using QMVC;

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

#region 转场命令

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

#endregion


public class InitCharacterCommand : AbstractCommand
{
	int tableId;

	public InitCharacterCommand(int tableId)
	{
		this.tableId = tableId;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new InitCharacterEvent(tableId));
	}
}
