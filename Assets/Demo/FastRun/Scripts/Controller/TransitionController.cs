using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public Image mask;
    public Image icon;
    public TMP_Text iconText;

    VertexGradient gradient;



	private void Init()
    {
        mask.material.SetFloat("_Radius", 1);
        icon.rectTransform.localScale = Vector3.zero;
		gradient = new VertexGradient();
		gradient.topLeft = new Color(1, 1, 1, 0);
		gradient.topRight = new Color(1, 1, 1, 0);
		gradient.bottomLeft = new Color(1, 1, 1, 0);
		gradient.bottomRight = new Color(1, 1, 1, 0);
		iconText.colorGradient = gradient;
        iconText.gameObject.SetActive(true);
    }


    Sequence MaskSequence(bool isOpen)
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

    Sequence IconSequence(bool isOpen)
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

    Sequence TextSequence(bool isOpen)
    {
        Sequence sequence = DOTween.Sequence();
        if (isOpen)
        {
			sequence.Append(DOTween.To(() => 0f, x =>
			{
				gradient.topLeft = new Color(1, 1, 1, x);
				gradient.topRight = new Color(1, 1, 1, x);
			}, 1, 1).SetEase(Ease.Linear).OnUpdate(() => iconText.colorGradient = gradient));
		}
        else
        {
			sequence.Append(DOTween.To(() => 1f, x =>
			{
                gradient.topLeft = new Color(1, 1, 1, x);
				gradient.topRight = new Color(1, 1, 1, x);
			}, 0, 1).SetEase(Ease.Linear).OnUpdate(() => iconText.colorGradient = gradient));
		}
        return sequence;
	}


    Sequence TransitionSequence(bool isOpen)
    {
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = MaskSequence(isOpen);//遮挡
		Sequence iconSequence = IconSequence(isOpen);//图标
		mainSequence.Append(maskSequence);
		mainSequence.AppendInterval(0.1f);
		mainSequence.Append(iconSequence);
        return mainSequence;
	}

    private void Progress(float value)
    {
		float top = value >= 0.5f ? 1 : value * 2;
		float bottom = value > 0.5f ? 2 * (value - 0.5f) : 0;
		gradient.topLeft = new Color(1, 1, 1, top);
		gradient.topRight = new Color(1, 1, 1, top);
		gradient.bottomLeft = new Color(1, 1, 1, bottom);
		gradient.bottomRight = new Color(1, 1, 1, bottom);
		iconText.colorGradient = gradient;
	}

	private void ProgressOver()
	{
		iconText.gameObject.SetActive(false);
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = MaskSequence(false);//遮挡
		Sequence iconSequence = IconSequence(false);//图标
		mainSequence.Append(iconSequence);
		mainSequence.AppendInterval(0.1f);
		mainSequence.Append(maskSequence);
		mainSequence.OnComplete(() => gameObject.SetActive(false));
		mainSequence.Play();
	}


	public void ToTransition(ref UnityAction<float> progress,ref UnityAction progressover,UnityAction loadaction)
    {
        gameObject.SetActive(true);
        Init();
		Sequence mainSequence = DOTween.Sequence();
		Sequence maskSequence = MaskSequence(true);//遮挡
		Sequence iconSequence = IconSequence(true);//图标
		iconSequence.OnComplete(() => loadaction.Invoke());//触发加载程序
        //Sequence textSequence = TextSequence(true);//一半文字
		mainSequence.Append(maskSequence);
		mainSequence.AppendInterval(0.1f);
		mainSequence.Append(iconSequence);
        //mainSequence.Append(textSequence);

        progress = Progress;
		progressover = ProgressOver;

		mainSequence.Play();
    }


   
}
