using System.Collections;
using QMVC;
using UnityEngine;

/// <summary>
/// 主页控制器
/// </summary>
public class HomeController : BaseController
{
    [SerializeField] HomeView _view;

	GlobalSystem _globalSystem;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetHomeSingleton(this);



		// 注册系统事件
		this.RegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.RegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
		this.RegisterEvent<HomeInitByTransitionOverEvent>(OnHomeInitByTransitionOver);

	}



	

	/// <summary>
	/// 主页初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnHomeInitByTransitionOver(HomeInitByTransitionOverEvent evt)
	{

	}

	/// <summary>
	/// 加载主页事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadHome(LoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetLoad();
	}

	/// <summary>
	/// 卸载主页事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadHome(UnLoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetUnLoad();
	}

	/// <summary>
	/// 加载主页资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator HomeAssetLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 卸载主页资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator HomeAssetUnLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();
		_globalSystem.SetHomeSingleton(null);

		// 注销事件
		this.UnRegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.UnRegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
		this.UnRegisterEvent<HomeInitByTransitionOverEvent>(OnHomeInitByTransitionOver);
	}

}
