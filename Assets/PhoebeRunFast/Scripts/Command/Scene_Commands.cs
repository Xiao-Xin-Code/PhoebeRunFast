using QMVC;
using System.Collections;

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