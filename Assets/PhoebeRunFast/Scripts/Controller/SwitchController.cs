using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : BaseController
{
	[SerializeField] SwitchView _view;

	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();
		gameObject.SetActive(false);
	}
}
