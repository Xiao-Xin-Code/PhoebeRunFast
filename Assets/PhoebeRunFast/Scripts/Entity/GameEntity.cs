using QMVC;

public class GameEntity : IEntity
{
	public BindableProperty<GameState> GameState = new BindableProperty<GameState>(global::GameState.Ready);


	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}
