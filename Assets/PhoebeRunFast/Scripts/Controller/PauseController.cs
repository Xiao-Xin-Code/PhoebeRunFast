using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class PauseController : BaseController
{
    [SerializeField] PauseView _view;

    BootModel _bootModel;

	protected override void OnInit()
	{
		base.OnInit();

        _bootModel = this.GetModel<BootModel>();

        _view.RegisterContinuePressed(OnContinuePressed);
        _view.RegisterResetPressed(OnResetPressed);
        _view.RegisterExitPressed(OnExitPressed);

        this.RegisterEvent<GamePauseEvent>(OnGamePause);
        gameObject.SetActive(false);
	}




    private void OnContinuePressed()
    {
		Sequence sequence = _view.ActiveSequence(false);
        //关闭-启动倒计时-结束后开启
        sequence.OnComplete(()=>gameObject.SetActive(false));
        sequence.Play();
	}

    private void OnResetPressed()
    {
        this.SendCommand(new OpenTransitionCommand(OnReset));
	}

    private void OnExitPressed()
    {
        this.SendCommand(new OpenTransitionCommand(() => _bootModel.Stage.Value = Stage.Main));
    }



    private void OnGamePause(GamePauseEvent evt)
    {
        _view.StateInit();
        gameObject.SetActive(true);
        _view.ActiveSequence(true).Play();
    }



    private void OnReset()
    {
        Sequence sequence = _view.ActiveSequence(false);
        sequence.OnComplete(() => this.SendCommand<GameResetCommand>());
        sequence.Play();
    }




	protected override void OnDeInit()
	{
		base.OnDeInit();

        _view.UnRegisterContinuePressed(OnContinuePressed);
        _view.UnRegisterResetPressed(OnResetPressed);
        _view.UnRegisterExitPressed(OnExitPressed);
	}

}
