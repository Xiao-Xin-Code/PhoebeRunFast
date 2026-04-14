using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class SwitchController : BaseController
{
	[SerializeField] SwitchView _view;

	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();


		_view.RegisterLeftPressed(OnLeftPresed);
		_view.RegisterRightPressed(OnRightPressed);

		gameObject.SetActive(false);
	}



	private void OnLeftPresed()
	{
		this.SendCommand<ToLeftRoleCommand>();
	}


	private void OnRightPressed()
	{
		this.SendCommand<ToRightRoleCommand>();
	}
}
