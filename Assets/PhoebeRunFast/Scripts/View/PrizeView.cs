
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrizeView : MonoBehaviour,
    IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;

    [SerializeField] Image backImage;
    [SerializeField] Image frontImage;


    event Action<PointerEventData> onPointerDownEvent;
    event Action<PointerEventData> onPointerUpEvent;


    public void SetFront(Sprite sprite)
    {
        frontImage.sprite = sprite;
    }

    public void SetBack(Sprite sprite)
    {
        backImage.sprite = sprite;
    }

    public void SetState(bool flipped)
    {
        rectTransform.rotation = flipped ? Quaternion.Euler(0,180,0) : Quaternion.identity;
        frontImage.gameObject.SetActive(flipped);
        backImage.gameObject.SetActive(!flipped);
    }



    public Sequence ShakeSequence()
    {
        Sequence shakeSequence = DOTween.Sequence();
        shakeSequence.Append(rectTransform.DOShakeAnchorPos(0.1f, 0.1f, 3).SetEase(Ease.Linear));
        shakeSequence.Join(rectTransform.DOShakeRotation(0.1f, 0.1f, 3).SetEase(Ease.Linear));
        shakeSequence.Join(rectTransform.DOShakeScale(0.1f, 0.1f, 3).SetEase(Ease.Linear));
        return shakeSequence;
    }

    public Sequence FlipSequence()
    {
        Sequence flipSequence = DOTween.Sequence();
        Vector3 midRot = new Vector3(0,90,0);
        Vector3 endRot = new Vector3(0,180,0);

        flipSequence.Append(rectTransform.DORotate(midRot,0.5f).SetEase(Ease.InOutQuad));
        flipSequence.AppendCallback(() =>
        {
            SetState(true);
        });
        flipSequence.Append(rectTransform.DORotate(endRot,0.5f).SetEase(Ease.InOutQuad));
        return flipSequence;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDownEvent?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUpEvent?.Invoke(eventData);
    }


    #region Register

    public void RegisterPointerDown(Action<PointerEventData> onPointerDown)
    {
        onPointerDownEvent += onPointerDown;
    }
    
    public void RegisterPointerUp(Action<PointerEventData> onPointerUp)
    {
        onPointerUpEvent += onPointerUp;
    }

    #endregion


    #region UnRegister

    public void UnRegisterPointerDown(Action<PointerEventData> onPointerDown)
    {
        onPointerDownEvent -= onPointerDown;
    }

    public void UnRegisterPointerUp(Action<PointerEventData> onPointerUp)
    {
        onPointerUpEvent -= onPointerUp;
    }

    #endregion




}