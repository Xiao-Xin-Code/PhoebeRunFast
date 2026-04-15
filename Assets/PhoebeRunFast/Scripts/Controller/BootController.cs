using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

public class BootController : BaseController
{
	[SerializeField] BootView _view;

	GameModel _gameModel;
	protected override void OnInit()
	{
		base.OnInit();

		MonoService.Instance.RemoveAllUpdateListeners();
	}

	private void Start()
	{
		//this.SendCommand(new OpenTransitionCommand(() => _gameModel.Stage.Value = Stage.Main));
	}




}
