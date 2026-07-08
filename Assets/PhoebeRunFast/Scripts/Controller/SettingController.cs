using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class SettingController : BaseController
{
    [SerializeField] SettingView _view;




	protected override void OnInit()
	{
		base.OnInit();


		this.RegisterEvent<SettingActiveEvent>(OnSettingActive);

		gameObject.SetActive(false);
	}


	


	private void OnSettingActive(SettingActiveEvent evt)
	{
		_view.StateInit();
		_view.gameObject.SetActive(true);
		_view.ActiveSequence(evt.isActive).Play();
	}



}
