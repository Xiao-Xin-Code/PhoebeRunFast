using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeMenuView : MonoBehaviour
{
     [SerializeField] Button begin;
	[SerializeField] Button setBtn;

	#region Register

	/// <summary>
	/// 注册开始按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterBeginPressed(UnityAction action)
	{
		begin.onClick.AddListener(action);
	}

	/// <summary>
	/// 注册设置按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void RegisterSetPressed(UnityAction action)
	{
		setBtn.onClick.AddListener(action);
	}

	#endregion

	#region UnRegister

	/// <summary>
	/// 注销开始按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterBeginPressed(UnityAction action)
	{
		begin.onClick.RemoveListener(action);
	}

	/// <summary>
	/// 注销设置按钮点击事件
	/// </summary>
	/// <param name="action">回调函数</param>
	public void UnRegisterSetPressed(UnityAction action)
	{
		setBtn.onClick.RemoveListener(action);
	}

	#endregion
}