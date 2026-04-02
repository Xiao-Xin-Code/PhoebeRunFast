using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour,
    IPointerDownHandler,IPointerUpHandler
{
	public Image cardFront;      // 卡片正面（图片）
	public Image cardBack;       // 卡片背面（遮盖）

	[Header("动画参数")]
	public float flipDuration = 0.4f;      // 翻转时长
	public float shakeDuration = 0.3f;     // 抖动时长
	public float shakeStrength = 15f;      // 抖动强度
	public int shakeVibrato = 20;          // 抖动次数

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
		Sequence shakeSequence = ShakeSequence();
		Sequence flipSequence = FlipSequence();
		mainSequence.Append(shakeSequence);
		mainSequence.Append(flipSequence);
		mainSequence.Play();
	}



	Sequence ShakeSequence()
	{
		Sequence shakeSequence = DOTween.Sequence();
		shakeSequence.Append(transform.DOShakePosition(2, shakeStrength, shakeVibrato).SetEase(Ease.Linear));
		shakeSequence.Join(transform.DOShakeRotation(2, shakeStrength, shakeVibrato).SetEase(Ease.Linear));
		shakeSequence.Join(transform.DOShakeScale(2, 0.1f, shakeVibrato).SetEase(Ease.Linear));
		return shakeSequence;
	}

	Sequence FlipSequence()
	{
		Sequence flipSequence = DOTween.Sequence();
		Vector3 midRot = new Vector3(0, 90, 0);
		Vector3 endRot = new Vector3(0, 180, 0);

		flipSequence.Append(transform.DORotate(midRot, flipDuration / 2).SetEase(Ease.InOutQuad));
		flipSequence.AppendCallback(() =>
		{
			cardBack.gameObject.SetActive(false);
			cardFront.gameObject.SetActive(true);
		});
		flipSequence.Append(transform.DORotate(endRot, flipDuration / 2).SetEase(Ease.InOutQuad));
		return flipSequence;
	}
}
