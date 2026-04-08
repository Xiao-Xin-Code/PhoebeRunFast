using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TransitionController : BaseController
{
    [SerializeField] TransitionView _view;


    public void ToTransition(ref UnityAction<float> progress,ref UnityAction transitionover,UnityAction loadaction)
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
