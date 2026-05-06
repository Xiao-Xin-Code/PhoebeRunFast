using QMVC;

public class GameEntity : IEntity
{
	public BindableProperty<GameState> GameState = new BindableProperty<GameState>(global::GameState.Ready);

	#region 属性

	public BindableProperty<string> RoleId = new BindableProperty<string>();
	public BindableProperty<float> Health = new BindableProperty<float>();
	public BindableProperty<float> Energy = new BindableProperty<float>();
	public BindableProperty<float> Attack = new BindableProperty<float>();
	public BindableProperty<float> Defense = new BindableProperty<float>();
	public BindableProperty<float> Speed = new BindableProperty<float>();
	public BindableProperty<float> CoolDownReduction = new BindableProperty<float>();

	#endregion





	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
