using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

public class BootController : BaseController
{
	[SerializeField] BootView _view;

	GlobalSystem _globalSystem;


	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetBootSingleton(this);

		MonoService.Instance.Init();



		this.SendCommand(new OpenTransitionCommand(StageChanged));
	}


	/// <summary>
	/// 阶段切换方法
	/// </summary>
	private void StageChanged()
	{
		_globalSystem.GlobalModel.Stage.Value = Stage.Home;
	}
	
	public void SetMaskVisible(bool visible)
	{
		_view.SetMaskVisible(visible);
	}


	//TODO: 加载出转场面板，设置面板

	protected override void OnDeInit()
	{
		base.OnDeInit();
		_globalSystem.SetBootSingleton(null);
	}



}
