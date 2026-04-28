using QMVC;

public class GameEntity : IEntity
{
	public BindableProperty<GameState> GameState = new BindableProperty<GameState>(global::GameState.Ready);

	public RoleData roleData = new RoleData();


	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
