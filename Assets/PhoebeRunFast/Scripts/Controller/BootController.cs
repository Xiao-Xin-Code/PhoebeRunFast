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

		MonoService.Instance.RemoveAllUpdateListeners();
	}

	private void Start()
	{
		//this.SendCommand(new OpenTransitionCommand(() => _gameModel.Stage.Value = Stage.Main));
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();
		_globalSystem.SetBootSingleton(null);
	}



}
