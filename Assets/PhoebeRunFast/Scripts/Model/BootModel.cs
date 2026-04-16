using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

/// <summary>
/// 引导模型
/// </summary>
public class BootModel : AbstractModel
{
	/// <summary>
	/// 当前阶段
	/// </summary>
	public BindableProperty<Stage> Stage = new BindableProperty<Stage>(global::Stage.Home);

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		
	}
}
