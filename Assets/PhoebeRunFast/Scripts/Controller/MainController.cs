using System.Collections;
using QMVC;
using UnityEngine;

/// <summary>
/// 主控制器
/// </summary>
public class MainController : BaseController
{
	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		// 注册系统事件
		this.RegisterEvent<LoadMainEvent>(OnLoadMain);
		this.RegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
		this.RegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}

	/// <summary>
	/// 主界面初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnMainInitByTransitionOver(MainInitByTransitionOverEvent evt)
	{

	}

	/// <summary>
	/// 加载主界面事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadMain(LoadMainEvent evt)
	{
		evt.enumerator = MainAssetLoad();
	}

	/// <summary>
	/// 卸载主界面事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadMain(UnLoadMainEvent evt)
	{
		evt.enumerator = MainAssetUnLoad();
	}

	/// <summary>
	/// 加载主界面资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator MainAssetLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 卸载主界面资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator MainAssetUnLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();

		// 注销事件
		this.UnRegisterEvent<LoadMainEvent>(OnLoadMain);
		this.UnRegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
		this.UnRegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}

}
