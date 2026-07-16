using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class BackPackController : BaseController
{
	[SerializeField] BackPackView _view;


	protected override void OnInit()
	{
		base.OnInit();

		this.RegisterEvent<BackPackActiveEvent>(OnBackPackActive);

		gameObject.SetActive(false);
	}



    private void OnBackPackActive(BackPackActiveEvent evt)
    {
		//需要获取背包数据
		//然后根据背包数据生成背包物品
		//这个背包格子的名称（这个格子直接包含格子和物品） 


        gameObject.SetActive(evt.isActive);
    }


	protected override void OnDeInit()
	{
		base.OnDeInit();
		this.UnRegisterEvent<BackPackActiveEvent>(OnBackPackActive);
	}


}
