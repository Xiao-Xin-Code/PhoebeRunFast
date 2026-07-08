using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class PauseController : BaseController
{
    [SerializeField] PauseView _view;

    GlobalSystem _globalSystem;
    GlobalModel _globalModel;

	protected override void OnInit()
	{
		base.OnInit();
        _globalSystem = this.GetSystem<GlobalSystem>();
		_globalModel = this.GetModel<GlobalModel>();

        _view.RegisterContinuePressed(OnContinuePressed);
        _view.RegisterResetPressed(OnResetPressed);
        _view.RegisterExitPressed(OnExitPressed);

        this.RegisterEvent<GamePauseEvent>(OnGamePause);
        gameObject.SetActive(false);
	}




    private void OnContinuePressed()
    {
		Sequence sequence = _view.ActiveSequence(false);
        sequence.AppendInterval(3f);
        //关闭-启动倒计时-结束后开启
        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            _globalSystem.GameSingleton.GameEntity.GameState.Value = GameState.Running;
        });
        sequence.Play();
	}

    private void OnResetPressed()
    {
        this.SendCommand(new OpenTransitionCommand(OnReset));
	}

    private void OnExitPressed()
    {
        this.SendCommand(new OpenTransitionCommand(() => _globalModel.Stage.Value = Stage.Main));
    }



    private void OnGamePause(GamePauseEvent evt)
    {
        _globalSystem.GameSingleton.GameEntity.GameState.Value = GameState.Paused;
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
        this.UnRegisterEvent<GamePauseEvent>(OnGamePause);
	}

}
