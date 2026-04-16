using QMVC;
using UnityEngine;

/// <summary>
/// 控制器基类
/// </summary>
public abstract class BaseController : MonoBehaviour, IController
{
	/// <summary>
	/// 获取架构实例
	/// </summary>
	/// <returns>架构实例</returns>
	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}

	/// <summary>
	/// 唤醒时调用初始化方法
	/// </summary>
	private void Awake()
	{
		OnInit();
	}

	/// <summary>
	/// 销毁时调用反初始化方法
	/// </summary>
	private void OnDestroy()
	{
		OnDeInit();
	}

	/// <summary>
	/// 初始化方法，子类可重写
	/// </summary>
	protected virtual void OnInit() { }

	/// <summary>
	/// 反初始化方法，子类可重写
	/// </summary>
	protected virtual void OnDeInit() { }
}
