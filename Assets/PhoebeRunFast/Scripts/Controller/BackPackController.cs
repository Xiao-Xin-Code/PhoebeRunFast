using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class BackPackController : BaseController
{
	protected override void OnInit()
	{
		base.OnInit();

		this.RegisterEvent<BackPackActiveEvent>(OnBackPackActive);

		gameObject.SetActive(false);
	}



    private void OnBackPackActive(BackPackActiveEvent evt)
    {

        gameObject.SetActive(evt.isActive);
    }


	protected override void OnDeInit()
	{
		base.OnDeInit();
		this.UnRegisterEvent<BackPackActiveEvent>(OnBackPackActive);
	}


}
