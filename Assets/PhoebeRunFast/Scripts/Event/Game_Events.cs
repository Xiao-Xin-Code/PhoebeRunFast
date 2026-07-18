
/// <summary>
/// 生命值变化事件
/// </summary>
public class HealthChangeEvent
{
	public float currentHealth;
	public float maxHealth;

	public HealthChangeEvent(float currentHealth, float maxHealth)
	{
		this.currentHealth = currentHealth;
		this.maxHealth = maxHealth;
	}
}

/// <summary>
/// 魔法值变化事件
/// </summary>
public class ManaChangeEvent
{
	public float currentMana;
	public float maxMana;

	public ManaChangeEvent(float currentMana, float maxMana)
	{
		this.currentMana = currentMana;
		this.maxMana = maxMana;
	}
}



#region 属性设置事件

public class SetHealthEvent
{
	public float health;

	public SetHealthEvent(float health)
	{
		this.health = health;
	}
}

public class SetEnergyEvent
{
	public float energy;

	public SetEnergyEvent(float energy)
	{
		this.energy = energy;
	}
}

public class SetAttackEvent
{
	public float attack;

	public SetAttackEvent(float attack)
	{
		this.attack = attack;
	}
}

public class SetDefenseEvent
{
	public float defense;

	public SetDefenseEvent(float defense)
	{
		this.defense = defense;
	}
}

public class SetSpeedEvent
{
	public float speed;

	public SetSpeedEvent(float speed)
	{
		this.speed = speed;
	}
}

public class SetCooldownReductionEvent
{
	public float cooldownReduction;

	public SetCooldownReductionEvent(float cooldownReduction)
	{
		this.cooldownReduction = cooldownReduction;
	}
}

public class SetEnergyRecoveryRateEvent
{
	public float energyRecoveryRate;

	public SetEnergyRecoveryRateEvent(float energyRecoveryRate)
	{
		this.energyRecoveryRate = energyRecoveryRate;
	}
}


#endregion


#region 玩家状态设置显示设置事件

public class UpdateStatusMaxHealthEvent
{
	public float maxHealth;

	public UpdateStatusMaxHealthEvent(float maxHealth)
	{
		this.maxHealth = maxHealth;
	}
}

public class UpdateStatusCurHealthEvent
{
	public float currentHealth;

	public UpdateStatusCurHealthEvent(float currentHealth)
	{
		this.currentHealth = currentHealth;
	}
}

/// <summary>
/// 更新损失血量上限事件
/// </summary>
public class UpdateStatusLossEvent
{
	public float lossHealth;

	public UpdateStatusLossEvent(float lossHealth)
	{
		this.lossHealth = lossHealth;
	}
}

public class UpdateStatusMaxEnergyEvent
{
	public float maxEnergy;

	public UpdateStatusMaxEnergyEvent(float maxEnergy)
	{
		this.maxEnergy = maxEnergy;
	}
}

public class UpdateStatusCurEnergyEvent
{
	public float currentEnergy;

	public UpdateStatusCurEnergyEvent(float currentEnergy)
	{
		this.currentEnergy = currentEnergy;
	}
}






#endregion











/// <summary>
/// 游戏状态变化事件
/// </summary>
public class GameStateChangeEvent
{
	public GameState newState;

	public GameStateChangeEvent(GameState newState)
	{
		this.newState = newState;
	}
}





public class GamePauseEvent
{

}

public class GameResetEvent
{

}

public class GameOverEvent
{

}



public class SetPlayerRoleEvent
{
	public RoleController role;

	public SetPlayerRoleEvent(RoleController role)
	{
		this.role = role;
	}
}



