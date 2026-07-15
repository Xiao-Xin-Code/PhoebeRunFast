
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PrizeView : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    [SerializeField] Image backImage;
    [SerializeField] Image frontImage;


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
            backImage.gameObject.SetActive(false);
            frontImage.gameObject.SetActive(true);
        });
        flipSequence.Append(rectTransform.DORotate(endRot,0.5f).SetEase(Ease.InOutQuad));
        return flipSequence;
    }


}