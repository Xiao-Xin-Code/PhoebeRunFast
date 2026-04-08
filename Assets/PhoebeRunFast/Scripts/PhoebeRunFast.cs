using QMVC;

public class PhoebeRunFast : Architecture<PhoebeRunFast>
{
	protected override void Init()
	{

		RegisterModel<GameModel>(new GameModel());


	}
}
