using QMVC;
using XLua;

public class LuaSystem : AbstractSystem
{
    LuaEnv luaEnv;

	protected override void OnInit()
	{
		luaEnv = new LuaEnv();

		//加载Lua文件


		
	}



	protected override void OnDeinit()
	{
		base.OnDeinit();
		luaEnv.Dispose();
	}
}
