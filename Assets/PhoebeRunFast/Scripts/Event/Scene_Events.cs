using System.Collections;

/// <summary>
/// 加载主页事件
/// </summary>
public class LoadHomeEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载主页事件
/// </summary>
public class UnLoadHomeEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 加载主界面事件
/// </summary>
public class LoadMainEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载主界面事件
/// </summary>
public class UnLoadMainEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 加载游戏事件
/// </summary>
public class LoadGameEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 卸载游戏事件
/// </summary>
public class UnLoadGameEvent
{
	public IEnumerator enumerator;
}

/// <summary>
/// 主页初始化完成事件
/// </summary>
public class HomeInitByTransitionOverEvent
{

}

/// <summary>
/// 主界面初始化完成事件
/// </summary>
public class MainInitByTransitionOverEvent
{

}

/// <summary>
/// 游戏初始化完成事件
/// </summary>
public class GameInitByTransitionOverEvent
{

}