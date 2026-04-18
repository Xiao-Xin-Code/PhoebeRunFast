
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


