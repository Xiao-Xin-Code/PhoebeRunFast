using System.Collections;
using System.Collections.Generic;
using QMVC;
using UnityEngine;

public class LotteryController : BaseController
{
	[SerializeField] LotteryView _view;


	protected override void OnInit()
	{
		base.OnInit();

		this.RegisterEvent<LotteryActiveEvent>(OnLotteryActive);

		gameObject.SetActive(false);
	}
	




	private void Lottery()
	{

	}




	private void OnLotteryActive(LotteryActiveEvent evt)
	{
		gameObject.SetActive(evt.isActive);
	}



	protected override void OnDeInit()
	{
		base.OnDeInit();

		this.UnRegisterEvent<LotteryActiveEvent>(OnLotteryActive);
	}


}