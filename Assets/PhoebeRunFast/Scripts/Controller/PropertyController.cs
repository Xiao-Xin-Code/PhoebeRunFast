using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyController : BaseController
{
	[SerializeField] PropertyView _view;

	protected override void OnInit()
	{
		base.OnInit();
		_view.StateInit();
		gameObject.SetActive(false);
	}
}
