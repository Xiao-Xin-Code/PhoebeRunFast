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
		_view.RegisterClosePressed(OnClosePressed);

		this.RegisterEvent<SettingActiveEvent>(OnSettingActive);

		gameObject.SetActive(false);
	}


	

	private void OnClosePressed()
	{
		_view.ActiveSequence(false).Play();
	}


	private void OnSettingActive(SettingActiveEvent evt)
	{
		_view.StateInit();
		_view.ActiveSequence(evt.isActive).Play();
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();
		_view.UnRegisterClosePressed(OnClosePressed);
		this.UnRegisterEvent<SettingActiveEvent>(OnSettingActive);
	}

}
