using QMVC;

public class FastRun : Architecture<FastRun>
{
	protected override void Init()
	{


		RegisterSystem<LuaSystem>(new LuaSystem());
		RegisterSystem<AssetSystem>(new AssetSystem());
		RegisterSystem<AudioSystem>(new AudioSystem());
		RegisterSystem<RoadSystem>(new RoadSystem());


	}
}
