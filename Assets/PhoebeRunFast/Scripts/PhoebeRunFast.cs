using QMVC;

public class PhoebeRunFast : Architecture<PhoebeRunFast>
{
	protected override void Init()
	{
		RegisterModel(new BootModel());
		RegisterModel(new GameModel());

		RegisterSystem(new StageSystem());

	}
}
