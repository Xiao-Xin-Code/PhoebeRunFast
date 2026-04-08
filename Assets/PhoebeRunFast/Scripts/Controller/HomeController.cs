using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : BaseController
{
    HomeView _view;

	protected override void OnInit()
	{
		base.OnInit();
		_view.RegisterBeginPressed(OnBeginPressed);
	}

    private void OnBeginPressed()
    {

    }

	protected override void OnDeInit()
	{
		base.OnDeInit();
		_view.UnRegisterBeginPressed(OnBeginPressed);
	}

}
