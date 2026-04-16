using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

/// <summary>
/// 飞行小鸟实体
/// </summary>
public class FlyBirdEntity : IEntity
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
	/// 飞行小鸟状态
	/// </summary>
	public BindableProperty<FlyBirdState> FlyBirdState = new BindableProperty<FlyBirdState>(global::FlyBirdState.Ready);

	/// <summary>
	/// 全局管道速度
	/// </summary>
	public float globalPipeSpeed = 10f;

}
