using Frame;
using QMVC;

/// <summary>
/// 游戏模型
/// </summary>
public class GameModel : AbstractModel
{
	/// <summary>
	/// 玩家当前生命值
	/// </summary>
	public BindableProperty<float> PlayerHealth = new BindableProperty<float>(100f);

	/// <summary>
	/// 玩家最大生命值
	/// </summary>
	public BindableProperty<float> PlayerMaxHealth = new BindableProperty<float>(100f);

	/// <summary>
	/// 玩家当前魔法值
	/// </summary>
	public BindableProperty<float> PlayerMana = new BindableProperty<float>(50f);

	/// <summary>
	/// 玩家最大魔法值
	/// </summary>
	public BindableProperty<float> PlayerMaxMana = new BindableProperty<float>(50f);

	/// <summary>
	/// 游戏状态
	/// </summary>
	public BindableProperty<GameState> GameState = new BindableProperty<GameState>(global::GameState.Ready);

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		// 初始化游戏状态
		GameState.Value = global::GameState.Ready;
		
		// 初始化玩家属性
		PlayerHealth.Value = PlayerMaxHealth.Value;
		PlayerMana.Value = PlayerMaxMana.Value;
	}
}
