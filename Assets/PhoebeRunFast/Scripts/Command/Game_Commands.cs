using QMVC;

/// <summary>
/// 暂停游戏命令
/// </summary>
public class PauseGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		GlobalSystem globalSystem = this.GetSystem<GlobalSystem>();
		globalSystem.GameSingleton.GameEntity.GameState.Value = GameState.Paused;
		this.SendEvent<GameStateChangeEvent>(new GameStateChangeEvent(GameState.Paused));
	}
}

/// <summary>
/// 恢复游戏命令
/// </summary>
public class ResumeGameCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		GlobalSystem globalSystem = this.GetSystem<GlobalSystem>();
		globalSystem.GameSingleton.GameEntity.GameState.Value = GameState.Running;
		this.SendEvent<GameStateChangeEvent>(new GameStateChangeEvent(GameState.Running));
	}
}

/// <summary>
/// 使用道具命令
/// </summary>
public class UseItemCommand : AbstractCommand
{
	private int _itemIndex;

	public UseItemCommand(int itemIndex)
	{
		_itemIndex = itemIndex;
	}

	protected override void OnExecute()
	{
		// 实现使用道具逻辑
	}
}

/// <summary>
/// 使用技能命令
/// </summary>
public class UseSkillCommand : AbstractCommand
{
	private int _skillIndex;

	public UseSkillCommand(int skillIndex)
	{
		_skillIndex = skillIndex;
	}

	protected override void OnExecute()
	{
		// 实现使用技能逻辑
	}
}


public class GamePauseCommand : AbstractCommand
{

	protected override void OnExecute()
	{
		this.SendEvent<GamePauseEvent>();
	}
}

public class GameResetCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<GameResetEvent>();
	}
}

public class GameOverCommand : AbstractCommand
{
	protected override void OnExecute()
	{
		this.SendEvent<GameOverEvent>();
	}
}