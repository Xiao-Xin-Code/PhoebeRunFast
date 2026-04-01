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
		Sequence mainSequence = DOTween.Sequence();

		Sequence shakeSequence = DOTween.Sequence();
		shakeSequence.Append(transform.DOPunchRotation(
		  new Vector3(0, 0, shakeStrength),
		  shakeDuration,
		  shakeVibrato,
		  0.5f
		));
		shakeSequence.Join(transform.DOPunchScale(
			new Vector3(0.1f, 0.1f, 0),
			shakeDuration,
			shakeVibrato,
			0.5f
		));

		// 使用 DOTween 的 DoVirtual 实现 3D 翻转效果
		Sequence flipSequence = DOTween.Sequence();

		// 记录初始旋转
		Vector3 startRot = transform.eulerAngles;
		Vector3 midRot = startRot + new Vector3(0, 90, 0);
		Vector3 endRot = startRot + new Vector3(0, 180, 0);

		// 前半段：旋转到90度（中间状态）
		flipSequence.Append(
			transform.DORotate(midRot, flipDuration / 2).SetEase(Ease.InOutQuad)
		);

		// 在中间状态切换图片（背面→正面）
		flipSequence.AppendCallback(() => {
			cardBack.gameObject.SetActive(false);
			cardFront.gameObject.SetActive(true);

			// 设置卡片正面图片（根据稀有度）
			//SetCardFrontImage();

			// 播放翻转音效
			//AudioManager.Play("CardFlip");
		});

		// 后半段：旋转到180度（完成）
		flipSequence.Append(
			transform.DORotate(endRot, flipDuration / 2).SetEase(Ease.InOutQuad)
		);

		// 翻转完成后回调
		flipSequence.OnComplete(() => {
			// 最终归位
			transform.DORotate(startRot + new Vector3(0, 180, 0), 0);

			// 触发揭开完成事件
			//OnCardRevealed();
		});


		mainSequence.Append(shakeSequence);
		mainSequence.Append(flipSequence);

		mainSequence.Play();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
