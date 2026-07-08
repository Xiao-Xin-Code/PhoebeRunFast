using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class GameOverController : BaseController
{
	[SerializeField] GameOverView _view;


	protected override void OnInit()
	{
		base.OnInit();

		_view.RegisterTryAgainPressed(OnTryAgainPressed);
		_view.RegisterExitPressed(OnExitPressed);

		this.RegisterEvent<GameOverEvent>(OnGameOverActive);

		gameObject.SetActive(false);
	}



	private void OnTryAgainPressed()
	{
		//开启转场
		//清除资源
		//加载
	}

	private void OnExitPressed()
	{
		
	}



	private void OnGameOverActive(GameOverEvent evt)
	{
		_view.StateInit();
		gameObject.SetActive(true);
		_view.ActiveSequence(true).Play();
	}




	protected override void OnDeInit()
	{
		base.OnDeInit();

		_view.UnRegisterTryAgainPressed(OnTryAgainPressed);
		_view.UnRegisterExitPressed(OnExitPressed);
	}


}
