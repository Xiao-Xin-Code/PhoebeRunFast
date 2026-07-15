using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrizeController : BaseController,
    IPointerDownHandler,IPointerUpHandler
    
{
    [SerializeField] PrizeView _view;

    public void OnPointerDown(PointerEventData eventData)
    {
        ShakeAndFlip();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void ShakeAndFlip()
    {
        Sequence mainSequence = DOTween.Sequence();
        Sequence shakeSequence = _view.ShakeSequence();
        Sequence flipSequence = _view.FlipSequence();
        mainSequence.Append(shakeSequence);
        mainSequence.Append(flipSequence);
        mainSequence.Play();
    }

}