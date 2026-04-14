using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class ShopController : BaseController
{

	protected override void OnInit()
	{
		base.OnInit();
		this.RegisterEvent<ShopActiveEvent>(OnShopActive);
		gameObject.SetActive(false);

	}



    private void OnShopActive(ShopActiveEvent evt)
    {
        gameObject.SetActive(evt.isActive);
    }
}
