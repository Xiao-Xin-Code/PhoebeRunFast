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



public class SetPlayerRoleCommand : AbstractCommand
{
	RoleController role;

	public SetPlayerRoleCommand(RoleController role)
	{
		this.role = role;
	}

	protected override void OnExecute()
	{
		UnityEngine.Debug.Log("设置：" + role);
		this.SendEvent(new SetPlayerRoleEvent(role));
	}
}


public class SetHealthCommand : AbstractCommand
{
	private float health;

	public SetHealthCommand(float health)
	{
		this.health = health;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetHealthEvent(health));
	}
}

public class SetEnergyCommand : AbstractCommand
{
	private float energy;

	public SetEnergyCommand(float energy)
	{
		this.energy = energy;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetEnergyEvent(energy));
	}
}

public class SetAttackCommand : AbstractCommand
{
	private float attack;

	public SetAttackCommand(float attack)
	{
		this.attack = attack;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetAttackEvent(attack));
	}
}

public class SetDefenseCommand : AbstractCommand
{
	private float defense;

	public SetDefenseCommand(float defense)
	{
		this.defense = defense;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetDefenseEvent(defense));
	}
}

public class SetSpeedCommand : AbstractCommand
{
	private float speed;

	public SetSpeedCommand(float speed)
	{
		this.speed = speed;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetSpeedEvent(speed));
	}
}

public class SetCooldownReductionCommand : AbstractCommand
{
	private float cooldownReduction;

	public SetCooldownReductionCommand(float cooldownReduction)
	{
		this.cooldownReduction = cooldownReduction;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetCooldownReductionEvent(cooldownReduction));
	}
}

public class SetEnergyRecoveryRateCommand : AbstractCommand
{
	private float energyRecoveryRate;

	public SetEnergyRecoveryRateCommand(float energyRecoveryRate)
	{
		this.energyRecoveryRate = energyRecoveryRate;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new SetEnergyRecoveryRateEvent(energyRecoveryRate));
	}
}



#region 玩家状态设置显示设置命令

public class UpdateStatusMaxHealthCommand : AbstractCommand
{
	private float maxHealth;

	public UpdateStatusMaxHealthCommand(float maxHealth)
	{
		this.maxHealth = maxHealth;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateStatusMaxHealthEvent(maxHealth));
	}
}

public class UpdateStatusCurHealthCommand : AbstractCommand
{
	private float currentHealth;

	public UpdateStatusCurHealthCommand(float currentHealth)
	{
		this.currentHealth = currentHealth;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateStatusCurHealthEvent(currentHealth));
	}
}

public class UpdateStatusLossCommand : AbstractCommand
{
	private float lossHealth;

	public UpdateStatusLossCommand(float lossHealth)
	{
		this.lossHealth = lossHealth;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateStatusLossEvent(lossHealth));
	}
}

public class UpdateStatusMaxEnergyCommand : AbstractCommand
{
	private float maxEnergy;

	public UpdateStatusMaxEnergyCommand(float maxEnergy)
	{
		this.maxEnergy = maxEnergy;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateStatusMaxEnergyEvent(maxEnergy));
	}
}

public class UpdateStatusCurEnergyCommand : AbstractCommand
{
	private float currentEnergy;

	public UpdateStatusCurEnergyCommand(float currentEnergy)
	{
		this.currentEnergy = currentEnergy;
	}

	protected override void OnExecute()
	{
		this.SendEvent(new UpdateStatusCurEnergyEvent(currentEnergy));
	}
}


#endregion

