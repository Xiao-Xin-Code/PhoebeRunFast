using System.Collections;
using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 游戏控制器
/// </summary>
public class GameController : BaseController
{
	[SerializeField] GameView _view;

	GlobalSystem _globalSystem;


	GameEntity _entity;
	public GameEntity GameEntity => _entity;


	public Transform[] GetLines()
	{
		return _view.Lanes;
	}

	public Transform GetLine(int line)
	{
		if(line < 0 || line >= _view.Lanes.Length)
		{
			Debug.LogError("line out of range");
			return null;
		}
		return _view.Lanes[line];
	}




	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetGameSingleton(this);


		// 注册系统事件
		this.RegisterEvent<LoadGameEvent>(OnLoadGame);
		this.RegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.RegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);
		this.RegisterEvent<GameResetEvent>(OnGameReset);

		_entity = new GameEntity();
	}

	/// <summary>
	/// 游戏初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnGameInitByTransitionOver(GameInitByTransitionOverEvent evt)
	{
		//TODO: 初始可以是一个开场动画（实时）
		//播放
		Debug.Log("开场动画");
	}

	private void OnGameReset(GameResetEvent evt)
	{
		MonoService.Instance.StartCoroutine(GameReset());
	}


	/// <summary>
	/// 加载游戏事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadGame(LoadGameEvent evt)
	{
		evt.enumerator = GameAssetLoad();
	}

	/// <summary>
	/// 卸载游戏事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadGame(UnLoadGameEvent evt)
	{
		evt.enumerator = GameAssetUnLoad();
	}

	/// <summary>
	/// 加载游戏资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator GameAssetLoad()
	{
		//TODO: 加载资源
		//TODO: 初始一段环境
		//TODO: 初始Player
		//TODO: 根据需要加载的角色获取角色数据
		//TODO：根据获取的角色数据，计算角色属性 赋值RoleData

		yield return new WaitForSeconds(5f);
	}

	/// <summary>
	/// 卸载游戏资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator GameAssetUnLoad()
	{
		yield return new WaitForSeconds(5f);
	}



	IEnumerator GameReset()
	{
		yield return new WaitForSeconds(5f);
		this.SendCommand(new CloseTransitionCommand(() => OnGameInitByTransitionOver(null)));
	}



	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();

		_globalSystem.SetGameSingleton(null);

		// 注销事件
		this.UnRegisterEvent<LoadGameEvent>(OnLoadGame);
		this.UnRegisterEvent<UnLoadGameEvent>(OnUnLoadGame);
		this.UnRegisterEvent<GameInitByTransitionOverEvent>(OnGameInitByTransitionOver);

	}

}
