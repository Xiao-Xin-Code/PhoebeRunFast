using System;
using DG.Tweening;
using QMVC;
using UnityEngine;

public class TransitionController : BaseController
{
    [SerializeField] TransitionView _view;


	protected override void OnInit()
	{
		base.OnInit();

        this.RegisterEvent<OpenTransitionEvent>(OnOpenTransition);
		this.RegisterEvent<CloseTransitionEvent>(OnCloseTransition);
		this.RegisterEvent<UpdateProgressEvent>(OnUpdateProgress);
		this.RegisterEvent<UpdateProgressDeltaEvent>(OnUpdateProgressDelta);

        gameObject.SetActive(false);
	}



    private void OnOpenTransition(OpenTransitionEvent evt)
    {
		_view.StateInit();
		gameObject.SetActive(true);
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = _view.MaskSequence(true);
		Sequence iconSequence = _view.IconSequence(true);
		mainSequence.Append(maskSequence);
		mainSequence.Append(iconSequence);
		mainSequence.OnComplete(evt.action);
		mainSequence.Play();
	}

    private void OnCloseTransition(CloseTransitionEvent evt)
    {
		_view.SetIconTextActive(false);
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = _view.MaskSequence(false);//遮挡
		Sequence iconSequence = _view.IconSequence(false);//图标
		mainSequence.Append(iconSequence);
		mainSequence.Append(maskSequence);
		mainSequence.OnComplete(() =>
		{
			evt.action?.Invoke();
			gameObject.SetActive(false);
		});
		mainSequence.Play();
	}

    private void OnUpdateProgress(UpdateProgressEvent evt)
    {
        _view.Progress(evt.progress);
    }

	private void OnUpdateProgressDelta(UpdateProgressDeltaEvent evt)
	{
		_view.ProgressDelta(evt.delta);
	}


    public void ToTransition(ref Action<float> progress,ref Action transitionover,Action loadaction)
    {
        _view.StateInit();
        Sequence mainSequence = DOTween.Sequence();
        Sequence maskSequence = _view.MaskSequence(true);
        Sequence iconSequence = _view.IconSequence(true);
        iconSequence.OnComplete(() => loadaction?.Invoke());
        mainSequence.Append(maskSequence);
        mainSequence.Append(iconSequence);

        progress = _view.Progress;
        transitionover = TransitionOver;

        mainSequence.Play();
    }

    private void TransitionOver()
    {
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = _view.MaskSequence(false);//遮挡
		Sequence iconSequence = _view.IconSequence(false);//图标
		mainSequence.Append(iconSequence);
		mainSequence.AppendInterval(0.1f);
		mainSequence.Append(maskSequence);
		mainSequence.OnComplete(() => gameObject.SetActive(false));
		mainSequence.Play();
	}

}
