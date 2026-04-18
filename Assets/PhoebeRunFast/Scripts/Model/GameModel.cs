using Frame;
using QMVC;

/// <summary>
/// 游戏模型
/// </summary>
public class GameModel : AbstractModel
{
	/// <summary>
	/// 游戏状态
	/// </summary>
	public BindableProperty<GameState> GameState = new BindableProperty<GameState>(global::GameState.Ready);

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{

	}
}
