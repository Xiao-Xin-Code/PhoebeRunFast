using UnityEngine;
using DG.Tweening;

[ExecuteAlways]
public class CircularMaskEffect : MonoBehaviour
{
	[Header("Settings")]
	public float transitionDuration = 1f;
	public Vector2 centerNormalized = new Vector2(0.5f, 0.5f);
	[Range(0, 1)]
	public float radius = 1f;
	public Color maskColor = Color.black;

	private Material material;
	private Sequence currentSequence;

	void Start()
	{
		// 创建材质
		Shader shader = Shader.Find("Hidden/CircularMask");
		if (shader == null)
		{
			// 如果找不到shader，尝试创建
			Debug.LogError("Please create CircularMask shader first");
			return;
		}
		material = new Material(shader);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (material != null)
		{
			// 更新Shader参数
			Vector2 center = new Vector2(centerNormalized.x * Screen.width, centerNormalized.y * Screen.height);
			float maxRadius = Mathf.Max(Screen.width, Screen.height);
			float currentRadius = radius * maxRadius;

			material.SetVector("_Center", center);
			material.SetFloat("_Radius", currentRadius);
			material.SetColor("_MaskColor", maskColor);
			material.SetVector("_ScreenSize", new Vector2(Screen.width, Screen.height));

			// 应用效果
			Graphics.Blit(src, dest, material);
		}
		else
		{
			Graphics.Blit(src, dest);
		}
	}

	// 圆形缩小
	public Sequence ShrinkCircle(System.Action onComplete = null)
	{
		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();

		currentSequence = DOTween.Sequence();

		Tween radiusTween = DOTween.To(() => radius, x => radius = x, 0, transitionDuration)
									 .SetEase(Ease.InOutQuad);

		currentSequence.Append(radiusTween);

		if (onComplete != null)
			currentSequence.OnComplete(() => onComplete());

		return currentSequence;
	}

	// 圆形扩大
	public Sequence ExpandCircle(System.Action onComplete = null)
	{
		if (currentSequence != null && currentSequence.IsActive())
			currentSequence.Kill();

		currentSequence = DOTween.Sequence();

		Tween radiusTween = DOTween.To(() => radius, x => radius = x, 1, transitionDuration)
									 .SetEase(Ease.InOutQuad);

		currentSequence.Append(radiusTween);

		if (onComplete != null)
			currentSequence.OnComplete(() => onComplete());

		return currentSequence;
	}

	// 完整转场
	public Sequence FullTransition(System.Action onMiddle = null, System.Action onComplete = null)
	{
		Sequence fullSequence = DOTween.Sequence();

		fullSequence.Append(ShrinkCircle(() => {
			if (onMiddle != null) onMiddle();
		}));

		fullSequence.AppendInterval(1f);
		fullSequence.Append(ExpandCircle());

		if (onComplete != null)
			fullSequence.OnComplete(() => onComplete());

		return fullSequence;
	}
}