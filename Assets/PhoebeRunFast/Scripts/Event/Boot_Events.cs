using DG.Tweening;

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

#region 转场事件

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

#endregion


public class InitCharacterEvent
{
	public int tableId;

	public InitCharacterEvent(int tableId)
	{
		this.tableId = tableId;
	}
}