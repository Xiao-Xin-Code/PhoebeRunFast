using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 转场视图
/// </summary>
public class TransitionView : MonoBehaviour
{
    [SerializeField] Image mask;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text iconText;

	VertexGradient gradient;

	/// <summary>
	/// 状态初始化
	/// </summary>
	public void StateInit()
    {
        mask.material.SetFloat("_Radius", 1);
        icon.rectTransform.localScale = Vector3.one;
		gradient = new VertexGradient();
		gradient.topLeft = new Color(1, 1, 1, 0);
		gradient.topRight = new Color(1, 1, 1, 0);
		gradient.bottomLeft = new Color(1, 1, 1, 0);
		gradient.bottomRight = new Color(1, 1, 1, 0);
		iconText.colorGradient = gradient;
		iconText.gameObject.SetActive(true);
	}

	/// <summary>
	/// 遮罩动画序列
	/// </summary>
	/// <param name="isOpen">是否打开</param>
	/// <returns>动画序列</returns>
	public Sequence MaskSequence(bool isOpen)
	{
		Sequence sequence = DOTween.Sequence();
		if (isOpen)
		{
			sequence.Append(DOTween.To(
				() => mask.material.GetFloat("_Radius"),
				x => mask.material.SetFloat("_Radius", x),
				0, 0.4f).SetEase(Ease.InOutQuad)
			);
		}
		else
		{
			sequence.Append(DOTween.To(
			   () => mask.material.GetFloat("_Radius"),
			   x => mask.material.SetFloat("_Radius", x),
			   1, 0.5f).SetEase(Ease.OutBack)
		   );
		}
		return sequence;
	}

	/// <summary>
	/// 图标动画序列
	/// </summary>
	/// <param name="isOpen">是否打开</param>
	/// <returns>动画序列</returns>
	public Sequence IconSequence(bool isOpen)
	{
		Sequence sequence = DOTween.Sequence();
		if (isOpen)
		{
			sequence.Append(icon.rectTransform.DOScale(1, 0.25f).SetEase(Ease.OutBack));
		}
		else
		{
			sequence.Append(icon.rectTransform.DOScale(0, 0.25f).SetEase(Ease.OutBack));
		}
		return sequence;
	}

	/// <summary>
	/// 设置进度
	/// </summary>
	/// <param name="value">进度值</param>
	public void Progress(float value)
	{
		float top = value >= 0.5f ? 1 : value * 2;
		float bottom = value > 0.5f ? 2 * (value - 0.5f) : 0;
		gradient.topLeft = new Color(1, 1, 1, top);
		gradient.topRight = new Color(1, 1, 1, top);
		gradient.bottomLeft = new Color(1, 1, 1, bottom);
		gradient.bottomRight = new Color(1, 1, 1, bottom);
		iconText.colorGradient = gradient;
	}

	/// <summary>
	/// 设置进度增量
	/// </summary>
	/// <param name="delta">增量值</param>
	public void ProgressDelta(float delta)
	{
		if (gradient.topLeft.a < 1)
		{
			gradient.topLeft = new Color(1, 1, 1, gradient.topLeft.a + delta);
			gradient.topRight = new Color(1, 1, 1, gradient.topRight.a + delta);
		}
		else
		{
			gradient.bottomLeft = new Color(1, 1, 1, gradient.bottomLeft.a + delta);
			gradient.bottomRight = new Color(1, 1, 1, gradient.bottomRight.a + delta);
		}
		iconText.colorGradient = gradient;
	}

}
