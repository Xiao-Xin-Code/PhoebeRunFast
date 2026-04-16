using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

/// <summary>
/// 小鸟控制器
/// </summary>
public class BirdController : BaseController
{
	[SerializeField] BirdView _view;

	[SerializeField] FlyBirdController flyBird;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		// 注册输入事件
		this.RegisterEvent<BirdInputDownEvent>(OnBirdUp);
	}

	/// <summary>
	/// 小鸟上升事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	void OnBirdUp(BirdInputDownEvent evt)
	{
		if(flyBird.FlyBirdState == FlyBirdState.Run)
		{
			_view.RB.velocity = Vector2.up * 600f;
		}
	}

	/// <summary>
	/// 状态初始化
	/// </summary>
	public void StateInit()
	{
		_view.RB.velocity = Vector2.zero;
		_view.RectTransform.anchoredPosition = Vector2.zero;
	}

	/// <summary>
	/// 设置是否为运动学刚体
	/// </summary>
	/// <param name="isKinematic">是否为运动学刚体</param>
	public void SetIsKinematic(bool isKinematic)
	{
		_view.RB.isKinematic = isKinematic;
	}

	/// <summary>
	/// 触发碰撞事件
	/// </summary>
	/// <param name="collision">碰撞体</param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Pipe" || collision.tag == "Boundary")
		{
			flyBird.FlyBirdState = FlyBirdState.Over;
		}
	}

}
